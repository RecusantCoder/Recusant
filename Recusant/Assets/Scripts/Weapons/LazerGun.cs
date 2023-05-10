using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerGun : Weapon
{
    protected new int localWeaponlevel;
    protected new int numOfShots = 1;
    protected new float shotFrequency = 2.5f;
    private int damage = 10;
    protected new List<float> times = new List<float>();
    protected new List<float> firedList = new List<float>();
    
    private float lazerLength = 0.1f;
    private float lazerWidth = 0.1f;

    public override void Shoot(Transform firePoint, int weaponLevel)
    {
        WeaponLevels(weaponLevel);

        for (int i = 0; i < numOfShots; i++)
        {
            if (times.Capacity < numOfShots)
            {
                for (int j = 0; j < 100; j++)
                {
                    times.Add(0);
                    firedList.Add(0);
                }
            }
            float shotFrequency3 = 2.0f + (0.1f * i);
            
            if (firedList[i] > 1)
            {
                shotFrequency3 = 2.0f;
            }

            if (Time.time - times[i] > shotFrequency3)
            {
                firedList[i]++;

                FireShot(firePoint, weaponLevel);
            
                times[i] = Time.time;
            }
        }
    }

    protected override void FireShot(Transform firePoint, int weaponLevel)
    {

        Quaternion rotationA = Quaternion.Euler(0f, 0f, 90f);
        //Vector3 offsetA = new Vector3(1f, 0f, 0f);
        GameObject lazerBeam = Instantiate(Resources.Load<GameObject>("Prefabs/LazerBeam"), firePoint.position, firePoint.rotation * rotationA);
        Rigidbody2D rb = lazerBeam.GetComponent<Rigidbody2D>();
        AudioManager.instance.Play("lazerGunGunshot");
        
        // Modify the scale of the lazerBeam
        lazerBeam.transform.localScale = new Vector3(lazerLength, lazerWidth, 1f);
        
        // Get the script component and change its damage amount
        LazerBeam lazerBeamScript = lazerBeam.GetComponent<LazerBeam>();
        lazerBeamScript.lazerDamage += damage; // Change the damage amount
        
        Debug.Log("lazergun. SF: " + shotFrequency + " LL: " + lazerLength + " LW: " + lazerWidth + " d: " + lazerBeamScript.lazerDamage);
        
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
                shotFrequency -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                damage += 10;
                break;
            case 2:
                print ("Lvl 2 lazerGun");
                shotFrequency -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                break;
            case 3:
                print ("Lvl 3 lazerGun");
                shotFrequency -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                damage += 10;
                break;
            case 4:
                print ("Lvl 4 lazerGun");
                shotFrequency -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                break;
            case 5:
                print ("Lvl 5 lazerGun");
                shotFrequency -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                break;
            case 6:
                print ("Lvl 6 lazerGun");
                shotFrequency -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                damage += 10;
                break;
            case 7:
                print ("Lvl 7 lazerGun");
                shotFrequency -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                break;
            case 8:
                print ("Lvl 8 lazerGun");
                shotFrequency -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                break;
            case 9:
                print ("Lvl 9 lazerGun");
                shotFrequency -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                damage += 10;
                break;
            case 10:
                print ("Lvl 10 lazerGun");
                shotFrequency -= 0.1f;
                lazerLength += 0.1f;
                lazerWidth += 0.1f;
                numOfShots++;
                Debug.Log("Level10 i l t d");
                break;
            default:
                print ("Default lazerGun.");
                break;
        }
    }
}
