using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fulmen : Weapon
{
    private float lastShotTime;

    public override void Shoot(Transform firePoint)
    {
        if (Time.time - lastShotTime > 2.0f)
        {
            Debug.Log("Fired New Fulmen!");
            GameObject lightning = Instantiate(Resources.Load<GameObject>("Prefabs/Lightning"), firePoint.position, firePoint.rotation);
            lastShotTime = Time.time;
            AudioManager.instance.Play("lightning");
        }
    }
}
