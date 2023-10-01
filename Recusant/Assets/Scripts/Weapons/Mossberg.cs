using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mossberg : Weapon
{
    public int pellets = 5;
    public float bulletSpeed = 20f;
    private float pelletSpread = 20f;
    private int bulletDamage = 5;
    private int penetrations = 0;
    
    protected new int localWeaponlevel;
    protected new int numOfShots = 1;
    
    
    public float groupDelay = 2.0f;
    public float shotDelay = 0.1f;
    private bool isFiring = false;
    private Coroutine firingCoroutine;
    private int weaponLevelPassed;
    private Transform firePointPassed;
    
    
    public override void Shoot(Transform firePoint, int weaponLevel)
    {
        firePointPassed = firePoint;
        weaponLevelPassed = weaponLevel;
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

        for (int i = 0; i < pellets; i++)
        {
            Quaternion rotationA = Quaternion.Euler(0f, 0f, Random.Range(-pelletSpread, pelletSpread));
            GameObject pellet = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/Pellet"), firePoint.position, firePoint.rotation * rotationA);
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
    

    protected override void UpdateWeaponBehavior(int level)
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
