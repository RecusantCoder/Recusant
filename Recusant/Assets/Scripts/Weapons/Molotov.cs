using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : Weapon
{
    public int damage = 0;
    public int localWeaponLevel;
    public float throwSpeed = 10f;
    public int numOfThrows = 1;
    private int penetrations = 0;
    private int molotovRadius = 1;
    private float molotovDuration = 1.0f;
    
    public float groupDelay = 3.0f;
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
        for (int i = 0; i < numOfThrows; i++)
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
        Debug.Log("Throwing molotov");
        Quaternion rotationA = Quaternion.Euler(0f, 0f, 0f);
        GameObject thrownGrenade = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/ThrownMolotov"), firePoint.position, firePoint.rotation * rotationA);
        Rigidbody2D rb = thrownGrenade.GetComponent<Rigidbody2D>();
        rb.AddForce(thrownGrenade.transform.up * throwSpeed, ForceMode2D.Impulse);
        
        AudioManager.instance.Play("Throw");
        
        // Apply torque for spinning motion
        float torqueForce = 10f;
        rb.AddTorque(torqueForce, ForceMode2D.Impulse);
        
        //Modify the values on the thrownGrenade
        ThrownMolotov thrownGrenadeScript = thrownGrenade.GetComponent<ThrownMolotov>();
        thrownGrenadeScript.grenadeDamage += damage;
        thrownGrenadeScript.effectRadius += molotovRadius;
        thrownGrenadeScript.thrownMolotovDuration += molotovRadius;
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
                print("Lvl 2 molotov");
                molotovRadius++;
                break;
            case 3:
                print("Lvl 3 molotov");
                break;
            case 4:
                print("Lvl 4 molotov");
                groupDelay -= 0.5f;
                break;
            case 5:
                print("Lvl 5 molotov");
                break;
            case 6:
                print("Lvl 6 molotov");
                molotovRadius++;
                break;
            case 7:
                print("Lvl 7 molotov");
                break;
            case 8:
                print("Lvl 8 molotov");
                groupDelay -= 0.5f;
                break;
            case 9:
                print("Lvl 9 molotov");
                break;
            case 10:
                print("Lvl 10 molotov");
                molotovRadius++;
                break;
            default:
                print("Default molotov.");
                break;
        }
    }
}
