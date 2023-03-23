using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 3f;
    
    private void OnDrawGizmosSelected()
    {
        if (lookRadius == null)
            return;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
