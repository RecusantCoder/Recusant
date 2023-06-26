using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLevelSlot : MonoBehaviour
{
    public Image itemImage;
    public Image itemLevelImage1;
    public Image itemLevelImage2;

    public void FillImages(Item item, int level)
    {
        itemImage.sprite = item.icon;
        itemLevelImage1.sprite = Resources.Load<Sprite>("Sprites/tally5");
        
        
        if (level == 0)
                {
                    itemLevelImage1.sprite = Resources.Load<Sprite>("Sprites/confusedItem");
                    Color c = itemLevelImage2.color;
                    c.a = 0.0f;
                    itemLevelImage2.color = c;
                }
                
                if (level >= 1 && level <= 5)
                {
                    string tallySpritePath = "Sprites/tally" + level;
                    itemLevelImage1.sprite = Resources.Load<Sprite>(tallySpritePath);
                    Color c = itemLevelImage2.color;
                    c.a = 0.0f;
                    itemLevelImage2.color = c;
                }
                
                if (level >= 6 && level <= 10)
                {
                    itemLevelImage1.sprite = Resources.Load<Sprite>("Sprites/tally5");
                        
                    Color c = itemLevelImage2.color;
                    c.a = 1.0f;
                    itemLevelImage2.color = c;
                    int weaponLevelMinus5 = level - 5;
                    string tallySpritePath = "Sprites/tally" + weaponLevelMinus5;
                    itemLevelImage2.sprite = Resources.Load<Sprite>(tallySpritePath);
                }
        
        
        
        Debug.Log(item.itemName + " level image should be " + level);
    }
}
