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
                equipment.armorModifier = 1;
                break;
            case 2:
                equipment.armorModifier = 2;
                break;
            case 3:
                equipment.armorModifier = 3;
                break;
            case 4:
                equipment.armorModifier = 4;
                break;
            case 5:
                equipment.armorModifier = 5;
                break;
            case 6:
                equipment.armorModifier = 6;
                break;
            case 7:
                equipment.armorModifier = 7;
                break;
            case 8:
                equipment.armorModifier = 8;
                break;
            case 9:
                equipment.armorModifier = 9;
                break;
            case 10:
                equipment.armorModifier = 10;
                break;
            default:
                Debug.Log("No logic applied to " + equipment.itemName);
                break;
        }

        Debug.Log("Applied logic level " + equipmentLevel);
    }
}
