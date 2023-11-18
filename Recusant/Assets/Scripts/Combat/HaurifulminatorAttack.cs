using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaurifulminatorAttack : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;
    public Transform killPoint;
    public float attractForce = 10f;
    private GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();
        // Invoke PerformAttack method every second, starting after 1 second.
        InvokeRepeating("PerformAttack", 0.1f, 0.1f);
    }

    private void Update()
    {
        transform.position = GameManager.instance.player.transform.position;
    }

    // Method to perform the attack and update EnemyController for each enemy.
    private void PerformAttack()
    {
        // Find all instances of EnemyController in the scene.
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObject in enemies)
        {
            EnemyController enemy = enemyObject.GetComponent<EnemyController>();
            enemy.isProcessed = true;
            // Check if the enemy is left or right of the killPoint.
            if (enemyObject.transform.position.x < killPoint.position.x)
            {
                // Set the processorLocation to leftPoint.
                enemy.processorLocation = leftPoint.position;
            }
            else
            {
                // Set the processorLocation to rightPoint.
                enemy.processorLocation = rightPoint.position;
            }
            
            // Check if the enemy has reached the left or right point.
            if (Vector2.Distance(enemyObject.transform.position, leftPoint.position) < 0.3f || Vector2.Distance(enemyObject.transform.position, rightPoint.position) < 0.3f)
            {
                enemy.passedCheckpoint = true;
            }

            if (enemy.passedCheckpoint)
            {
                enemy.processorLocation = killPoint.position;
            }
            
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    { 
        //Debug.Log("Collided with " + other.name);
        if (other.transform.tag == "Enemy")
        {
            EnemyStats enemyStats = other.gameObject.GetComponent<EnemyStats>();
            enemyStats.wasProcessed = true;
            enemyStats.TakeDamage(666);
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
    
    private void OnDestroy()
    {
        foreach (var enemy in enemies)
        {
            EnemyStats enemyStats = enemy.gameObject.GetComponent<EnemyStats>();
            enemyStats.wasProcessed = true;
            enemyStats.TakeDamage(666);
        }
        PlayerManager.instance.player.GetComponent<Shooting>().haurifulminatorComponent.ProjectileDestroyed();
    }
}
