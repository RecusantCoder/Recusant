using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected int localWeaponlevel;

    public virtual void Shoot(Transform firePoint, int weaponLevel)
    {
        Debug.Log("Shooting a weapon");
    }

    protected virtual void WeaponLevels(int weaponLevel)
    {
        Debug.Log("Leveling a weapon");
    }
    
}
