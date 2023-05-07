using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : Weapon
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    private float lastShotTime;

    public override void Shoot(Transform firePoint, int weaponLevel)
    {
        
        if (Time.time - lastShotTime > 0.5f)
        {
            glockLevels(weaponLevel);
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet2"), firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * bulletSpeed, ForceMode2D.Impulse);
            lastShotTime = Time.time;
            AudioManager.instance.Play("pistolGunshot");
        }
        
        void glockLevels(int weaponLevel)
        {
            switch (weaponLevel)
            {
                case 1:
                    print ("Lvl 1 glock");
                    break;
                case 2:
                    print ("Lvl 2 glock");
                    break;
                case 3:
                    print ("Lvl 3 glock");
                    break;
                case 4:
                    print ("Lvl 4 glock");
                    break;
                case 5:
                    print ("Lvl 5 glock");
                    break;
                case 6:
                    print ("Lvl 6 glock");
                    break;
                case 7:
                    print ("Lvl 7 glock");
                    break;
                case 8:
                    print ("Lvl 8 glock");
                    break;
                case 9:
                    print ("Lvl 9 glock");
                    break;
                case 10:
                    print ("Lvl 10 glock");
                    break;
                default:
                    print ("Default glock.");
                    break;
            }
        }
    }
}

