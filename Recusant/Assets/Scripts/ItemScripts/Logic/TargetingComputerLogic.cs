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
            default:
                Debug.Log("No logic applied to " + equipment.itemName);
                break;
        }

        Debug.Log("Applied logic level " + equipmentLevel);
    }
}
