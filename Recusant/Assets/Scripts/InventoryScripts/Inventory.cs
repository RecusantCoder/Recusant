using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }
    
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    
    public List<Item> items = new List<Item>();
    public int space = 20;
    
    //sending item removal
    public event Action<Item> OnItemRemoved;

    public bool Add(Item item, bool pickedUp)
    {
        Debug.Log("in inventory add method with: " + item.itemName);
        bool alreadyInInventory = false;
        foreach (var pickup in items)
        {
            if (pickup.itemName == item.itemName)
            {
                alreadyInInventory = true;
                Debug.Log(pickup + " already in inventory");
            }
        }

        if (!(item is Equipment) && alreadyInInventory)
        {
            //Level up the item instead
            GameManager.instance.LevelSelectedWeapon(item.itemName);
        }

        if (!item.isDefaultItem && !alreadyInInventory)
        {
            if (items.Count >= space)
            {
                Debug.Log("Inventory is full.");
                return false;
            } 
            else
            {
                items.Add(item);

                try 
                {
                    if (!GameManager.instance.weaponLevelCount.ContainsKey(item.itemName))
                    {
                        if (pickedUp)
                        {
                            GameManager.instance.weaponLevelCount.Add(item.itemName, 1);
                            Debug.Log("Added " + item.itemName + " at level 1");
                        }
                    }
                    else
                    {
                        if (pickedUp)
                        {
                            GameManager.instance.LevelSelectedWeapon(item.name);
                            Debug.Log("Levelled " + item.itemName);
                        }
                    }

                    if (item is Equipment)
                    {
                        item.Use();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                if (onItemChangedCallback != null)
                    onItemChangedCallback.Invoke();
            }
            
        }

        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        
        OnItemRemoved?.Invoke(item);
        
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
