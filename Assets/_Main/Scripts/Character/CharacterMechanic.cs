using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
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

    public GameObject[] item; //0 = Flashlight // 1 = Key //  2 = Medkit // 3 = Syringe
    public int selectedGameObject;


    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        
        Raycast();

        DropItemInHand();

        InventorySlotInput();
    }

    public void DropItemInHand()
    {
        if (Input.GetKeyDown(KeyCode.G) && selectedItem != Item.EMPTY && selectedItem != Item.FLASHLIGHT)
        {
            inventorySystem.DeleteItemToSlot(selectedItem);

            item[selectedGameObject].SetActive(false);
        }
    }

    public void SelecetedItem()
    {
        switch (selectedItem)
        {
            case Item.FLASHLIGHT:
                EquipmentChanger(0);
                selectedGameObject = 0;
                break;
            case Item.KEY:
                EquipmentChanger(1);
                selectedGameObject = 1;
                break;
            case Item.MEDKIT:
                EquipmentChanger(2);
                selectedGameObject = 2;
                break;
            case Item.Syringe:
                EquipmentChanger(3);
                selectedGameObject = 3;
                break;
            default:
                EquipmentChanger(0);
                selectedGameObject = 0;
                break;
        }
    }

    
    public void EquipmentChanger(int _item)
    {
        item[_item].SetActive(true); 
        CmdEquipmentChanger(_item, true);

        for (int i = 0; i < item.Length; i++)
        {
            if (i != _item)
            {
                item[i].SetActive(false); 
                CmdEquipmentChanger(i, false);
            }
        }
    }

    [Command]
    public void CmdEquipmentChanger(int _item, bool _bool)
    {
        RpcEquipmentChanger(_item, _bool);
    }
    
    [ClientRpc]
    public void RpcEquipmentChanger(int _item, bool _bool)
    {
        item[_item].SetActive(_bool);
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
