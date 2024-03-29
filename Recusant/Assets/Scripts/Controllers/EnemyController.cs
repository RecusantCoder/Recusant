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
    private EnemyStats thisEnemyStats;
    private BoidMovement _boidMovement;
    
    public float damageRadius = 1f;
    
    public bool isFlipped = false;
    public bool isKnockbackActive = false;

    private float despawnRadius = 6f;
    private CircleCollider2D CC2D;
    
    private Coroutine knockbackCoroutine; 
    private float moveSpeedMemory = 0;
    public bool isDead;
    
    public bool isTouchingPlayer = false;
    private float damageTimer = 0f;
    private float damageInterval = 0.1f;
    
    Transform myObjectTransform;

    public bool isProcessed;
    public Vector2 processorLocation;
    public bool passedCheckpoint;

    public bool inACluster;
    public Vector2 clusterDestination;

    private void Start()
    {
        myObjectTransform = gameObject.transform;
        rb = this.GetComponent<Rigidbody2D>();
        player = PlayerManager.instance.player.transform;
        targetStats = player.GetComponent<CharacterStats>();
        thisEnemyStats = gameObject.GetComponent<EnemyStats>();
        _boidMovement = gameObject.GetComponent<BoidMovement>();

        CC2D = gameObject.GetComponent<CircleCollider2D>();
        moveSpeedMemory = moveSpeed;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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
                thisEnemyStats.TakeDamage(targetStats.spikeDamage.GetValue());
                targetStats.TakeDamage(thisEnemyStats.damage.GetValue());
                SpecialActionOnCombat();
                damageTimer = 0f; // Reset the timer
            }
        }
        
        //If this leaves the Screen bounds, return to Pool
        if (IsOutOfPlayerRadius())
        {
            ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
        }

        //Trying to stop enemy from rotating on its z axis on Build version
        var rotation = myObjectTransform.rotation;
        rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, 0f);
        myObjectTransform.rotation = rotation;
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
            damageTimer = 0f;
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

    public void moveCharacter(Vector2 direction)
    {
        if (!isDead)
        {
            if (isProcessed)
            {
                // Calculate the new position using Vector2.MoveTowards
                Vector2 newPosition = Vector2.MoveTowards(transform.position, processorLocation, 2 * Time.deltaTime);

                // Move the rigidbody to the new position
                rb.MovePosition(newPosition);
            }
            else
            {
                if (gameObject.name.Contains("Boid1"))
                {
                    _boidMovement.goodToMove = true;
                }
                else
                {
                    if (_boidMovement != null)
                    {
                        _boidMovement.goodToMove = false;
                    }
                }

                if (inACluster)
                {
                    // Calculate the new position using Vector2.MoveTowards
                    Vector2 newPosition = Vector2.MoveTowards(transform.position, clusterDestination, 2 * Time.deltaTime);

                    // Move the rigidbody to the new position
                    rb.MovePosition(newPosition);
                }
                else
                {
                    rb.MovePosition((Vector2)transform.position + (direction * (moveSpeed * Time.deltaTime)));
                }
            }
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
            knockbackCoroutine = null;
        }


        // Apply the knockback force to the enemy's rigidbody
        rb.AddForce(hitDirection * knockbackForce, ForceMode2D.Impulse);
        
        HitFlash(true);
        
        if (gameObject.activeSelf)
        {
            // Enable movement after knockback is complete
            knockbackCoroutine = StartCoroutine(EnableMovementAfterKnockback(knockbackDuration));
        }
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
        // unfreeze constraints, but keep z constraint
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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
