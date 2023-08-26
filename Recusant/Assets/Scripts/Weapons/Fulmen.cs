using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fulmen : Weapon
{
    private float lastShotTime;
    
    protected new int localWeaponlevel;
    protected new int numOfShots = 1;
    protected new float shotFrequency = 30.0f;
    protected new List<float> times = new List<float>();
    protected new List<float> firedList = new List<float>();

    public override void Shoot(Transform firePoint, int weaponLevel)
    {
        
        if (Time.time - lastShotTime > shotFrequency)
        {
            UpdateWeaponBehavior(weaponLevel);
            GameObject lightning = Instantiate(Resources.Load<GameObject>("Prefabs/Lightning"), firePoint.position, firePoint.rotation);
            lastShotTime = Time.time;
            AudioManager.instance.Play("lightning");
        }
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

    void UpdateWeaponBehavior(int weaponLevel)
    {
        switch (weaponLevel)
        {
            case 1:
                print ("Lvl 1 Fulmen");
                break;
            case 2:
                print ("Lvl 2 Fulmen");
                break;
            case 3:
                print ("Lvl 3 Fulmen");
                break;
            case 4:
                print ("Lvl 4 Fulmen");
                break;
            case 5:
                print ("Lvl 5 Fulmen");
                break;
            case 6:
                print ("Lvl 6 Fulmen");
                break;
            case 7:
                print ("Lvl 7 Fulmen");
                break;
            case 8:
                print ("Lvl 8 Fulmen");
                break;
            case 9:
                print ("Lvl 9 Fulmen");
                break;
            case 10:
                print ("Lvl 10 Fulmen");
                break;
            default:
                print ("Default Fulmen.");
                break;
        }
    }
}
