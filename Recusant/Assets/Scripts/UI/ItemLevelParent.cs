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
        EquipmentManager.instance.EldEvent += HandleEquipmentEvent;
        Inventory.instance.onItemChangedCallback += HandleInventoryEvent;
        itemDictionary = new Dictionary<Item, int>();
        Debug.Log("created itemdict");
    }

    private void HandleInventoryEvent()
    {
        Dictionary<string, int> localWeaponLevelCount = GameManager.instance.weaponLevelCount;
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
        Debug.Log(itemDictionary.Count + " dictionary size");
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
        
        Debug.Log(itemDictionary.Count + " dictionary size");
        
    }

    private void OnDestroy()
    {
        EquipmentManager.instance.EldEvent -= HandleEquipmentEvent;
        Inventory.instance.onItemChangedCallback -= HandleInventoryEvent;
    }

    public void InstantiateUIElements()
    {
        int childCount = transform.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            Destroy(child);
        }
        
        Dictionary<string, int> localWeaponLevelCount = GameManager.instance.weaponLevelCount;
        foreach (var i in Inventory.instance.items)
        {
            if (Inventory.instance.items == null)
            {
                Debug.Log("inventory is null");
            }

            if (itemDictionary == null)
            {
                Debug.Log("item dict is null");
            }

            if (i == null)
            {
                Debug.Log("i is null");
            }
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
        
        foreach (var j in itemDictionary)
        {
            GameObject instantiatedObject = Instantiate(itemLevelSlot, transform);
            ItemLevelSlot childScript = instantiatedObject.GetComponent<ItemLevelSlot>();
            childScript.FillImages(j.Key, j.Value);
        }
    }
    
}
