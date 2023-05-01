using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : Weapon
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    private float lastShotTime;

    public override void Shoot(Transform firePoint)
    {
        if (Time.time - lastShotTime > 0.5f)
        {
            Debug.Log("Fired New Glock!");
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet2"), firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * bulletSpeed, ForceMode2D.Impulse);
            lastShotTime = Time.time;
            AudioManager.instance.Play("pistolGunshot");
        }
    }
}

