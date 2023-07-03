using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public int damage = 1;
    private float knockBack = 0.1f;
    
    private Collider2D slashCollider;

    public Transform firePointLocal;
    public float horizontal;
    

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        
        slashCollider = GetComponent<Collider2D>();
        IgnoreBulletCollisions();
        
        //Adding damage modifier
        damage += PlayerManager.instance.player.GetComponent<PlayerStats>().damage.GetValue();
        
        StartCoroutine(RotateObject());
        
        Destroy(gameObject, 1f);
    }
    
    private IEnumerator RotateObject()
    {
        Quaternion startRotation = Quaternion.Euler(0f, 0f, 90f);
        Quaternion endRotation = Quaternion.Euler(0f, 0f, -90f);
        
        if (horizontal <= 0)
        {
            startRotation = Quaternion.Euler(0f, 180f, 90f);
            endRotation = Quaternion.Euler(0f, 180f, -90f);
        }

        float rotationDuration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            float t = elapsedTime / rotationDuration;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            transform.position = firePointLocal.position;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation; // Ensure the final rotation is set correctly
    }




    private void OnTriggerEnter2D(Collider2D other)
    { 
        //Debug.Log("Collided with " + other.name);
        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyStats>().TakeDamage(damage);
            
            // Calculate the hit direction based on the bullet's position and enemy's position
            Vector2 hitDirection = other.transform.position - transform.position;
            hitDirection.Normalize();

            // Apply knockback to the enemy
            other.gameObject.GetComponent<EnemyController>().ApplyKnockback(hitDirection, knockBack);

        }
        if (other.transform.tag == "Breakable")
        {
            other.gameObject.GetComponent<Breakable>().Damaged();
        }
    }
    
    void IgnoreBulletCollisions()
    {
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        foreach (Bullet bullet in bullets)
        {
            if (bullet != this)
            {
                Physics2D.IgnoreCollision(slashCollider, bullet.GetComponent<Collider2D>());
            }
        }
    }


}
