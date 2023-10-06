using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashbang : Weapon
{
    public int damage = 0;
    public int localWeaponLevel;
    public float throwSpeed = 3.0f;
    public int numOfThrows = 1;
    private int penetrations = 0;
    private int flashRadius = 1;
    private float flashbangDuration = 1.0f;
    
    
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
            Debug.Log("group delay: " + groupDelay);
            WeaponLevels(weaponLevelPassed);
            FireShot(firePointPassed, weaponLevelPassed);
            yield return new WaitForSeconds(shotDelay);
        }
        yield return new WaitForSeconds(groupDelay);
        isFiring = false;
    }

    protected override void FireShot(Transform firePoint, int weaponLevel)
    {
        Debug.Log("Throwing flashbang");
        Quaternion rotationA = Quaternion.Euler(0f, 0f, 0f);
        GameObject thrownGrenade = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/ThrownFlashbang"), firePoint.position, firePoint.rotation * rotationA);
        Rigidbody2D rb = thrownGrenade.GetComponent<Rigidbody2D>();
        rb.AddForce(thrownGrenade.transform.up * throwSpeed, ForceMode2D.Impulse);
        
        AudioManager.instance.Play("Throw");
        
        // Apply torque for spinning motion
        float torqueForce = 10f;
        rb.AddTorque(torqueForce, ForceMode2D.Impulse);
        
        //Modify the values on the thrownGrenade
        ThrownFlashbang thrownGrenadeScript = thrownGrenade.GetComponent<ThrownFlashbang>();
        thrownGrenadeScript.thrownFlashbangDuration += flashbangDuration;
        thrownGrenadeScript.effectRadius += flashRadius;
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
                print("Lvl 2 flash");
                flashbangDuration++;
                break;
            case 3:
                print("Lvl 3 flash");
                groupDelay -= 0.5f;
                break;
            case 4:
                print("Lvl 4 flash");
                flashRadius ++;
                break;
            case 5:
                print("Lvl 5 flash");
                flashbangDuration++;
                break;
            case 6:
                print("Lvl 6 flash");
                groupDelay -= 0.5f;
                break;
            case 7:
                print("Lvl 7 flash");
                flashRadius ++;
                break;
            case 8:
                print("Lvl 8 flash");
                flashbangDuration++;
                break;
            case 9:
                print("Lvl 9 flash");
                groupDelay -= 0.5f;
                break;
            case 10:
                print("Lvl 10 flash");
                flashRadius ++;
                break;
            default:
                print("Default flash.");
                break;
        }
    }
}
