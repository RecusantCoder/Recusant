using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiationZone : MonoBehaviour
{
    private int radiationDamage = 1;
    public float radiationRadius = 1;
    public float radiationZoneLifetime = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(radiationRadius, radiationRadius, radiationRadius);

        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();
        Destroy(gameObject, radiationZoneLifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    { 
        //Debug.Log("Collided with " + other.name);
        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyStats>().TakeDamage(radiationDamage);
            
            // Apply fire damage over time
            int enemyHealth10Percent = other.gameObject.GetComponent<EnemyStats>().maxHealth / 3;
            other.gameObject.GetComponent<StatusEffectController>().ApplyFireStatusEffect(enemyHealth10Percent, 10);
            // Calculate the hit direction based on the bullet's position and enemy's position
            Vector2 hitDirection = other.transform.position - transform.position;
            hitDirection.Normalize();
            other.gameObject.GetComponent<EnemyController>().ApplyKnockback(hitDirection, 0.01f, 10);
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
}
