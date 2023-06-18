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
        bool alreadyInInventory = false;
        foreach (var pickup in items)
        {
            if (pickup.itemName == item.itemName)
            {
                alreadyInInventory = true;
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
            } else
            {
                items.Add(item);

                try
                {
                    if (!(item is Equipment))
                    {
                        GameManager.instance.weaponLevelCount.Add(item.itemName, 0);
                    }
                
                    if (item is Equipment && pickedUp)
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
