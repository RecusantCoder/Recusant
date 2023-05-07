using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fulmen : Weapon
{
    private float lastShotTime;

    public override void Shoot(Transform firePoint, int weaponLevel)
    {
        
        if (Time.time - lastShotTime > 2.0f)
        {
            fulmenLevels(weaponLevel);
            GameObject lightning = Instantiate(Resources.Load<GameObject>("Prefabs/Lightning"), firePoint.position, firePoint.rotation);
            lastShotTime = Time.time;
            AudioManager.instance.Play("lightning");
        }
    }

    void fulmenLevels(int weaponLevel)
    {
        switch (weaponLevel)
        {
            case 1:
                print ("Lvl 1 Fulmen");
                break;
            case 2:
                print ("Lvl 2 Fulmen");
                break;
            case 3:
                print ("Lvl 3 Fulmen");
                break;
            case 4:
                print ("Lvl 4 Fulmen");
                break;
            case 5:
                print ("Lvl 5 Fulmen");
                break;
            case 6:
                print ("Lvl 6 Fulmen");
                break;
            case 7:
                print ("Lvl 7 Fulmen");
                break;
            case 8:
                print ("Lvl 8 Fulmen");
                break;
            case 9:
                print ("Lvl 9 Fulmen");
                break;
            case 10:
                print ("Lvl 10 Fulmen");
                break;
            default:
                print ("Default Fulmen.");
                break;
        }
    }
}
