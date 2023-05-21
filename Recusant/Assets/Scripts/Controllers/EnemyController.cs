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
    private Animator animator;
    
    CharacterCombat combat;
    public float damageRadius = 1f;
    
    public bool isFlipped = false;
    private bool isKnockbackActive = false;

    private float despawnRadius = 5f;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        player = PlayerManager.instance.player.transform;
        
        combat = GetComponent<CharacterCombat>();

    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        
        FaceTarget();
        FaceTarget2D();
        
        //Debug.Log("Distance: " + distance + " and Radius: " + damageRadius);

        if (distance <= damageRadius)
        {
            
            CharacterStats targetStats = player.GetComponent<CharacterStats>();
            if (targetStats != null)
            {
                combat.Attack(targetStats);
                
            }
        }
        
        //If this leaves the Screen bounds, return to Pool
        if (IsOutOfPlayerRadius())
        {
            ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
            Debug.Log("return to pool");
        }
        
    }

    private void FixedUpdate()
    {
        if (!isKnockbackActive)
            moveCharacter(movement);
    }


    // Rotate to face the target
    public void FaceTarget ()
    {
        Vector3 direction = (player.position - transform.position);
         float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
         //rb.rotation = angle;
         direction.Normalize();
         movement = direction;
    }

    public void FaceTarget2D()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    void moveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * (moveSpeed * Time.deltaTime)));
    }
    
    private void OnDrawGizmosSelected()
    {
        
        
        Gizmos.DrawWireSphere(gameObject.transform.position, damageRadius);
    }
    
    public void ApplyKnockback(Vector2 hitDirection, float knockbackForce)
    {
        // Disable movement during knockback
        isKnockbackActive = true;
        
        // Apply the knockback force to the enemy's rigidbody
        rb.AddForce(hitDirection * knockbackForce, ForceMode2D.Impulse);
        
        HitFlash(true);

        // Enable movement after knockback is complete
        StartCoroutine(EnableMovementAfterKnockback());
    }
    
    private IEnumerator EnableMovementAfterKnockback()
    {
        yield return new WaitForSeconds(0.1f); // Adjust this delay based on your preference
        isKnockbackActive = false;
        HitFlash(false);
    }

    private void HitFlash(bool wasHit)
    {
        Transform visualsChild = gameObject.transform.Find("Wobble/Visuals");
        if (visualsChild != null)
        {
            animator = visualsChild.GetComponent<Animator>();
            animator.SetBool("wasHit", wasHit);
        }
    }
    
    private bool IsOutOfPlayerRadius()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        // Debug the distance between the player and the enemy
        Debug.Log("Distance: " + distance);
        // Check if the distance exceeds the radius
        return distance > despawnRadius;
    }

    
    
    

}
