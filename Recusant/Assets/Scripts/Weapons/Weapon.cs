using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public virtual void Shoot(Transform firePoint)
    {
        Debug.Log("Shooting a weapon");
    }
    
}