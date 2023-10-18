using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwoleSamoyed : Weapon
{
    private float lastShotTime;
    
    protected new int localWeaponlevel;
    protected new int numOfShots = 1;
    protected new float shotFrequency = 0.1f;
    
    private int numberOfSwoleSamoyedAllowed = 1;
    private int numberOfSwoleSamoyedSpawned = 0;
    public int damage = 100;
    public int speed = 0;
    private List<GameObject> listOfSwoleSamoyedAttacks = new List<GameObject>();

    public override void Shoot(Transform firePoint, int weaponLevel)
    {
        WeaponLevels(weaponLevel);
        if (Time.time - lastShotTime > shotFrequency && numberOfSwoleSamoyedSpawned < numberOfSwoleSamoyedAllowed)
        {
            GameObject swoleSamoyedAttack = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/SwoleSamoyedAttack"));
            SwoleSamoyedAttack swoleSamoyedScript = swoleSamoyedAttack.GetComponent<SwoleSamoyedAttack>();
            swoleSamoyedScript.swoleSamoyedDamage += damage;
            swoleSamoyedScript.moveSpeed += speed;
            listOfSwoleSamoyedAttacks.Add(swoleSamoyedAttack);
            swoleSamoyedAttack.transform.position = firePoint.position;
            lastShotTime = Time.time;
            AudioManager.instance.Play("deepBark");
            numberOfSwoleSamoyedSpawned++;
        }
    }
    
    private void KillAndRespawnSwoleSamoyed()
    {
        foreach (var swoleSam in listOfSwoleSamoyedAttacks)
        {
            Destroy(swoleSam);
        }

        numberOfSwoleSamoyedSpawned = 0;
        
        // After destroying the objects, clear the list
        listOfSwoleSamoyedAttacks.Clear();
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
                print ("Default Qimmiq.");
                break;
        }
    }
}
