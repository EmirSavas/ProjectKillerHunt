using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image slotImage;

    public GameObject slotGameObject;

    public bool isSlotFull;
    
    public Item item; 

    public void ChangeSprite(Sprite newSprite)
    {
        slotImage.sprite = newSprite;
    }

    public void ChangeValues(Item _item)
    {
        slotGameObject.SetActive(true);
        isSlotFull = true;
        item = _item;
    }

    public void DeleteItemFromSlot()
    {
        slotImage.sprite = null;
        isSlotFull = false;
        item = Item.EMPTY; 
        slotGameObject.SetActive(false);
    }
}
