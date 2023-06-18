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
    

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem, int lvl);
    public OnEquipmentChanged onEquipmentChanged;
    
    private Equipment[] currentEquipment;
    private Inventory _inventory;

    public GameObject equipmentParent;

    GameObject targetEquipmentSlot;
    GameObject targetEquipmentSlotButton;
    GameObject targetEquipmentSlotIcon;
    Image targetEquipmentSlotIconImage;

    private int lastSlotFilled;
    private int[] equipmentLevelsArray;

    private void Start()
    {
        _inventory = Inventory.instance;
        int numOfSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numOfSlots];
        lastSlotFilled = 0;
        equipmentLevelsArray = new int[numOfSlots];
        
    }

    public void Equip(Equipment newItem)
    {
        //int slotIndex = (int)newItem.equipmentSlot;
        int slotIndex = lastSlotFilled;

        for (int i = 0; i < currentEquipment.Length; i++)
        {
            if (currentEquipment[i] != null)
            {
                if (currentEquipment[i].itemName == newItem.itemName)
                {
                    slotIndex = i;
                }
            }
        }
        

        Equipment oldItem = null;

        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            //_inventory.Add(oldItem, false);

            currentEquipment[slotIndex] = newItem;
            
            UpdateEquipmentSlot(slotIndex, true);
        }



        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem, equipmentLevelsArray[slotIndex]);
            Debug.Log("Invoked onEquipmentChanged " + newItem.damageModifier);
        }
        
        currentEquipment[slotIndex] = newItem;
        
        UpdateEquipmentSlot(slotIndex, true);

        if (currentEquipment.Length != lastSlotFilled)
        {
            lastSlotFilled++;
            Debug.Log("LSF: " + lastSlotFilled);
        }

        equipmentLevelsArray[slotIndex]++;

    }

    public void Unequip(int slotIndex)
    {
        Debug.Log("Unequip called");
        if (currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            //_inventory.Add(oldItem, false);

            currentEquipment[slotIndex] = null;
            
            UpdateEquipmentSlot(slotIndex, false);
            
            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem, 0);
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
        
        //Setting the opacity to full when item equipped
        Color imageColor = targetEquipmentSlotIconImage.color;
        imageColor.a = 1.0f;
        targetEquipmentSlotIconImage.color = imageColor;

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
