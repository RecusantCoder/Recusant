using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<InventoryItem> inventory; // List of items in the inventory
    public GameObject inventoryUI; // User interface element to display the inventory
    public GameObject inventoryItemPrefab; // Prefab for the inventory item
    
    public GameObject inventorySlot;
    public Transform inventoryContent;
    public GameObject panel;

    private void Start()
    {
        inventory = new List<InventoryItem>(); // Initialize the inventory
        panel = GameObject.Find("Panel");
        panel.gameObject.SetActive(false);
    }

    public void AddItem(InventoryItem item)
    {
        // Check if the item is already in the inventory
        InventoryItem existingItem = inventory.Find(i => i.itemName == item.itemName);
        if (existingItem != null)
        {
            existingItem.quantity += item.quantity; // Add to the existing item's quantity
        }
        else
        {
            inventory.Add(item); // Add the new item to the inventory
        }
        UpdateUI(); // Update the inventory user interface
    }

    public void RemoveItem(InventoryItem item)
    {
        inventory.Remove(item); // Remove the item from the inventory
        UpdateUI(); // Update the inventory user interface
    }

    private void UpdateUI()
    {
        // Clear the inventory UI
        /*foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }*/

        // Add inventory items to the UI
        foreach (InventoryItem item in inventory)
        {
            GameObject itemObject = Instantiate(inventoryItemPrefab, inventorySlot.transform);
            itemObject.transform.SetParent(inventoryContent, false);
            //Image icon = itemObject.transform.Find("Icon").GetComponent<Image>();
            //icon.sprite = item.sprite;
        }
    }

    public void ShowInventory()
    {
        if (panel.gameObject.activeSelf)
        {
            panel.gameObject.SetActive(false);
        }
        else
        {
            panel.gameObject.SetActive(true);
            
        }
        
    }

    public void AddTurbo()
    {
        InventoryItem newItem = new InventoryItem();
        newItem.itemName = "Turbo";
        newItem.description = "An artifact with weight reducing properties.";
        //newItem.sprite = turbo;
        newItem.quantity = 1;

        AddItem(newItem);
    }
}

