 using System;
 using System.Collections;
using System.Collections.Generic;
 using System.Runtime.CompilerServices;
 using UnityEngine;
using UnityEngine.UI;
 using Random = UnityEngine.Random;

 public class InventorySlot : MonoBehaviour
{
    private Item item;
    public Image icon;
    public Button removeButton;
    public GameObject player;
    
    
    
    private void Awake()
    {
        FindIconImageSprite();
    }

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
        
        Debug.Log("Picked up " + item.itemName);

    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        
        Vector2 imHere = player.transform.position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);;
        GameObject instance = Instantiate(Resources.Load("Prefabs/Pickups/" + item.itemName, typeof(GameObject)), imHere, Quaternion.identity) as GameObject;
        
        Debug.Log(item.itemName + " dropped.");

        Inventory.instance.Remove(item);
        
    }

    public void UseItem()
    {
        Debug.Log("Pressed!");
        if (item != null)
        {
            item.Use();
        }
    }

    private void FindIconImageSprite()
    {
        Transform itemButtonChild = transform.Find("ItemButton");
        Transform iconGrandChild = itemButtonChild.Find("Icon");
        Image iconImage = iconGrandChild.GetComponent<Image>();
        icon = iconImage;
    }
    
}
