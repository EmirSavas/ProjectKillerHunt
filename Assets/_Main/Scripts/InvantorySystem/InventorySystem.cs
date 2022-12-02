using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public CharacterMechanic characterMechanic;
   
    public ItemSlot[] slotGameObject;
   
    public Sprite[] itemSprite; //0 = Flashlight // 1 = Key //  2 = Medkit // 3 = Siringa

    private void Start()
    {
        AddItemToSlot(0);
    }

    public void SelectedSlot(int index)
    {
        characterMechanic.selectedItem = slotGameObject[index].item;

        characterMechanic.SelecetedItem();
    }

    public void AddItemToSlot(Item item)
    {
        foreach (var slot in slotGameObject)
        {
            if (slot.isSlotFull == false)
            {
                slot.ChangeSprite(itemSprite[(int)item]);
                slot.ChangeValues(item);
                return;
            }
        }
    }

    public void DeleteItemToSlot(Item item)
    {
        foreach (var slot in slotGameObject)
        {
            if (slot.isSlotFull == true && slot.item == item)
            {
                slot.DeleteItemFromSlot();
                return;
            }
        }
    }
    
    
}