using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwoleSamoyedAttack : MonoBehaviour
{
    public int swoleSamoyedDamage = 0;
    private Rigidbody2D rb;
    public Transform targetTransform;
    private CircleCollider2D CC2D;
    private float moveSpeedMemory = 0;
    public float moveSpeed = 360f;
    private float searchRadius = 1f; 
    private float knockBack = 10.0f;
    private float knockBackDuration = 5.0f;
    private bool isTouchingEnemy = false;
    private float damageTimer = 0f;
    private float damageInterval = 0.5f;
    private GameObject currentEnemy = null; // Store the current enemy.
    private GameObject player;
    private Vector3 newPosition;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();
        swoleSamoyedDamage = (int)(swoleSamoyedDamage * ((float)PlayerManager.instance.player.GetComponent<PlayerStats>().damage.GetValue() / 10 + 1));

        rb = this.GetComponent<Rigidbody2D>();

        CC2D = gameObject.GetComponent<CircleCollider2D>();
        moveSpeedMemory = moveSpeed;
        
        StartCoroutine(OrbitPlayer());

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
                    currentEnemy.GetComponent<EnemyStats>().TakeDamage(swoleSamoyedDamage);
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
        transform.position = newPosition;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Enemy")
        {
            isTouchingEnemy = true;
            damageTimer = 0f;
            currentEnemy = other.gameObject; // Store the current enemy.
            currentEnemy.GetComponent<EnemyStats>().TakeDamage(swoleSamoyedDamage);
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
    
    private IEnumerator OrbitPlayer()
    {
        float angle = 0f;

        while (true)
        {
            newPosition = player.transform.position + Quaternion.Euler(0, 0, angle) * new Vector3(searchRadius, 0, 0);
            angle += moveSpeed * Time.deltaTime;
            yield return null;
        }
    }
    
    public void FaceTarget2D()
    {
        transform.rotation = transform.position.y > player.transform.position.y ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity;
    }
}
