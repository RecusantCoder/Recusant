using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected int localWeaponlevel;
    protected int numOfShots = 1;
    protected float shotFrequency = 3.0f;
    protected List<float> times = new List<float>();
    protected List<float> firedList = new List<float>();

    public virtual void Shoot(Transform firePoint, int weaponLevel)
    {
        Debug.Log("shooting");
    }
    
    protected virtual void FireShot(Transform firePoint, int weaponLevel)
    {
        Debug.Log("Leveling a weapon");
    }

    protected virtual void WeaponLevels(int weaponLevel)
    {
        Debug.Log("Weapon levels");
    }

    protected virtual void UpdateWeaponBehavior(int level)
    {
        
    }
    
}
