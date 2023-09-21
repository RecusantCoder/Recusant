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
    private CharacterStats targetStats;
    
    CharacterCombat combat;
    public float damageRadius = 1f;
    
    public bool isFlipped = false;
    public bool isKnockbackActive = false;

    private float despawnRadius = 25f;
    private CircleCollider2D CC2D;
    
    private Coroutine knockbackCoroutine; 
    private float moveSpeedMemory = 0;
    public bool isDead;
    
    private bool isTouchingPlayer = false;
    private float damageTimer = 0f;
    private float damageInterval = 1f;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        player = PlayerManager.instance.player.transform;
        targetStats = player.GetComponent<CharacterStats>();
        combat = GetComponent<CharacterCombat>();

        CC2D = gameObject.GetComponent<CircleCollider2D>();
        moveSpeedMemory = moveSpeed;
    }

    private void Update()
    {

        FaceTarget();
        FaceTarget2D();
        
        if (isTouchingPlayer)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                gameObject.GetComponent<EnemyStats>().TakeDamage(targetStats.spikeDamage.GetValue());
                combat.Attack(targetStats);
                SpecialActionOnCombat();
                damageTimer = 0f; // Reset the timer
            }
        }
        
        //If this leaves the Screen bounds, return to Pool
        if (IsOutOfPlayerRadius())
        {
            ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            isTouchingPlayer = true;
            damageTimer = 0f;
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            isTouchingPlayer = false;
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
        // freeze constraints to fully stop it
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(duration); // Adjust this delay based on your preference
        isKnockbackActive = false;
        HitFlash(false);
        // Restore the original velocity after knockback effect ends
        rb.velocity = originalVelocity;
        // unfreeze constraints
        rb.constraints = RigidbodyConstraints2D.None;
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

    public void SpecialActionOnCombat()
    {
        if (gameObject.name.Contains("Turtle"))
        {
            Debug.Log("Creating turtleExplosion");
            GameObject turtleExplosion = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/TurtleExplosion"), transform.position, transform.rotation);
            ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
        }
        
        if (gameObject.name.Contains("PlantSmall"))
        {
            Debug.Log("I am a small plant, i was hit!");
        }
    }

    public void SpecialActionOnDeath()
    {
        if (gameObject.name.Contains("PlantSmall"))
        {
            Debug.Log("Creating PlantMedium");
            GameManager.instance.SpawnEnemy(GameManager.instance.mediumPlant, gameObject.transform.position);
        } else if (gameObject.name.Contains("PlantMedium"))
        {
            Debug.Log("Creating PlantLarge");
            GameManager.instance.SpawnEnemy(GameManager.instance.largePlant, gameObject.transform.position);
        }
    }
    
    

}
