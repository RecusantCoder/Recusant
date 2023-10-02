using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Flamethrower : Weapon
{
    private int flameDamage = 1;
    public float flameSpeed = 2f;
    public float flameSpread = 30f;

    protected new int localWeaponlevel;
    protected new int numOfShots = 10;
    
    public float groupDelay = 3.0f;
    public float shotDelay = 0.1f;
    private bool isFiring = false;
    private Coroutine firingCoroutine;
    private int weaponLevelPassed;
    private Transform firePointPassed;

    private int flameAudioCounter;
    private int shotOffset = 0;
    private int directionCounter = 0;
    
    
    
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
        
        //Offsetting the firepoint
        shotOffset = 0;
        Vector3 offsetFirepoint = firePointPassed.eulerAngles;
        offsetFirepoint.z = SetupFirepointPassed();
        for (int i = 0; i < numOfShots; i++)
        { 
            shotOffset += 1;
            WeaponLevels(weaponLevelPassed);
            
            offsetFirepoint.z += shotOffset;
            firePointPassed.eulerAngles = offsetFirepoint;
            FireShot(firePointPassed, weaponLevelPassed);

            switch (flameAudioCounter)
            {
                case 0:
                    AudioManager.instance.Play("Flame1");
                    break;
                case 1:
                    AudioManager.instance.Play("Flame2");
                    break;
                default:
                    AudioManager.instance.Play("Flame3");
                    break;
            }
            if (flameAudioCounter < 2)
            {
                flameAudioCounter++;
            }
            else
            {
                flameAudioCounter = 0;
            }
            yield return new WaitForSeconds(shotDelay);
        }
        yield return new WaitForSeconds(groupDelay);
        isFiring = false;
    }

    private int SetupFirepointPassed()
    {
        int angle = 0;
        if (directionCounter == 4)
        {
            directionCounter = 0;
        }

        switch (directionCounter)
        {
            case 0:
                angle = 0 -30;
                break;
            case 1:
                angle = 90 -30;
                break;
            case 2:
                angle = 180 -30;
                break;
            case 3:
                angle = 270 -30;
                break;
        }

        directionCounter++;
        return angle;
    }
    

    protected override void FireShot(Transform firePoint, int weaponLevel)
    {
        GameObject flame = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/Flame"), firePoint.position, firePoint.rotation);
        Rigidbody2D rb = flame.GetComponent<Rigidbody2D>();
        
        rb.AddForce(flame.transform.up * flameSpeed, ForceMode2D.Impulse);

        // Get the bullet script component and change its damage amount
        Flame flameScript = flame.GetComponent<Flame>();
        flameScript.Damage = flameScript.Damage += flameDamage;
        flameScript.firePointLocal = firePoint;
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
                print("Lvl 1 Flamethrower");
                break;
            case 2:
                print("Lvl 2 Flamethrower");
                flameDamage += 1;
                numOfShots += 1;
                break;
            case 3:
                print("Lvl 3 Flamethrower");
                flameDamage += 1;
                flameSpeed += 0.25f;
                break;
            case 4:
                print("Lvl 4 Flamethrower");
                flameDamage += 1;
                numOfShots += 1;
                break;
            case 5:
                print("Lvl 5 Flamethrower");
                flameDamage += 1;
                flameSpeed += 0.25f;
                break;
            case 6:
                print("Lvl 6 Flamethrower");
                flameDamage += 1;
                numOfShots++;
                break;
            case 7:
                print("Lvl 7 Flamethrower");
                flameDamage += 1;
                flameSpeed += 0.25f;
                break;
            case 8:
                print("Lvl 8 Flamethrower");
                flameDamage += 1;
                numOfShots++;
                break;
            case 9:
                print("Lvl 9 Flamethrower");
                flameDamage += 1;
                flameSpeed += 0.25f;
                break;
            case 10:
                print("Lvl 10 Flamethrower");
                flameDamage += 1;
                numOfShots++;
                break;
            default:
                print("Default Flamethrower.");
                break;
        }
    }
}

