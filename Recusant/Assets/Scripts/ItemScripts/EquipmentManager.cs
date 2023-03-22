using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    
    public static EquipmentManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of EquipmentManager found!");
            return;
        }
        instance = this;
    }
    
    #endregion

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);

    public OnEquipmentChanged onEquipmentChanged;
    
    private Equipment[] currentEquipment;
    private Inventory _inventory;

    public GameObject equipmentParent;

    GameObject targetEquipmentSlot;
    GameObject targetEquipmentSlotButton;
    GameObject targetEquipmentSlotIcon;
    Image targetEquipmentSlotIconImage;

    private void Start()
    {
        _inventory = Inventory.instance;
        int numOfSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numOfSlots];
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipmentSlot;

        Equipment oldItem = null;
        
        
        

        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            _inventory.Add(oldItem);
            currentEquipment[slotIndex] = newItem;
            
            UpdateEquipmentSlot(slotIndex, true);
        }

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
        
        currentEquipment[slotIndex] = newItem;
        
        UpdateEquipmentSlot(slotIndex, true);
        
        
        
    }

    public void Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            _inventory.Add(oldItem);

            currentEquipment[slotIndex] = null;
            
            UpdateEquipmentSlot(slotIndex, false);
            
            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
        }
    }
    
    void UpdateEquipmentSlot(int slotIndex, bool equip)
    {
        targetEquipmentSlot = equipmentParent.transform.GetChild(slotIndex).gameObject;
        targetEquipmentSlotButton = targetEquipmentSlot.transform.GetChild(0).gameObject;
        targetEquipmentSlotIcon = targetEquipmentSlotButton.transform.GetChild(0).gameObject;
        targetEquipmentSlotIconImage = targetEquipmentSlotIcon.GetComponent<Image>();
        targetEquipmentSlotIconImage.enabled = equip;
        
        if (equip)
        {
            targetEquipmentSlotIconImage.sprite = currentEquipment[slotIndex].icon;
        }
        else
        {
            targetEquipmentSlotIconImage.sprite = null;
        }
        
      
    }
}
