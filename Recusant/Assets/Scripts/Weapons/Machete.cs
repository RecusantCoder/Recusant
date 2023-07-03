using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machete : Weapon
{
    private int macheteDamage = 15;

    protected new int localWeaponlevel;
    protected new int numOfShots = 1;
    protected new float shotFrequency = 1.0f;
    protected new List<float> times = new List<float>();
    protected new List<float> firedList = new List<float>();
    
    
    public override void Shoot(Transform firePoint, int weaponLevel)
    {
        WeaponLevels(weaponLevel);
        
        for (int i = 0; i < numOfShots; i++)
        {
            if (times.Capacity < numOfShots)
            {
                for (int j = 0; j < 100; j++)
                {
                    times.Add(0);
                    firedList.Add(0);
                }
            }
            float shotFrequency3 = 1.5f + (0.1f * i);
            
            if (firedList[i] > 1)
            {
                shotFrequency3 = 1.5f;
            }

            if (Time.time - times[i] > shotFrequency3)
            {
                firedList[i]++;

                FireShot(firePoint, weaponLevel);
            
                times[i] = Time.time;
            }
        }
    }
    

    protected override void FireShot(Transform firePoint, int weaponLevel)
    {
        GameObject slash = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/Slash"), firePoint.position, firePoint.rotation);
        Rigidbody2D rb = slash.GetComponent<Rigidbody2D>();

        // Get the bullet script component and change its damage amount
        Slash slashScript = slash.GetComponent<Slash>();
        slashScript.damage += macheteDamage; // Change the damage amount
        slashScript.firePointLocal = firePoint;
        slashScript.horizontal = PlayerManager.instance.player.GetComponent<PlayerMovement>().joystick.Horizontal;
        AudioManager.instance.Play("Slash");
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
        switch (level)
        {
            case 1:
                print("Lvl 1 Machete");
                break;
            case 2:
                print("Lvl 2 Machete");
                macheteDamage += 10;
                numOfShots += 1;
                break;
            case 3:
                print("Lvl 3 Machete");
                macheteDamage += 10;
                numOfShots += 1;
                break;
            case 4:
                print("Lvl 4 Machete");
                macheteDamage += 10;
                numOfShots += 1;
                break;
            case 5:
                print("Lvl 5 Machete");
                macheteDamage += 10;
                numOfShots += 1;
                break;
            case 6:
                print("Lvl 6 Machete");
                macheteDamage += 10;
                numOfShots += 1;
                numOfShots++;
                break;
            case 7:
                print("Lvl 7 Machete");
                macheteDamage += 10;
                numOfShots += 1;
                break;
            case 8:
                print("Lvl 8 Machete");
                macheteDamage += 10;
                numOfShots += 1;
                numOfShots++;
                break;
            case 9:
                print("Lvl 9 Machete");
                macheteDamage += 10;
                numOfShots += 1;
                break;
            case 10:
                print("Lvl 10 Machete");
                macheteDamage += 10;
                numOfShots += 1;
                numOfShots++;
                break;
            default:
                print("Default Machete.");
                break;
        }
    }
}
