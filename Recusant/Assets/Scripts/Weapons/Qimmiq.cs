using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qimmiq : Weapon
{
    private float lastShotTime;
    
    protected new int localWeaponlevel;
    protected new int numOfShots = 1;
    protected new float shotFrequency = 0.1f;
    
    private int numberOfQimmiqAllowed = 1;
    private int numberOfQimmiqSpawned = 0;
    public int damage = 10;
    public int speed = 0;
    private List<GameObject> listOfQimmiqAttacks = new List<GameObject>();

    public override void Shoot(Transform firePoint, int weaponLevel)
    {
        WeaponLevels(weaponLevel);
        if (Time.time - lastShotTime > shotFrequency && numberOfQimmiqSpawned < numberOfQimmiqAllowed)
        {
            GameObject qimmiqAttack = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/QimmiqAttack"));
            QimmiqAttack qAScript = qimmiqAttack.GetComponent<QimmiqAttack>();
            qAScript.qimmiqDamage += damage;
            qAScript.moveSpeed += speed;
            listOfQimmiqAttacks.Add(qimmiqAttack);
            qimmiqAttack.transform.position = firePoint.position;
            lastShotTime = Time.time;
            AudioManager.instance.Play("SingleBark");
            numberOfQimmiqSpawned++;
        }
    }
    
    private void KillAndRespawnQimmiqs()
    {
        foreach (var qimmy in listOfQimmiqAttacks)
        {
            Destroy(qimmy);
        }

        numberOfQimmiqSpawned = 0;
        
        // After destroying the objects, clear the list
        listOfQimmiqAttacks.Clear();
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
                print ("Lvl 1 Qimmiq");
                break;
            case 2:
                print ("Lvl 2 Qimmiq");
                damage += 5;
                KillAndRespawnQimmiqs();
                break;
            case 3:
                print ("Lvl 3 Qimmiq");
                speed++;
                KillAndRespawnQimmiqs();
                break;
            case 4:
                print ("Lvl 4 Qimmiq");
                damage += 5;
                break;
            case 5:
                print ("Lvl 5 Qimmiq");
                numberOfQimmiqAllowed++;
                KillAndRespawnQimmiqs();
                print("inlvl5: " + numberOfQimmiqAllowed);
                break;
            case 6:
                print ("Lvl 6 Qimmiq");
                damage += 5;
                KillAndRespawnQimmiqs();
                break;
            case 7:
                print ("Lvl 7 Qimmiq");
                speed++;
                KillAndRespawnQimmiqs();
                break;
            case 8:
                print ("Lvl 8 Qimmiq");
                damage += 5;
                KillAndRespawnQimmiqs();
                break;
            case 9:
                print ("Lvl 9 Qimmiq");
                speed++;
                KillAndRespawnQimmiqs();
                break;
            case 10:
                print ("Lvl 10 Qimmiq");
                numberOfQimmiqAllowed++;
                KillAndRespawnQimmiqs();
                break;
            default:
                print ("Default Qimmiq.");
                break;
        }
    }
}
