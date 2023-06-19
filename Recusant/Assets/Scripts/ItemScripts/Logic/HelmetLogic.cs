using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetLogic : EquipmentLogic
{
    public override void ApplyLogic(Equipment equipment, int equipmentLevel)
    {
        switch (equipmentLevel)
        {
            case 1:
                equipment.armorModifier = 5;
                break;
            case 2:
                equipment.armorModifier = 10;
                break;
            case 3:
                equipment.armorModifier = 15;
                break;
            case 4:
                equipment.armorModifier = 20;
                break;
            case 5:
                equipment.armorModifier = 25;
                break;
            case 6:
                equipment.armorModifier = 30;
                break;
            case 7:
                equipment.armorModifier = 35;
                break;
            case 8:
                equipment.armorModifier = 40;
                break;
            case 9:
                equipment.armorModifier = 45;
                break;
            case 10:
                equipment.armorModifier = 50;
                break;
            default:
                Debug.Log("No logic applied to " + equipment.itemName);
                break;
        }

        Debug.Log("Applied logic level " + equipmentLevel);
    }
}
