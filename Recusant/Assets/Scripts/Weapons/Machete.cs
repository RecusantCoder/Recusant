using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machete : Weapon
{
    private int macheteDamage = 5;
    protected new int localWeaponlevel;
    protected new int numOfShots = 1;
    
    protected new float shotFrequency = 1.0f;
    public float groupDelay = 3.0f;
    public float shotDelay = 0.1f;
    private bool isFiring = false;
    private Coroutine firingCoroutine;
    private int weaponLevelPassed;
    private Transform firePointPassed;

    private int slashAudioCounter;
    
    
    
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

            switch (slashAudioCounter)
            {
                case 0:
                    AudioManager.instance.Play("Slash1");
                    break;
                case 1:
                    AudioManager.instance.Play("Slash2");
                    break;
                default:
                    AudioManager.instance.Play("Slash3");
                    break;
            }
            if (slashAudioCounter < 2)
            {
                slashAudioCounter++;
            }
            else
            {
                slashAudioCounter = 0;
            }
            yield return new WaitForSeconds(shotDelay);
        }
        yield return new WaitForSeconds(groupDelay);
        isFiring = false;
    }
    

    protected override void FireShot(Transform firePoint, int weaponLevel)
    {
        GameObject slash = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/Slash"), firePoint.position, firePoint.rotation);
        Rigidbody2D rb = slash.GetComponent<Rigidbody2D>();

        // Get the bullet script component and change its damage amount
        Slash slashScript = slash.GetComponent<Slash>();
        slashScript.damage += macheteDamage; // Change the damage amount
        slashScript.firePointLocal = firePoint;
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
                numOfShots += 1;
                break;
            case 3:
                print("Lvl 3 Machete");
                macheteDamage += 5;
                break;
            case 4:
                print("Lvl 4 Machete");
                numOfShots += 1;
                break;
            case 5:
                print("Lvl 5 Machete");
                macheteDamage += 5;
                break;
            case 6:
                print("Lvl 6 Machete");
                numOfShots++;
                break;
            case 7:
                print("Lvl 7 Machete");
                macheteDamage += 5;
                break;
            case 8:
                print("Lvl 8 Machete");
                numOfShots++;
                break;
            case 9:
                print("Lvl 9 Machete");
                macheteDamage += 5;
                break;
            case 10:
                print("Lvl 10 Machete");
                numOfShots++;
                break;
            default:
                print("Default Machete.");
                break;
        }
    }
}
