using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting_Computer : Weapon
{
    private int localWeaponlevel;
    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = GameManager.instance.player.GetComponent<PlayerStats>();
    }
    
    public override void Shoot(Transform firePoint, int weaponLevel)
    {
        WeaponLevels(weaponLevel);
    }
    
    protected override void WeaponLevels(int weaponLevel)
    {
        if (localWeaponlevel != weaponLevel && weaponLevel <= 10)
        {
            int oldLevel = localWeaponlevel;
            localWeaponlevel = weaponLevel;

            for (int level = oldLevel + 1; level <= localWeaponlevel; level++)
            {
                UpdateWeaponBehavior(level);
            }
        }
    }


    protected override void UpdateWeaponBehavior(int level)
    {
        Debug.Log("Targeting_Computer level: " + level);
        switch (level)
        {
            case 1:
                playerStats.damage.AddModifier(1);
                break;
            case 2:
                playerStats.damage.AddModifier(1);
                break;
            case 3:
                playerStats.damage.AddModifier(1);
                break;
            case 4:
                playerStats.damage.AddModifier(1);
                break;
            case 5:
                playerStats.damage.AddModifier(1);
                break;
            case 6:
                playerStats.damage.AddModifier(1);
                break;
            case 7:
                playerStats.damage.AddModifier(1);
                break;
            case 8:
                playerStats.damage.AddModifier(1);
                break;
            case 9:
                playerStats.damage.AddModifier(1);
                break;
            case 10:
                playerStats.damage.AddModifier(1);
                break;
            default:
                Debug.Log("default targeting computer");
                break;
        }
    }
}
