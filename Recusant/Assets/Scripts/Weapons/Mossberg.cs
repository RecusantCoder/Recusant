using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mossberg : Weapon
{
    public GameObject bulletPrefab;
    public int pellets = 5;
    public float bulletSpeed = 20f;
    private float pelletSpread = 20f;
    private int numOfShots = 1;
    private int bulletDamage = 10;
    private int penetrations = 0;
    
    private List<float> times = new List<float>();
    private List<float> firedList = new List<float>();

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
            float shotFrequency3 = 1.0f + (0.1f * i);
            
            if (firedList[i] > 1)
            {
                shotFrequency3 = 1.0f;
            }

            if (Time.time - times[i] > shotFrequency3)
            {
                firedList[i]++;

                Debug.Log("firing mossberg " + i);
            
                FireShot(firePoint, weaponLevel);
            
                times[i] = Time.time;
            }
        }
    }

    void FireShot(Transform firePoint, int weaponLevel)
    {
        //WeaponLevels(weaponLevel);

        for (int i = 0; i < pellets; i++)
        {
            Quaternion rotationA = Quaternion.Euler(0f, 0f, Random.Range(-pelletSpread, pelletSpread));
            GameObject pellet = Instantiate(Resources.Load<GameObject>("Prefabs/Pellet"), firePoint.position, firePoint.rotation * rotationA);
            Rigidbody2D rb = pellet.GetComponent<Rigidbody2D>();
            rb.AddForce(pellet.transform.up * bulletSpeed, ForceMode2D.Impulse);
           
            // Get the bullet script component and change its damage amount
            Bullet bulletScript = pellet.GetComponent<Bullet>();
            bulletScript.bulletDamage += bulletDamage; // Change the damage amount
            bulletScript.penetrations += penetrations;

        };
        AudioManager.instance.Play("shotgunGunshot");
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

    private void UpdateWeaponBehavior(int level)
    {
        switch (level)
        {
            case 2:
                print("Lvl 2 mossberg");
                pellets++;
                break;
            case 3:
                print("Lvl 3 mossberg");
                pellets++;
                bulletDamage += 5;
                break;
            case 4:
                print("Lvl 4 mossberg");
                pellets++;
                break;
            case 5:
                print("Lvl 5 mossberg");
                pellets++;
                penetrations++;
                bulletDamage += 5;
                break;
            case 6:
                print("Lvl 6 mossberg");
                pellets++;
                numOfShots++;
                break;
            case 7:
                print("Lvl 7 mossberg");
                pellets++;
                bulletDamage += 5;
                break;
            case 8:
                print("Lvl 8 mossberg");
                pellets++;
                numOfShots++;
                break;
            case 9:
                print("Lvl 9 mossberg");
                pellets++;
                bulletDamage += 5;
                break;
            case 10:
                print("Lvl 10 mossberg");
                pellets++;
                penetrations++;
                numOfShots++;
                break;
            default:
                print("Default mossberg.");
                break;
        }
    }
}
