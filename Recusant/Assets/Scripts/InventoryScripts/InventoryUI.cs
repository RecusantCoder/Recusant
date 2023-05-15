using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;
    private InventorySlot[] slots;
    
    private Inventory _inventory;
    // Start is called before the first frame update
    void Start()
    {
        _inventory = Inventory.instance;
        _inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUI()
    {
        Debug.Log(slots.Length + " slots count");
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < _inventory.items.Count)      
            {
                slots[i].AddItem(_inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    public void ShowInventory()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    private void OnDestroy()
    {
        _inventory.onItemChangedCallback -= UpdateUI;
    }
}
