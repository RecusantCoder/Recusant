using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QimmiqAttack : MonoBehaviour
{
    public int qimmiqDamage = 0;
    private Rigidbody2D rb;
    public Transform targetTransform;
    private CircleCollider2D CC2D;
    private float moveSpeedMemory = 0;
    public float moveSpeed = 2f;
    private float searchRadius = 4f; 
    private float knockBack = 0.1f;
    private float knockBackDuration = 0.25f;
    public bool isFlipped = false;
    private bool isTouchingEnemy = false;
    private float damageTimer = 0f;
    private float damageInterval = 0.5f;
    private GameObject currentEnemy = null; // Store the current enemy.
    public int qimmiqNumber;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();
        qimmiqDamage = (int)(qimmiqDamage * ((float)PlayerManager.instance.player.GetComponent<PlayerStats>().damage.GetValue() / 10 + 1));
        Debug.Log("qimmmiqAttackDamage: " + qimmiqDamage);
        
        rb = this.GetComponent<Rigidbody2D>();

        CC2D = gameObject.GetComponent<CircleCollider2D>();
        moveSpeedMemory = moveSpeed;
    }

    private void Update()
    {

        try
        {
            FaceTarget2D();
        }
        catch (Exception e)
        {
        }

        if (isTouchingEnemy && currentEnemy != null)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                if (currentEnemy != null)
                {
                    currentEnemy.GetComponent<EnemyStats>().TakeDamage(qimmiqDamage);
                    if (currentEnemy != null)
                    {
                        // Calculate the hit direction based on the QimmiqAttack's position and enemy's position
                        Vector2 hitDirection = currentEnemy.transform.position - transform.position;
                        hitDirection.Normalize();
                        // Apply knockback to the enemy
                        currentEnemy.GetComponent<EnemyController>()
                            .ApplyKnockback(hitDirection, knockBack, knockBackDuration);
                    }
                }

                damageTimer = 0f; // Reset the timer
            }
        }
    }

    private void FixedUpdate()
    {
        
        try
        {
            targetTransform = FindNearestEnemy(qimmiqNumber*3).transform;
        }
        catch (Exception e)
        {
            //Debug.Log("no enemy found");
        }
        if (targetTransform != null)
        {
            Vector2 direction = targetTransform.position - gameObject.transform.position;
            // Normalize the direction vector to have a length of 1
            direction.Normalize();
            rb.MovePosition((Vector2)transform.position + (direction * (moveSpeed * Time.deltaTime)));
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Enemy")
        {
            isTouchingEnemy = true;
            damageTimer = 0f;
            currentEnemy = other.gameObject; // Store the current enemy.
            currentEnemy.GetComponent<EnemyStats>().TakeDamage(qimmiqDamage);
        }

        if (other.transform.CompareTag("Breakable"))
        {
            other.gameObject.GetComponent<Breakable>().Damaged();
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "Enemy")
        {
            isTouchingEnemy = false;
            currentEnemy = null; // Clear the current enemy when it exits.
        }
    }
    
    void IgnoreBulletCollisions()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("111");
        Collider2D projectileCollider = GetComponent<Collider2D>();
        foreach (GameObject obj in objectsWithTag)
        {
            Collider2D collider = obj.GetComponent<Collider2D>();
            if (collider != null)
            {
                Physics2D.IgnoreCollision(projectileCollider, collider);
            }
        }
    }
    
    private GameObject FindNearestEnemy(int skipAmount)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;
        int skipCount = 0;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance( GameManager.instance.player.position, enemy.transform.position);
            if (distance <= searchRadius && distance < nearestDistance)
            {
                CircleCollider2D enemyCollider = enemy.GetComponent<CircleCollider2D>();
                if (enemyCollider != null)
                {
                    if (enemyCollider.isActiveAndEnabled)
                    {
                        if (skipAmount == skipCount)
                        {
                            nearestEnemy = enemy;
                            nearestDistance = distance;
                        }
                        else
                        {
                            skipCount++;
                        }
                    }
                }
            }
        }

        return nearestEnemy;
    }
    
    public void FaceTarget2D()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > targetTransform.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < targetTransform.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
}
