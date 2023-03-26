using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform player;
    private Vector2 movement;
    public float moveSpeed = 5f;
    
    CharacterCombat combat;
    public float damageRadius = 1f;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        player = PlayerManager.instance.player.transform;
        
        combat = GetComponent<CharacterCombat>();

    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        //Debug.Log(distance);
        FaceTarget();

        if (distance <= damageRadius)
        {
            
            CharacterStats targetStats = player.GetComponent<CharacterStats>();
            if (targetStats != null)
            {
                combat.Attack(targetStats);
                
            }
        }
        
    }

    private void FixedUpdate()
    {
        moveCharacter(movement);
    }


    // Rotate to face the target
    public void FaceTarget ()
    {
        Vector3 direction = (player.position - transform.position);
         float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
         rb.rotation = angle;
         direction.Normalize();
         movement = direction;
    }

    void moveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * (moveSpeed * Time.deltaTime)));
    }
    
    private void OnDrawGizmosSelected()
    {
        
        
        Gizmos.DrawWireSphere(gameObject.transform.position, damageRadius);
    }
}
