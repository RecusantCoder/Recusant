using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerGun : Weapon
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    private float lastShotTime;

    public override void Shoot(Transform firePoint, int weaponLevel)
    {
        
        if (Time.time - lastShotTime > 2.5f)
        {
            lazerGunLevels(weaponLevel);
            Quaternion rotationA = Quaternion.Euler(0f, 0f, 90f);
            //Vector3 offsetA = new Vector3(1f, 0f, 0f);
            GameObject lazerBeam = Instantiate(Resources.Load<GameObject>("Prefabs/LazerBeam"), firePoint.position, firePoint.rotation * rotationA);
            Rigidbody2D rb = lazerBeam.GetComponent<Rigidbody2D>();
            lastShotTime = Time.time;
            AudioManager.instance.Play("lazerGunGunshot");
        }
        
        void lazerGunLevels(int weaponLevel)
        {
            switch (weaponLevel)
            {
                case 1:
                    print ("Lvl 1 lazerGun");
                    break;
                case 2:
                    print ("Lvl 2 lazerGun");
                    break;
                case 3:
                    print ("Lvl 3 lazerGun");
                    break;
                case 4:
                    print ("Lvl 4 lazerGun");
                    break;
                case 5:
                    print ("Lvl 5 lazerGun");
                    break;
                case 6:
                    print ("Lvl 6 lazerGun");
                    break;
                case 7:
                    print ("Lvl 7 lazerGun");
                    break;
                case 8:
                    print ("Lvl 8 lazerGun");
                    break;
                case 9:
                    print ("Lvl 9 lazerGun");
                    break;
                case 10:
                    print ("Lvl 10 lazerGun");
                    break;
                default:
                    print ("Default lazerGun.");
                    break;
            }
        }
    }
}
