using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLevelParent : MonoBehaviour
{
    public GameObject itemLevelSlot;
    private Dictionary<Item, int> itemDictionary;

    private void Awake()
    {
        itemDictionary = new Dictionary<Item, int>();
    }

    private void Start()
    {
        EquipmentManager.instance.EldEvent += HandleEquipmentEvent;
        Inventory.instance.onItemChangedCallback += HandleInventoryEvent;
    }

    private void HandleInventoryEvent()
    {
        Dictionary<string, int> localWeaponLevelCount = GameManager.instance.weaponLevelCount;
        
        // Create a list to store items to remove from the itemDictionary
        List<Item> itemsToRemove = new List<Item>();
        
        // Check each item in the itemDictionary
        foreach (var item in itemDictionary.Keys)
        {
            if (!Inventory.instance.items.Contains(item))
            {
                // If the item is not in the inventory, add it to the removal list
                itemsToRemove.Add(item);
            }
        }

        // Remove items from the itemDictionary that are no longer in the inventory
        foreach (var itemToRemove in itemsToRemove)
        {
            itemDictionary.Remove(itemToRemove);
        }
        
        // Now, update the itemDictionary for items still in the inventory
        foreach (var i in Inventory.instance.items)
        {
            if (itemDictionary.ContainsKey(i))
            {
                if (localWeaponLevelCount.ContainsKey(i.itemName))
                {
                    itemDictionary[i] = localWeaponLevelCount[i.itemName];
                }
            }
            else
            {
                itemDictionary.Add(i, localWeaponLevelCount[i.itemName]);
            }
        }
    }

    private void HandleEquipmentEvent(Item equipment, int itemLevelLocal)
    {
        if (itemDictionary.ContainsKey(equipment))
        {
            itemDictionary[equipment] = itemLevelLocal;
        }
        else
        {
            itemDictionary.Add(equipment, itemLevelLocal);
        }
    }

    private void OnDestroy()
    {
        EquipmentManager.instance.EldEvent -= HandleEquipmentEvent;
        Inventory.instance.onItemChangedCallback -= HandleInventoryEvent;
    }

    public void InstantiateUIElements()
    {
        Debug.Log("items in inventory, according to InstatiateUIElements: ");
        int childCount = transform.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            Destroy(child);
        }
        
        Dictionary<string, int> localWeaponLevelCount = GameManager.instance.weaponLevelCount;
        foreach (var i in Inventory.instance.items)
        {
            Debug.Log(i.itemName + " in inventory");
            if (itemDictionary.ContainsKey(i))
            {
                if (localWeaponLevelCount.ContainsKey(i.itemName))
                {
                    itemDictionary[i] = localWeaponLevelCount[i.itemName];
                }
            }
            else
            {
                itemDictionary.Add(i, localWeaponLevelCount[i.itemName]);
            }
        }

        int count = 0;
        
        foreach (var e in EquipmentManager.instance.GetCurrentEquipment)
        {
            if (EquipmentManager.instance.GetCurrentEquipment[count] != null)
            {
                if (itemDictionary.ContainsKey(e))
                {
                    if (EquipmentManager.instance.GetEquipmentLevelsArray()[count] == itemDictionary[e])
                    {
                        itemDictionary[e] = EquipmentManager.instance.GetEquipmentLevelsArray()[count];
                    }
                }
                else
                {
                    itemDictionary.Add(e,
                        itemDictionary[e] = EquipmentManager.instance.GetEquipmentLevelsArray()[count]);
                }

                count++;
            }
        }
        
        foreach (var j in itemDictionary)
        {
            GameObject instantiatedObject = Instantiate(itemLevelSlot, transform);
            ItemLevelSlot childScript = instantiatedObject.GetComponent<ItemLevelSlot>();
            childScript.FillImages(j.Key, j.Value);
        }
    }
    
}
