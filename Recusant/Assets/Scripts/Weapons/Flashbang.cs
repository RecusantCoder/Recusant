using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashbang : Weapon
{
    public int damage = 0;
    public int localWeaponLevel;
    public float throwSpeed = 10f;
    public int numOfThrows = 1;
    private int penetrations = 0;
    private int flashRadius = 5;
    
    
    protected new List<float> times = new List<float>();
    protected new List<float> firedList = new List<float>();

    public override void Shoot(Transform firePoint, int weaponLevel)
    {
        WeaponLevels(weaponLevel);
        
        for (int i = 0; i < numOfThrows; i++)
        {
            if (times.Capacity < numOfThrows)
            {
                for (int j = 0; j < 100; j++)
                {
                    times.Add(0);
                    firedList.Add(0);
                }
            }
            float shotFrequency3 = 3.5f + (0.1f * i);
            
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
        Debug.Log("Throwing flashbang");
        Quaternion rotationA = Quaternion.Euler(0f, 0f, 0f);
        GameObject thrownGrenade = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/ThrownFlashbang"), firePoint.position, firePoint.rotation * rotationA);
        Rigidbody2D rb = thrownGrenade.GetComponent<Rigidbody2D>();
        rb.AddForce(thrownGrenade.transform.up * throwSpeed, ForceMode2D.Impulse);
        
        AudioManager.instance.Play("Throw");
        
        // Apply torque for spinning motion
        float torqueForce = 10f;
        rb.AddTorque(torqueForce, ForceMode2D.Impulse);
        
        //Modify the values on the thrownGrenade
        ThrownFlashbang thrownGrenadeScript = thrownGrenade.GetComponent<ThrownFlashbang>();
        
    }

    protected override void WeaponLevels(int weaponLevel)
    {
        if (localWeaponLevel != weaponLevel && weaponLevel <= 10)
        {
            int oldLevel = localWeaponLevel;
            localWeaponLevel = weaponLevel;

            for (int level = oldLevel + 1; level <= localWeaponLevel; level++)
            {
                UpdateWeaponBehavior(level);
            }
        }
    }

    protected override void UpdateWeaponBehavior(int level)
    {
        switch (level)
        {
            case 2:
                print("Lvl 2 flash");
                flashRadius += 10;
                break;
            case 3:
                print("Lvl 3 flash");
                flashRadius += 10;
                break;
            case 4:
                print("Lvl 4 flash");
                flashRadius += 10;
                break;
            case 5:
                print("Lvl 5 flash");
                flashRadius += 10;
                break;
            case 6:
                print("Lvl 6 flash");
                flashRadius += 10;
                break;
            case 7:
                print("Lvl 7 flash");
                flashRadius += 10;
                break;
            case 8:
                print("Lvl 8 flash");
                flashRadius += 10;
                break;
            case 9:
                print("Lvl 9 flash");
                flashRadius += 10;
                break;
            case 10:
                print("Lvl 10 flash");
                flashRadius += 10;
                ;
                break;
            default:
                print("Default flash.");
                break;
        }
    }
}
