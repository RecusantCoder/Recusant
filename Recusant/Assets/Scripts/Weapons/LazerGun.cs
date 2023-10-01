using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerGun : Weapon
{
    protected new int localWeaponlevel;
    protected new int numOfShots = 1;
    private int damage = 10;
    private float lazerLength = 0.1f;
    private float lazerWidth = 0.1f;
    
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

        Quaternion rotationA = Quaternion.Euler(0f, 0f, 90f);
        GameObject lazerBeam = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/LazerBeam"), firePoint.position, firePoint.rotation * rotationA);
        Rigidbody2D rb = lazerBeam.GetComponent<Rigidbody2D>();
        AudioManager.instance.Play("lazerGunGunshot");
        
        // Modify the scale of the lazerBeam
        lazerBeam.transform.localScale = new Vector3(lazerLength, lazerWidth, 1f);
        
        // Get the script component and change its damage amount
        LazerBeam lazerBeamScript = lazerBeam.GetComponent<LazerBeam>();
        lazerBeamScript.lazerDamage += damage; // Change the damage amount
        
        Debug.Log("lazergun. groupDelay: " + groupDelay + " LazerLength: " + lazerLength + " LazerWidth: " + lazerWidth);
        
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
                print ("Lvl 1 lazerGun");
                groupDelay -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                damage += 10;
                break;
            case 2:
                print ("Lvl 2 lazerGun");
                groupDelay -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                break;
            case 3:
                print ("Lvl 3 lazerGun");
                groupDelay -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                damage += 10;
                break;
            case 4:
                print ("Lvl 4 lazerGun");
                groupDelay -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                break;
            case 5:
                print ("Lvl 5 lazerGun");
                groupDelay -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                break;
            case 6:
                print ("Lvl 6 lazerGun");
                groupDelay -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                damage += 10;
                break;
            case 7:
                print ("Lvl 7 lazerGun");
                groupDelay -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                break;
            case 8:
                print ("Lvl 8 lazerGun");
                groupDelay -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                break;
            case 9:
                print ("Lvl 9 lazerGun");
                groupDelay -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                damage += 10;
                break;
            case 10:
                print ("Lvl 10 lazerGun");
                groupDelay -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                numOfShots++;
                break;
            default:
                print ("Default lazerGun.");
                break;
        }
    }
}
