using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : Weapon
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    private float lastShotTime1;
    private float shotFrequency1 = 1.0f;
    private int numOfShots = 1;
    private int bulletDamage = 10;
    private int penetrations = 0;
    private int localWeaponlevel = 0;
    
    private float lastShotTime2;
    private float shotFrequency2 = 1.1f;
    private int fired2 = 0;

    private List<float> times = new List<float>();
    private List<float> firedList = new List<float>();

    public override void Shoot(Transform firePoint, int weaponLevel)
    {
        /*if (Time.time - lastShotTime1 > shotFrequency1)
        {
            Debug.Log("fired1");
            
            FireShot(firePoint, weaponLevel);
            
            lastShotTime1 = Time.time;
        }
        
        if (Time.time - lastShotTime2 > shotFrequency2)
        {
            //This logic makes the shot delay by 0.1 seconds
            fired2++;
            if (fired2 == 2)
            {
                shotFrequency2 = 1.0f;
            }
            
            Debug.Log("fired2");
            
            FireShot(firePoint, weaponLevel);
            
            lastShotTime2 = Time.time;
        }*/

        for (int i = 0; i < numOfShots; i++)
        {
            if (times.Capacity < numOfShots)
            {
                Debug.Log("cap is 0");
                for (int j = 0; j < 100; j++)
                {
                    times.Add(0);
                    firedList.Add(0);
                }
            }
            float shotFrequency3 = 1.0f + (0.1f * i);
            
            if (firedList[i] > 2)
            {
                shotFrequency3 = 1.0f;
            }

            if (Time.time - times[i] > shotFrequency3)
            {
                firedList[i]++;

                Debug.Log("firing " + i);
            
                FireShot(firePoint, weaponLevel);
            
                times[i] = Time.time;
            }
        }
    }

    void FireShot(Transform firePoint, int weaponLevel)
    {
        GlockLevels(weaponLevel);
        
        GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet2"), firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletSpeed, ForceMode2D.Impulse);
        AudioManager.instance.Play("pistolGunshot");

        // Get the bullet script component and change its damage amount
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.bulletDamage += bulletDamage; // Change the damage amount
        bulletScript.penetrations += penetrations;
    }

    void GlockLevels(int weaponLevel)
    {
        if (localWeaponlevel != weaponLevel)
        {
            localWeaponlevel = weaponLevel;

            switch (weaponLevel)
            {
                case 1:
                    print("Lvl 1 glock");
                    break;
                case 2:
                    print("Lvl 2 glock");
                    numOfShots++;
                    break;
                case 3:
                    print("Lvl 3 glock");
                    numOfShots++;
                    bulletDamage += 5;
                    break;
                case 4:
                    print("Lvl 4 glock");
                    numOfShots++;
                    break;
                case 5:
                    print("Lvl 5 glock");
                    penetrations++;
                    break;
                case 6:
                    print("Lvl 6 glock");
                    numOfShots++;
                    break;
                case 7:
                    print("Lvl 7 glock");
                    numOfShots++;
                    bulletDamage += 5;
                    break;
                case 8:
                    print("Lvl 8 glock");
                    numOfShots++;
                    break;
                case 9:
                    print("Lvl 9 glock");
                    numOfShots++;
                    bulletDamage += 5;
                    penetrations++;
                    break;
                case 10:
                    print("Lvl 10 glock");
                    numOfShots++;
                    bulletDamage += 50;
                    penetrations++;
                    break;
                default:
                    print("Default glock.");
                    break;
            }
        }
    }
}

