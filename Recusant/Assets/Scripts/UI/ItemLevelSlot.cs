using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLevelSlot : MonoBehaviour
{
    public Image itemImage;
    public Image itemLevelImage;

    public void FillImages(Item item, int level)
    {
        itemImage.sprite = item.icon;
        itemLevelImage.sprite = Resources.Load<Sprite>("Sprites/tally5");
        Debug.Log(item.itemName + " level image should be " + level);
    }
}
