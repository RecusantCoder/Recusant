using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public virtual void Shoot(Transform firePoint, int weaponLevel)
    {
        Debug.Log("Shooting a weapon");
    }
    
}
