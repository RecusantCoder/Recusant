using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 3f;
    private Transform target;

    private void Start()
    {
        target = PlayerManager.instance.player.transform;
        
    }

    private void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (lookRadius == null)
            return;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
