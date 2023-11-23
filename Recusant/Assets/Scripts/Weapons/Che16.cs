using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Che16 : Weapon
{
    protected new int localWeaponlevel;
    protected new int numOfShots = 2;
    private float pelletSpread = 20f;
    private int damage = 5;
    private int penetrations = 0;
    public int pellets = 5;
    public float bulletSpeed = 20f;
    public int radius = 1;
    public int duration = 5;

    public float groupDelay = 0.001f;
    public float shotDelay = 0.25f;
    private bool isFiring = false;
    private Coroutine firingCoroutine;
    private int weaponLevelPassed;
    private Transform firePointPassed;
    
    
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
        AudioManager.instance.Play("shotgunGunshot");
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
            GameObject attack = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/ThrownMolotov"), firePoint.position, firePoint.rotation * rotationA);
            Rigidbody2D rb = attack.GetComponent<Rigidbody2D>();
            rb.AddForce(attack.transform.up * bulletSpeed, ForceMode2D.Impulse);
            // Apply torque for spinning motion
            float torqueForce = 10f;
            rb.AddTorque(torqueForce, ForceMode2D.Impulse);
           
            //Modify the values on the thrownGrenade
            ThrownMolotov attackScript = attack.GetComponent<ThrownMolotov>();
            attackScript.grenadeDamage += damage;
            attackScript.effectRadius += radius;
            attackScript.thrownMolotovDuration += duration;

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
                print ("Default Che16.");
                break;
        }
    }
    
}
