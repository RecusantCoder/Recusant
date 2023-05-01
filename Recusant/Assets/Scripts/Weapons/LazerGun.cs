using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerGun : Weapon
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    private float lastShotTime;

    public override void Shoot(Transform firePoint)
    {
        if (Time.time - lastShotTime > 2.5f)
        {
            //Debug.Log("Fired New LazerGun!");
            Quaternion rotationA = Quaternion.Euler(0f, 0f, 90f);
            //Vector3 offsetA = new Vector3(1f, 0f, 0f);
            GameObject lazerBeam = Instantiate(Resources.Load<GameObject>("Prefabs/LazerBeam"), firePoint.position, firePoint.rotation * rotationA);
            Rigidbody2D rb = lazerBeam.GetComponent<Rigidbody2D>();
            lastShotTime = Time.time;
            AudioManager.instance.Play("lazerGunGunshot");
        }
    }
}
