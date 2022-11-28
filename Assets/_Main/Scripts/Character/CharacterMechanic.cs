using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
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
    public CharacterMovement player;
    private Hideable hideableObj;
    public GameObject feedback;
    public InventorySystem inventorySystem;
    public Transform hand;
    
    //Item
    public Item selectedItem;
    public GameObject[] itemPrefab; 
    public List<GameObject> item; //0 = Flashlight // 1 = Key //  2 = Medkit // 3 = Syringe
    public int selectedGameObject;
    [SyncVar(hook = nameof(OnWeaponChanged))]
    public int activeWeaponSynced;
    public Flashlight _flashlight;



    void Start()
    {
        for (int i = 0; i < itemPrefab.Length; i++)
        {
            GameObject spawnedObj = Instantiate(itemPrefab[i], hand);
            item.Add(spawnedObj);
        }
        
        Invoke("SetItemOnStart", 1f);
    }

    void SetItemOnStart()
    {
        foreach (var _item in item)
        {
            if (_item != null)
            {
                _item.SetActive(false);
            }
        }

        _flashlight = item[0].GetComponent<Flashlight>();
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
            _flashlight.CmdOpenFlashlight(_flashlight.lightOnOff);
        }
    }

    public void DropItemInHand()
    {
        if (Input.GetKeyDown(KeyCode.G) && selectedItem != Item.EMPTY && selectedItem != Item.FLASHLIGHT)
        {
            inventorySystem.DeleteItemToSlot(selectedItem);

            item[selectedGameObject].SetActive(false);
        }
    }
    
    void OnWeaponChanged(int _Old, int _New)
    {
        if (0 <= _Old && _Old < item.Count && item[_Old] != null)
            item[_Old].SetActive(false);
        
        if (0 <= _New && _New < item.Count && item[_New] != null)
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
            default:
                selectedGameObject = 0;
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
                feedback.SetActive(true);
                
                InteractWithObject(hit.collider.gameObject);
            }

            if (hit.collider.gameObject.layer == 7)
            {
                feedback.SetActive(true);
                
                hideableObj = hit.collider.GetComponent<Hideable>();
            
                if (Input.GetKeyDown(KeyCode.E) && !player.playerHiding)
                {
                    hideableObj.CheckPlayer(netIdentity);
                }
            }
        }

        else
        {
            feedback.SetActive(false);
            
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
}
