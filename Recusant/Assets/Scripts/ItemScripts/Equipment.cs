using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipmentSlot;

    public int armorModifier;
    public int damageModifier;
    public int healthRegenModifier;
    public int speedModifier;
    public int pickupRadiusModifier;

    public override void Use()
    {
        base.Use();
        RemoveFromInventory();
        EquipmentManager.instance.Equip(this);
        Debug.Log("Used " + this.itemName);
        
    }
}

public enum EquipmentSlot
{
    Head, Chest, Equipment, Weapon, Tool
}
