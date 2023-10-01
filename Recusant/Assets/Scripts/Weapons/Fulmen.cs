using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fulmen : Weapon
{
    protected new int localWeaponlevel;
    protected new int numOfShots = 1;

    public float groupDelay = 60.0f;
    public float shotDelay = 1.0f;
    private bool isFiring = false;
    private Coroutine firingCoroutine;
    private int weaponLevelPassed;
    private Transform firePointPassed;

    private float fulmenChanceToSave = 15.0f;
    private bool isSafe = false;

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
        GameObject lightning = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/Lightning"), firePoint.position, Quaternion.identity);
        Lightning lightningScript = lightning.GetComponent<Lightning>();
        lightningScript.safeLightning = IsSaved();
        AudioManager.instance.Play("lightning");
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
                print ("Lvl 1 Fulmen");
                groupDelay -= 6.0f;
                break;
            case 2:
                print ("Lvl 2 Fulmen");
                fulmenChanceToSave += 10.0f;
                break;
            case 3:
                print ("Lvl 3 Fulmen");
                groupDelay -= 6.0f;
                break;
            case 4:
                print ("Lvl 4 Fulmen");
                fulmenChanceToSave += 10.0f;
                break;
            case 5:
                print ("Lvl 5 Fulmen");
                groupDelay -= 6.0f;
                break;
            case 6:
                print ("Lvl 6 Fulmen");
                fulmenChanceToSave += 10.0f;
                break;
            case 7:
                print ("Lvl 7 Fulmen");
                groupDelay -= 6.0f;
                break;
            case 8:
                print ("Lvl 8 Fulmen");
                fulmenChanceToSave += 10.0f;
                break;
            case 9:
                print ("Lvl 9 Fulmen");
                groupDelay -= 6.0f;
                break;
            case 10:
                print ("Lvl 10 Fulmen");
                fulmenChanceToSave += 10.0f;
                break;
            default:
                print ("Default Fulmen.");
                break;
        }
        print (" with group delay: " + groupDelay + " and chance: " + fulmenChanceToSave);
    }
    
    bool IsSaved()
    {
        int randomNumber = Random.Range(1, 101);
        bool saved = randomNumber <= fulmenChanceToSave;
        Debug.Log("randomNumber: " + randomNumber + " chanceToSave: " + fulmenChanceToSave + " IsSaved: " + saved);
        return saved;
    }
}
