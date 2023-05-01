using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mossberg : Weapon
{
    public GameObject bulletPrefab;
    public int pellets = 10;
    public float bulletSpeed = 20f;
    private float lastShotTime;

    public override void Shoot(Transform firePoint)
    {
        if (Time.time - lastShotTime > 1.0f)
        {
            //Debug.Log("Fired New Mossberg!");

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
    }
}
