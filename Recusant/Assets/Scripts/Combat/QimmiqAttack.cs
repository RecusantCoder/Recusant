using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QimmiqAttack : MonoBehaviour
{
    private int qimmiqDamage = 50;
    private Rigidbody2D rb;
    public Transform targetTransform;
    CharacterCombat combat;
    private CircleCollider2D CC2D;
    private float moveSpeedMemory = 0;
    public float moveSpeed = 5f;
    private float searchRadius = 4f; 
    private float knockBack = 0.1f;
    private float knockBackDuration = 0.25f;
    public bool isFlipped = false;
    private bool isTouchingEnemy = false;
    private float damageTimer = 0f;
    private float damageInterval = 1f;
    private GameObject currentEnemy = null; // Store the current enemy.

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();
        qimmiqDamage += PlayerManager.instance.player.GetComponent<PlayerStats>().damage.GetValue();
        
        rb = this.GetComponent<Rigidbody2D>();

        combat = GetComponent<CharacterCombat>();

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
                currentEnemy.GetComponent<EnemyStats>().TakeDamage(qimmiqDamage);
                // Calculate the hit direction based on the QimmiqAttack's position and enemy's position
                Vector2 hitDirection = currentEnemy.transform.position - transform.position;
                hitDirection.Normalize();
                // Apply knockback to the enemy
                currentEnemy.GetComponent<EnemyController>().ApplyKnockback(hitDirection, knockBack, knockBackDuration);

                damageTimer = 0f; // Reset the timer
            }
        }
    }

    private void FixedUpdate()
    {
        
        try
        {
            targetTransform = FindNearestEnemy().transform;
        }
        catch (Exception e)
        {
        }
        if (targetTransform != null)
        {
            Vector2 direction = targetTransform.position - gameObject.transform.position;
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
    
    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= searchRadius && distance < nearestDistance)
            {
                CircleCollider2D enemyCollider = enemy.GetComponent<CircleCollider2D>();
                if (enemyCollider != null)
                {
                    if (enemyCollider.isActiveAndEnabled)
                    {
                        nearestEnemy = enemy;
                        nearestDistance = distance;
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
