using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mossberg : Weapon
{
    public GameObject bulletPrefab;
    public int pellets = 10;
    public float bulletSpeed = 20f;
    private float lastShotTime;

    public override void Shoot(Transform firePoint, int weaponLevel)
    {
        
        if (Time.time - lastShotTime > 1.0f)
        {
            mossbergLevels(weaponLevel);

            for (int i = 0; i < pellets; i++)
            {
                Quaternion rotationA = Quaternion.Euler(0f, 0f, Random.Range(-20f, 20f));
                GameObject pellet = Instantiate(Resources.Load<GameObject>("Prefabs/Pellet"), firePoint.position, firePoint.rotation * rotationA);
                Rigidbody2D rb = pellet.GetComponent<Rigidbody2D>();
                rb.AddForce(pellet.transform.up * bulletSpeed, ForceMode2D.Impulse);
                        
            };
            lastShotTime = Time.time;
            AudioManager.instance.Play("shotgunGunshot");
        }
        
        void mossbergLevels(int weaponLevel)
        {
            switch (weaponLevel)
            {
                case 1:
                    print ("Lvl 1 mossberg");
                    break;
                case 2:
                    print ("Lvl 2 mossberg");
                    break;
                case 3:
                    print ("Lvl 3 mossberg");
                    break;
                case 4:
                    print ("Lvl 4 mossberg");
                    break;
                case 5:
                    print ("Lvl 5 mossberg");
                    break;
                case 6:
                    print ("Lvl 6 mossberg");
                    break;
                case 7:
                    print ("Lvl 7 mossberg");
                    break;
                case 8:
                    print ("Lvl 8 mossberg");
                    break;
                case 9:
                    print ("Lvl 9 mossberg");
                    break;
                case 10:
                    print ("Lvl 10 mossberg");
                    break;
                default:
                    print ("Default mossberg.");
                    break;
            }
        }
    }
}
