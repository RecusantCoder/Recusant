using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoscoAttack : MonoBehaviour
{
    public Transform firePoint;
    private float searchRadius = 4.0f;
    private GameObject nearEnemy;
    private float bulletSpeed = 30f;
    private float offset = 1.0f;
    private GameObject player;
    
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();
        // Invoke PerformAttack method every second, starting after 1 second.
        InvokeRepeating("PerformAttack", 0.1f, 0.1f);
    }

    private void Update()
    {
        nearEnemy = FindNearestEnemy();
        Debug.Log(nearEnemy + " <-nearEnemy");
        
        Vector2 playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>().Movement;

        if (playerMovement is not { x: 0.000f, y: 0.000f })
        {
            // Normalize the movement to get the direction without the magnitude
            playerMovement.Normalize();

            // Update the follower's position based on player's movement
            transform.position = (Vector2)player.transform.position - offset * playerMovement;
        }
    }

    // Method to perform the attack and update EnemyController for each enemy.
    private void PerformAttack()
    {
        if (nearEnemy != null)
        {
            Vector2 direction = nearEnemy.transform.position - firePoint.position;
            firePoint.right = direction.normalized;

            GameObject bullet = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/Bullet2"), firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        
            //rb.AddForce(firePoint.up * bulletSpeed, ForceMode2D.Impulse);
            rb.velocity = firePoint.right * bulletSpeed;

            // Get the bullet script component and change its damage amount
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.bulletDamage = 100; // Change the damage amount
            bulletScript.penetrations = 100;
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


}
