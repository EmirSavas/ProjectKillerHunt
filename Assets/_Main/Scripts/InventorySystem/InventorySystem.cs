using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public CharacterMechanic characterMechanic;
   
    public ItemSlot[] slotGameObject;
    
    public Sprite slotUI;

    public Sprite selectedSlotUI;
   
    public Sprite[] itemSprite; //0 = Flashlight // 1 = Key //  2 = Medkit // 3 = Siringa // 4 = Null
    

    public void SelectedSlot(int index)
    {
        characterMechanic.selectedItem = slotGameObject[index].item;

        characterMechanic.SelecetedItem();

        slotGameObject[index].GetComponent<Image>().sprite = selectedSlotUI;
            
        for (int i = 0; i < slotGameObject.Length; i++)
        {
            if (i != index)
            {
                slotGameObject[i].GetComponent<Image>().sprite = slotUI;
            }
        }
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
