using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : Weapon
{
    public float bulletSpeed = 20f;
    private int bulletDamage = 10;
    private int penetrations = 0;

    private int localWeaponlevel;
    private int numOfShots = 1;

    public float groupDelay = 1.2f;
    public float shotDelay = 0.1f;
    private bool isFiring = false;
    private Coroutine firingCoroutine;
    private int weaponLevelPassed;
    private Transform firePointPassed;

    public void Shoot(Transform firePoint, int weaponLevel, bool enemyNear)
    {
        firePointPassed = firePoint;
        weaponLevelPassed = weaponLevel;
        if (enemyNear)
        {
            StartFiring();
        }
        if (!enemyNear)
        {
            //Debug.Log("No enemy in range. " + isFiring + " <-is Firing");
        }
    }
    
    public void StartFiring()
    {
        if (!isFiring)
        {
            firingCoroutine = StartCoroutine(FireGroups());
        }
    }

    private IEnumerator FireGroups()
    {
        isFiring = true;
        for (int i = 0; i < numOfShots; i++)
        { 
            WeaponLevels(weaponLevelPassed);
            FireShot(firePointPassed, weaponLevelPassed);
            yield return new WaitForSeconds(shotDelay);
        }
        yield return new WaitForSeconds(groupDelay);
        isFiring = false;
    }

    protected override void FireShot(Transform firePoint, int weaponLevel)
    {
        GameObject bullet = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/Bullet2"), firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        
        //rb.AddForce(firePoint.up * bulletSpeed, ForceMode2D.Impulse);
        rb.velocity = firePoint.right * bulletSpeed;
        
        AudioManager.instance.Play("pistolGunshot");

        // Get the bullet script component and change its damage amount
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.bulletDamage += bulletDamage; // Change the damage amount
        bulletScript.penetrations += penetrations;
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
                print("Lvl 1 glock");
                break;
            case 2:
                print("Lvl 2 glock");
                numOfShots++;
                break;
            case 3:
                print("Lvl 3 glock");
                groupDelay -= 0.2f;
                break;
            case 4:
                print("Lvl 4 glock");
                numOfShots++;
                break;
            case 5:
                print("Lvl 5 glock");
                bulletDamage += 10;
                break;
            case 6:
                print("Lvl 6 glock");
                numOfShots++;
                break;
            case 7:
                print("Lvl 7 glock");
                penetrations++;
                break;
            case 8:
                print("Lvl 8 glock");
                bulletDamage += 10;
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
                bulletDamage += 5;
                penetrations++;
                break;
            default:
                print("Default glock.");
                break;
        }
    }
}

