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
    private bool hasPlayedOnce;
    
    CharacterCombat combat;
    public float damageRadius = 1f;
    
    public bool isFlipped = false;
    public bool isKnockbackActive = false;

    private float despawnRadius = 25f;
    private CircleCollider2D CC2D;
    
    private Coroutine knockbackCoroutine; 
    private float moveSpeedMemory = 0;
    public bool isDead;
    public bool isTurtle;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        player = PlayerManager.instance.player.transform;
        
        combat = GetComponent<CharacterCombat>();

        CC2D = gameObject.GetComponent<CircleCollider2D>();
        moveSpeedMemory = moveSpeed;
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
                if (CC2D != null && CC2D.isActiveAndEnabled)
                {
                    combat.Attack(targetStats);
                    if (isTurtle)
                    {
                        Debug.Log("Creating turtleExplosion");
                        GameObject turtleExplosion = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/TurtleExplosion"), transform.position, transform.rotation);
                        isTurtle = false;
                        Destroy(gameObject);
                    }
                }
            }
        }
        
        //If this leaves the Screen bounds, return to Pool
        if (IsOutOfPlayerRadius())
        {
            ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
            //Debug.Log("return to pool");
        }
        
    }

    private void FixedUpdate()
    {
        if (!isKnockbackActive)
        {
            moveSpeed = moveSpeedMemory;
            moveCharacter(movement);
        }
        else
        {
            moveSpeed = 0;
        }
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
        if (!isDead)
        {
            rb.MovePosition((Vector2)transform.position + (direction * (moveSpeed * Time.deltaTime)));
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, damageRadius);
    }
    
    public void ApplyKnockback(Vector2 hitDirection, float knockbackForce, float knockbackDuration)
    {
        Debug.Log("ApplyKnockback");
        
        // Disable movement during knockback
        isKnockbackActive = true;
        
        // If a knockback coroutine is already running, stop it
        if (knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
        }


        // Apply the knockback force to the enemy's rigidbody
        rb.AddForce(hitDirection * knockbackForce, ForceMode2D.Impulse);
        
        HitFlash(true);

        // Enable movement after knockback is complete
        StartCoroutine(EnableMovementAfterKnockback(knockbackDuration));
    }
    
    private IEnumerator EnableMovementAfterKnockback(float duration)
    {
        Debug.Log("EnableMovementAfterKnockback");
        // Store the original velocity
        Vector2 originalVelocity = rb.velocity;
        // Set the velocity to zero to stop any ongoing movement
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(duration); // Adjust this delay based on your preference
        isKnockbackActive = false;
        HitFlash(false);
        // Restore the original velocity after knockback effect ends
        rb.velocity = originalVelocity;
        knockbackCoroutine = null; // Reset the knockbackCoroutine reference
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
        return distance > despawnRadius;
    }

    
    
    

}
