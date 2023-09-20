using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshyLogic : EquipmentLogic
{
    public override void ApplyLogic(Equipment equipment, int equipmentLevel)
    {
        switch (equipmentLevel)
        {
            case 1:
                equipment.healthRegenModifier = 1;
                break;
            case 2:
                equipment.healthRegenModifier = 2;
                break;
            case 3:
                equipment.healthRegenModifier = 3;
                break;
            case 4:
                equipment.healthRegenModifier = 4;
                break;
            case 5:
                equipment.healthRegenModifier = 5;
                break;
            case 6:
                equipment.healthRegenModifier = 6;
                break;
            case 7:
                equipment.healthRegenModifier = 7;
                break;
            case 8:
                equipment.healthRegenModifier = 8;
                break;
            case 9:
                equipment.healthRegenModifier = 9;
                break;
            case 10:
                equipment.healthRegenModifier = 10;
                break;
            default:
                Debug.Log("No logic applied to " + equipment.itemName);
                break;
        }

        Debug.Log("Applied logic level " + equipmentLevel);
    }
}

