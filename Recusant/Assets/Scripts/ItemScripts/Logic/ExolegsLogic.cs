using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExolegsLogic : EquipmentLogic
{
    public override void ApplyLogic(Equipment equipment, int equipmentLevel)
    {
        switch (equipmentLevel)
        {
            case 1:
                equipment.speedModifier = 1;
                break;
            case 2:
                equipment.speedModifier = 2;
                break;
            case 3:
                equipment.speedModifier = 3;
                break;
            default:
                Debug.Log("No logic applied to " + equipment.itemName);
                break;
        }

        Debug.Log("Applied logic level " + equipmentLevel);
    }
}
