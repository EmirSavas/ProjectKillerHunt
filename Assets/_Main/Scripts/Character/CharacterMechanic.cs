using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public enum Item
{
    FLASHLIGHT,
    KEY,
    MEDKIT,
    SYRINGE,
    EMPTY
}

public class CharacterMechanic : NetworkBehaviour
{
    //Companents
    public Image[] poisonImage;
    public CharacterMovement player;
    public GameObject pressE;
    public InventorySystem inventorySystem;
    public GameObject pauseMenu;

    //Hidable Obj
    private bool _leftDoorOpening = false;
    private bool _rightDoorOpening = false;
    private Hideable _hideableObj;

    //Item
    public Item selectedItem;
    public List<GameObject> droppedItemPrefab;
    public GameObject[] item; //0 = Flashlight // 1 = Key //  2 = Medkit // 3 = Syringe
    public int selectedGameObject;
    public Flashlight _flashlight;
    public int selectedSlot;

    [SyncVar(hook = nameof(OnWeaponChanged))]
    public int activeWeaponSynced;
    
    //PauseMenu
    public bool pauseGame;


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

        PauseMenu();
        
        if (!pauseGame)
        {
            Raycast();
            
            if (!StatusEffect.ContainStatusEffect(StatusEffectType.SLOW, GetComponent<PlayerStatusEffectHandler>()))
            {
                DropItemInHand();

                InventorySlotInput();

                UseItemInHand();
            }
            
            //ReducerToPoisonTimer();
        }
    }

    private void PauseMenu()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            player.enabled = pauseGame;
            pauseGame = !pauseGame;
            pauseMenu.SetActive(pauseGame);
            
            if (pauseGame)
            {
                Cursor.lockState = CursorLockMode.None;
            }

            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
    
    private void UseItemInHand()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && selectedItem != Item.EMPTY)
        {
            if (selectedItem == Item.FLASHLIGHT)
            {
                _flashlight.lightOnOff = !_flashlight.lightOnOff;
                CmdFlashChangeState(_flashlight.lightOnOff);
            }
            
            if (selectedItem == Item.MEDKIT)
            {
                inventorySystem.DeleteItemToSlot(selectedItem);

                ResetItemInHand();
            }
            
            if (selectedItem == Item.SYRINGE)
            {
                StatusEffect.RemoveStatusEffect(StatusEffectType.POISON, GetComponent<PlayerStatusEffectHandler>());
            }
            
            if (selectedItem == Item.KEY)
            {
                inventorySystem.DeleteItemToSlot(selectedItem);

                ResetItemInHand();
            }
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

            ResetItemInHand();
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
            case Item.SYRINGE:
                selectedGameObject = 3;
                CmdChangeActiveWeapon(selectedGameObject);
                break;
            case Item.EMPTY:
                selectedGameObject = 4;
                CmdChangeActiveWeapon(selectedGameObject);
                break;
        }
    }

    public void ResetItemInHand()
    {
        selectedItem = Item.EMPTY;

        selectedGameObject = 4;
            
        CmdChangeActiveWeapon(selectedGameObject);
    }

    public void AddItemToInventory(Item item)
    {
        inventorySystem.AddItemToSlot(item);
    }

    private void InventorySlotInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedSlot = 0;
            inventorySystem.SelectedSlot(selectedSlot);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedSlot = 1;
            inventorySystem.SelectedSlot(selectedSlot);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedSlot = 2;
            inventorySystem.SelectedSlot(selectedSlot);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedSlot = 3;
            inventorySystem.SelectedSlot(selectedSlot);
        }
    }
    
    private void Raycast()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        
        if (Physics.SphereCast(ray, 0.25f ,out RaycastHit hit, 2))
        {
            if (hit.collider.gameObject.layer == 8)
            {
                pressE.SetActive(true);
                
                InteractWithObject(hit.collider.gameObject);
            }

            if (hit.collider.gameObject.layer == 7)
            {
                pressE.SetActive(true);
                
                _hideableObj = hit.collider.GetComponent<Hideable>();
                
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _hideableObj.CmdInteract();
                }
            }
        }

        else
        {
            pressE.SetActive(false);
        }
    }
    
    private void InteractWithObject(GameObject colliderGameObject)
    {
        if (colliderGameObject.TryGetComponent(out IInteractable interactableObj))
        {
            interactableObj.Interact(this, player);
        }
    }
}
