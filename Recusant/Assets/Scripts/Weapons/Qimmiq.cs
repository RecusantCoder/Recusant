using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qimmiq : Weapon
{
    private float lastShotTime;
    
    protected new int localWeaponlevel;
    protected new int numOfShots = 1;
    protected new float shotFrequency = 5.0f;
    protected new List<float> times = new List<float>();
    protected new List<float> firedList = new List<float>();

    public override void Shoot(Transform firePoint, int weaponLevel)
    {
        
        if (Time.time - lastShotTime > shotFrequency)
        {
            UpdateWeaponBehavior(weaponLevel);
            GameObject qimmiqAttack = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/QimmiqAttack"));
            qimmiqAttack.transform.position = firePoint.position;
            lastShotTime = Time.time;
            AudioManager.instance.Play("SingleBark");
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
                print ("Lvl 1 Qimmiq");
                break;
            case 2:
                print ("Lvl 2 Qimmiq");
                break;
            case 3:
                print ("Lvl 3 Qimmiq");
                break;
            case 4:
                print ("Lvl 4 Qimmiq");
                break;
            case 5:
                print ("Lvl 5 Qimmiq");
                break;
            case 6:
                print ("Lvl 6 Qimmiq");
                break;
            case 7:
                print ("Lvl 7 Qimmiq");
                break;
            case 8:
                print ("Lvl 8 Qimmiq");
                break;
            case 9:
                print ("Lvl 9 Qimmiq");
                break;
            case 10:
                print ("Lvl 10 Qimmiq");
                break;
            default:
                print ("Default Qimmiq.");
                break;
        }
    }
}
