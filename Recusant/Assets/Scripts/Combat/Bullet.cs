using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int bulletDamage = 100;
    
    private Collider2D bulletCollider;
    

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        
        bulletCollider = GetComponent<Collider2D>();
        IgnoreBulletCollisions();
    }

    private void OnTriggerEnter2D(Collider2D other)
    { 
        //Debug.Log("Collided with " + other.name);
        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyStats>().TakeDamage(bulletDamage);

        }

        Destroy(gameObject);
    }
    
    void IgnoreBulletCollisions()
    {
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        foreach (Bullet bullet in bullets)
        {
            if (bullet != this)
            {
                Physics2D.IgnoreCollision(bulletCollider, bullet.GetComponent<Collider2D>());
            }
        }
    }
}
