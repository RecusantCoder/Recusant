using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeStormAttack : MonoBehaviour
{
    private int attackDamage = 100;
    private float knockBack = 0.1f;
    private float knockBackDuration = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();
        
        //Adding damage modifier
        //Debug.Log("Bullet damage before changes " + bulletDamage);
        float damageToPercent = (float)PlayerManager.instance.player.GetComponent<PlayerStats>().damage.GetValue() / 10 + 1;
        //Debug.Log("damageToPercent: " + damageToPercent);
        damageToPercent = attackDamage * damageToPercent;
        //Debug.Log("damgeToPrct with bullet damage " + damageToPercent);
        attackDamage = (int)damageToPercent;
        //Debug.Log("bullet damage final: " + bulletDamage);
        
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    { 
        //Debug.Log("Collided with " + other.name);
        if (other.transform.tag == "Enemy")
        {
            EnemyStats enemyStats = other.gameObject.GetComponent<EnemyStats>();
            enemyStats.TakeDamage(attackDamage);
            
            // Calculate the hit direction based on the bullet's position and enemy's position
            Vector2 hitDirection = other.transform.position - transform.position;
            hitDirection.Normalize();

            // Apply knockback to the enemy
            other.gameObject.GetComponent<EnemyController>().ApplyKnockback(hitDirection, knockBack, knockBackDuration);
        }
        if (other.transform.tag == "Breakable")
        {
            other.gameObject.GetComponent<Breakable>().Damaged();
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
        PlayerManager.instance.player.GetComponent<Shooting>().bladeStormComponent.ProjectileDestroyed();
    }
}
