using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Flamethrower : Weapon
{
    private int flameDamage = 1;
    public float flameSpeed = 5f;
    public float flameSpread = 30f;

    protected new int localWeaponlevel;
    protected new int numOfShots = 10;
    protected new float shotFrequency = 1.0f;
    protected new List<float> times = new List<float>();
    protected new List<float> firedList = new List<float>();

    private int flameAudioCounter;
    
    
    
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
            float shotFrequency3 = 1.5f + (0.1f * i);
            
            if (firedList[i] > 1)
            {
                shotFrequency3 = 1.5f;
            }

            if (Time.time - times[i] > shotFrequency3)
            {
                firedList[i]++;

                FireShot(firePoint, weaponLevel);

                switch (flameAudioCounter)
                {
                    case 0:
                        AudioManager.instance.Play("Flame1");
                        break;
                    case 1:
                        AudioManager.instance.Play("Flame2");
                        break;
                    default:
                        AudioManager.instance.Play("Flame3");
                        break;
                }
                if (flameAudioCounter < 2)
                {
                    flameAudioCounter++;
                }
                else
                {
                    flameAudioCounter = 0;
                }
                
                times[i] = Time.time;
            }
        }
    }
    

    protected override void FireShot(Transform firePoint, int weaponLevel)
    {
        Quaternion rotationA = Quaternion.Euler(0f, 0f, Random.Range(-flameSpread, flameSpread));
        GameObject flame = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/Flame"), firePoint.position, firePoint.rotation * rotationA);
        Rigidbody2D rb = flame.GetComponent<Rigidbody2D>();
        
        rb.AddForce(flame.transform.up * flameSpeed, ForceMode2D.Impulse);

        // Get the bullet script component and change its damage amount
        Flame flameScript = flame.GetComponent<Flame>();
        Debug.Log("Flame damage before = " + flameScript.Damage);
        flameScript.Damage = flameScript.Damage += flameDamage;
        Debug.Log("Flame damage after = " + flameScript.Damage);
        flameScript.firePointLocal = firePoint;
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
                print("Lvl 1 Flamethrower");
                break;
            case 2:
                print("Lvl 2 Flamethrower");
                flameDamage += 10;
                numOfShots += 1;
                break;
            case 3:
                print("Lvl 3 Flamethrower");
                flameDamage += 10;
                numOfShots += 1;
                break;
            case 4:
                print("Lvl 4 Flamethrower");
                flameDamage += 10;
                numOfShots += 1;
                break;
            case 5:
                print("Lvl 5 Flamethrower");
                flameDamage += 10;
                numOfShots += 1;
                break;
            case 6:
                print("Lvl 6 Flamethrower");
                flameDamage += 10;
                numOfShots += 1;
                numOfShots++;
                break;
            case 7:
                print("Lvl 7 Flamethrower");
                flameDamage += 10;
                numOfShots += 1;
                break;
            case 8:
                print("Lvl 8 Flamethrower");
                flameDamage += 10;
                numOfShots += 1;
                numOfShots++;
                break;
            case 9:
                print("Lvl 9 Flamethrower");
                flameDamage += 10;
                numOfShots += 1;
                break;
            case 10:
                print("Lvl 10 Flamethrower");
                flameDamage += 10;
                numOfShots += 1;
                numOfShots++;
                break;
            default:
                print("Default Flamethrower.");
                break;
        }
    }
}

