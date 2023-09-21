using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qimmiq : Weapon
{
    private float lastShotTime;
    
    protected new int localWeaponlevel;
    protected new int numOfShots = 1;
    protected new float shotFrequency = 5.0f;
    protected new List<float> times = new List<float>();
    protected new List<float> firedList = new List<float>();
    private int numberOfQimmiqAllowed = 1;
    private int numberOfQimmiqSpawned = 0;
    public int damage = 10;
    public int speed = 0;

    public override void Shoot(Transform firePoint, int weaponLevel)
    {

        if (Time.time - lastShotTime > shotFrequency && numberOfQimmiqSpawned < numberOfQimmiqAllowed)
        {
            Debug.Log("1Allowed: " + numberOfQimmiqAllowed + " spawned: " + numberOfQimmiqSpawned);
            WeaponLevels(weaponLevel);
            Debug.Log("2Allowed: " + numberOfQimmiqAllowed + " spawned: " + numberOfQimmiqSpawned);
            GameObject qimmiqAttack = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/QimmiqAttack"));
            qimmiqAttack.GetComponent<QimmiqAttack>().qimmiqDamage += damage;
            qimmiqAttack.GetComponent<QimmiqAttack>().moveSpeed += speed;
            qimmiqAttack.transform.position = firePoint.position;
            lastShotTime = Time.time;
            AudioManager.instance.Play("SingleBark");
            numberOfQimmiqSpawned++;
            Debug.Log("3Allowed: " + numberOfQimmiqAllowed + " spawned: " + numberOfQimmiqSpawned);
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
                print ("Lvl 1 Qimmiq");
                print("inlvl1: " + numberOfQimmiqAllowed);
                break;
            case 2:
                print ("Lvl 2 Qimmiq");
                damage += 5;
                break;
            case 3:
                print ("Lvl 3 Qimmiq");
                speed++;
                break;
            case 4:
                print ("Lvl 4 Qimmiq");
                damage += 5;
                break;
            case 5:
                print ("Lvl 5 Qimmiq");
                numberOfQimmiqAllowed++;
                print("inlvl5: " + numberOfQimmiqAllowed);
                break;
            case 6:
                print ("Lvl 6 Qimmiq");
                damage += 5;
                break;
            case 7:
                print ("Lvl 7 Qimmiq");
                speed++;
                break;
            case 8:
                print ("Lvl 8 Qimmiq");
                damage += 5;
                break;
            case 9:
                print ("Lvl 9 Qimmiq");
                speed++;
                break;
            case 10:
                print ("Lvl 10 Qimmiq");
                numberOfQimmiqAllowed++;
                break;
            default:
                print ("Default Qimmiq.");
                break;
        }
    }
}
