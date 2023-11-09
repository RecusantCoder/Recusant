using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniNuke : Weapon
{
    public int damage = 0;
    public int localWeaponLevel;
    public float throwSpeed = 3f;
    public int numOfThrows = 1;
    private int penetrations = 0;
    private int grenadeRadius = 1;
    
    
    public float groupDelay = 100.0f;
    public float shotDelay = 0.1f;
    private bool isFiring = false;
    private Coroutine firingCoroutine;
    private int weaponLevelPassed;
    private Transform firePointPassed;
    
    public float spawnRadius = 4.0f;
    public float spawnHeight = 10.0f;
    public float downwardForce = 1.0f;

    private GameObject crosshair;

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
        Debug.Log("Firing mini nuke");

        // Calculate a random horizontal offset within the spawnRadius.
        float randomXOffset = Random.Range(-spawnRadius, spawnRadius);
        // Calculate the spawn position above the player.
        //Vector3 spawnPosition = firePoint.transform.position + new Vector3(randomXOffset, spawnHeight, 0);
        float angle = Random.Range(0, 360);
        Vector3 endPosition = firePoint.transform.position + new Vector3(randomXOffset, 0, 0);
        
        GameObject crosshairPrefab = Resources.Load<GameObject>("Prefabs/Projectiles/GroundCrosshair");
        crosshair = Instantiate(crosshairPrefab, endPosition, Quaternion.identity);

        Vector3 spawnPosition = endPosition;
        spawnPosition.y += spawnHeight;
        
        // Instantiate the MiniNuke at the calculated position.
        GameObject miniNukePrefab = Resources.Load<GameObject>("Prefabs/Projectiles/MiniNukeAttack");
        GameObject miniNukeAttack = Instantiate(miniNukePrefab, spawnPosition, Quaternion.identity);
        MiniNukeAttack miniNukeAttackScript = miniNukeAttack.GetComponent<MiniNukeAttack>();
        miniNukeAttackScript.miniNukeAttackDamage += damage;
        miniNukeAttackScript.miniNukeSpawnPosition = spawnPosition;
        miniNukeAttackScript.miniNukeEndPosition = endPosition;
        miniNukeAttackScript.crosshair = crosshair;

        // Add a downward force to the MiniNuke.
        Rigidbody2D rb = miniNukeAttack.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(Vector2.down * downwardForce, ForceMode2D.Impulse);
        }
        
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
                print ("Default mininuke.");
                break;
        }
    }
}
