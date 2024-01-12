using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public Transform target;
    void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
