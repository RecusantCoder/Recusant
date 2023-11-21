using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeStorm : Weapon
{
    protected new int localWeaponlevel;
    protected new int numOfShots = 8;

    public float groupDelay = 0.001f;
    public float shotDelay = 0.16f;
    private bool isFiring = false;
    private Coroutine firingCoroutine;
    private int weaponLevelPassed;
    private Transform firePointPassed;
    
    private int numberOfProjectilesAllowed = 100;
    private int numberOfProjectilesSpawned = 0;
    
    private Vector2 direction;
    private float speed = 5;

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
            
            // Calculate the angle in radians
            float angleInRadians = i * Mathf.PI / 4;
            // Calculate the direction vector
            direction = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
            
            yield return new WaitForSeconds(shotDelay);
        }
        yield return new WaitForSeconds(groupDelay);
        isFiring = false;
    }
    
    protected override void FireShot(Transform firePoint, int weaponLevel)
    {
        if (numberOfProjectilesSpawned < numberOfProjectilesAllowed)
        {
            GameObject attack = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/BladeStormAttack"),
                firePoint.position, Quaternion.identity);
            numberOfProjectilesSpawned++;
            
            Rigidbody2D rb = attack.GetComponent<Rigidbody2D>();
            
            // Rotate the bullet to face the specified direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            
            // Add a force to the bullet in the specified direction
            rb.AddForce(direction * 10f, ForceMode2D.Impulse);
            Destroy(attack, 1.0f);
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
                print ("Default BladeStormAttack.");
                break;
        }
    }
    
    public void ProjectileDestroyed()
    {
        numberOfProjectilesSpawned--;
    }
}
