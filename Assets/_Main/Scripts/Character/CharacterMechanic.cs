using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.RigidbodyPhysics;
using Telepathy;
using UnityEngine;
using UnityEngine.UI;

public enum Item
{
    FLASHLIGHT,
    KEY,
    MEDKIT,
    Syringe,
    EMPTY
}

public class CharacterMechanic : NetworkBehaviour
{
    //Companents
    public Image[] poisonImage;
    public CharacterMovement player;
    private Hideable hideableObj;
    public GameObject pressE;
    public InventorySystem inventorySystem;
    public Transform hand;
    
    //Debuff
    public bool isPoisonedStage1 = false;
    public bool isPoisonedStage2 = false;
    
    //Item
    public Item selectedItem;
    public List<GameObject> droppedItemPrefab;
    public GameObject[] item; //0 = Flashlight // 1 = Key //  2 = Medkit // 3 = Syringe
    public int selectedGameObject;
    public Flashlight _flashlight;

    [SyncVar(hook = nameof(OnWeaponChanged))]
    public int activeWeaponSynced;


    void Start()
    {
        foreach (var _item in item)
        {
            if (_item != null)
            {
                _item.SetActive(false);
            }
        }
    }


    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        
        Raycast();

        DropItemInHand();

        InventorySlotInput();

        FlashChangeState();
    }

    private void FlashChangeState()
    {
        if (Input.GetKeyDown(KeyCode.F) && selectedGameObject == 0)
        {
            _flashlight.lightOnOff = !_flashlight.lightOnOff;
            CmdFlashChangeState(_flashlight.lightOnOff);
        }
    }
    
    [Command]
    private void CmdFlashChangeState(bool value)
    {
        RpcFlashChangeState(value);
    }
    
    [ClientRpc]
    private void RpcFlashChangeState(bool value)
    {
        _flashlight.light.enabled = value;
    }

    public void DropItemInHand()
    {
        if (Input.GetKeyDown(KeyCode.G) && selectedItem != Item.EMPTY)
        {
            inventorySystem.DeleteItemToSlot(selectedItem);

            CmdSpawnDroppedItemPrefab(selectedGameObject);
        }
    }

    [Command]
    public void CmdSpawnDroppedItemPrefab(int value)
    {
        GameObject spawnDroppedItemPrefab = Instantiate(droppedItemPrefab[value], transform.position + transform.forward + transform.up, Quaternion.identity);
        spawnDroppedItemPrefab.GetComponent<Rigidbody>().AddForce(transform.forward * 10);
        NetworkServer.Spawn(spawnDroppedItemPrefab);
        RpcSpawnDroppedItemPrefab(value);
    }
    
    [ClientRpc]
    public void RpcSpawnDroppedItemPrefab(int value)
    {
        item[value].SetActive(false);
    }
    
    void OnWeaponChanged(int _Old, int _New)
    {
        if (0 <= _Old && _Old < item.Length && item[_Old] != null)
            item[_Old].SetActive(false);
        
        if (0 <= _New && _New < item.Length && item[_New] != null)
            item[_New].SetActive(true);
    }
    
    [Command]
    public void CmdChangeActiveWeapon(int newIndex)
    {
        activeWeaponSynced = newIndex;
    }

    public void SelecetedItem()
    {
        switch (selectedItem)
        {
            case Item.FLASHLIGHT:
                selectedGameObject = 0;
                CmdChangeActiveWeapon(selectedGameObject);
                break;
            case Item.KEY:
                selectedGameObject = 1;
                CmdChangeActiveWeapon(selectedGameObject);
                break;
            case Item.MEDKIT:
                selectedGameObject = 2;
                CmdChangeActiveWeapon(selectedGameObject);
                break;
            case Item.Syringe:
                selectedGameObject = 3;
                CmdChangeActiveWeapon(selectedGameObject);
                break;
        }
    }



    public void AddItemToInventory(Item item)
    {
        inventorySystem.AddItemToSlot(item);
    }

    private void InventorySlotInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inventorySystem.SelectedSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inventorySystem.SelectedSlot(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inventorySystem.SelectedSlot(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            inventorySystem.SelectedSlot(3);
        }
    }
    
    private void Raycast()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        
        if (Physics.Raycast(ray, out RaycastHit hit, 2))
        {
            if (hit.collider.gameObject.layer == 8)
            {
                pressE.SetActive(true);
                
                InteractWithObject(hit.collider.gameObject);
            }

            if (hit.collider.gameObject.layer == 7)
            {
                pressE.SetActive(true);
                
                hideableObj = hit.collider.GetComponent<Hideable>();
            
                if (Input.GetKeyDown(KeyCode.E) && !player.playerHiding)
                {
                    hideableObj.CheckPlayer(netIdentity);
                }
            }
        }

        else
        {
            pressE.SetActive(false);
            
            if (Input.GetKeyDown(KeyCode.E) && player.playerHiding)
            {
                hideableObj.CheckPlayer(netIdentity);
            }
        }
    }
    
    private void InteractWithObject(GameObject colliderGameObject)
    {
        if (colliderGameObject.TryGetComponent(out IInteractable interactableObj))
        {
            interactableObj.Interact(this);
        }
    }

    public void CarryHeavyItemChangeSpeed(bool value)
    {
        if (value)
        {
            player.speed = 1;
        }
        else
        {
            player.speed = 2;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Poison"))
        {
            if (!isPoisonedStage2)
            {
                if (!isPoisonedStage1)
                {
                    StartCoroutine(PoisonDelay(true, 0));
                }
                if (isPoisonedStage1)
                {
                    StartCoroutine(PoisonDelay(true, 1));
                }
            }
        }
    }

    private IEnumerator PoisonDelay(bool value, int i)
    {
        yield return new WaitForSeconds(5);
        
        if (i == 0)
        {
            isPoisonedStage1 = value;
            poisonImage[i].enabled = true;
        }

        else if (i == 1)
        {
            isPoisonedStage2 = value;
            poisonImage[i-1].enabled = false;
            poisonImage[i].enabled = true;
        }
    }
}
