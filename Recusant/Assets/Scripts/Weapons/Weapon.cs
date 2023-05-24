using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

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
