using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingComputerLogic : EquipmentLogic
{
    public override void ApplyLogic(Equipment equipment, int equipmentLevel)
    {
        switch (equipmentLevel)
        {
            case 1:
                equipment.damageModifier = 15;
                break;
            case 2:
                equipment.damageModifier = 30;
                break;
            case 3:
                equipment.damageModifier = 45;
                break;
            case 4:
                equipment.damageModifier = 60;
                break;
            case 5:
                equipment.damageModifier = 75;
                break;
            case 6:
                equipment.damageModifier = 90;
                break;
            case 7:
                equipment.damageModifier = 105;
                break;
            case 8:
                equipment.damageModifier = 120;
                break;
            case 9:
                equipment.damageModifier = 135;
                break;
            case 10:
                equipment.damageModifier = 150;
                break;
            default:
                Debug.Log("No logic applied to " + equipment.itemName);
                break;
        }

        Debug.Log("Applied logic level " + equipmentLevel);
    }
}
