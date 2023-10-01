using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public int damage = 0;
    private float knockBack = 0.1f;
    private float knockBackDuration = 0.25f;

    public Transform firePointLocal;
    public float horizontal;
    private PlayerMovement _playerMovement;


    private void Start()
    {
        _playerMovement = PlayerManager.instance.player.GetComponent<PlayerMovement>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();
        
        //Adding damage modifier
        damage = (int)(damage * ((float)PlayerManager.instance.player.GetComponent<PlayerStats>().damage.GetValue() / 10 + 1));

        StartCoroutine(RotateObject());
        
        Destroy(gameObject, 0.5f);
    }

    private IEnumerator RotateObject()
    {
        Debug.Log("horizontal " + _playerMovement.lastNonZeroInput);
        Quaternion startRotation = Quaternion.Euler(0f, 0f, 90f);
        Quaternion endRotation = Quaternion.Euler(0f, 0f, -90f);
        
        if (_playerMovement.lastNonZeroInput < 0.00f)
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
        Debug.Log("Ran IgnoreBulletCollisions");
    }


}
