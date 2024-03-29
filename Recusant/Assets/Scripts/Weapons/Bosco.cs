using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bosco : Weapon
{
    protected new int localWeaponlevel;
    protected new int numOfShots = 1;

    public float groupDelay = 0.1f;
    public float shotDelay = 1.0f;
    private bool isFiring = false;
    private Coroutine firingCoroutine;
    private int weaponLevelPassed;
    private Transform firePointPassed;

    private int numberOfProjectilesAllowed = 1;
    private int numberOfProjectilesSpawned = 0;

    public override void Shoot(Transform firePoint, int weaponLevel)
    {
        firePointPassed = firePoint;
        weaponLevelPassed = weaponLevel;
        if (!isFiring)
        {
            firingCoroutine = StartCoroutine(FireGroups());
        }

        if (weaponLevel > localWeaponlevel)
        {
            RestartFiring();
        }
    }
    
    // Method to stop the current coroutine and start a new one
    private void RestartFiring()
    {
        if (isFiring)
        {
            // Stop the current firing coroutine
            StopCoroutine(firingCoroutine);
            isFiring = false;
        }

        // Start a new firing coroutine
        firingCoroutine = StartCoroutine(FireGroups());
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
        if (numberOfProjectilesSpawned < numberOfProjectilesAllowed)
        {
            GameObject boscoAttack = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/BoscoAttack"), firePoint.position, Quaternion.identity);
            numberOfProjectilesSpawned++;
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
                GameManager.instance.player.GetComponent<CharacterStats>().revives.AddModifier(1);
                Debug.Log("Added revive");
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            default:
                print ("Default BoscoAttack.");
                break;
        }
    }

    public void ProjectileDestroyed()
    {
        numberOfProjectilesSpawned--;
    }
}
