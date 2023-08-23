using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    private int _damage = 5;
    private float knockBack = 0.1f;
    public int duration = 3;

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
        _damage += PlayerManager.instance.player.GetComponent<PlayerStats>().damage.GetValue();

        Destroy(gameObject, 1f);
    }

    public int Damage
    {
        get => _damage;
        set => _damage = value;
    }




    private void OnTriggerEnter2D(Collider2D other)
    { 
        //Debug.Log("Collided with " + other.name);
        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyStats>().TakeDamage(_damage);
            
            // Calculate the hit direction based on the bullet's position and enemy's position
            Vector2 hitDirection = other.transform.position - transform.position;
            hitDirection.Normalize();

            // Apply knockback to the enemy
            other.gameObject.GetComponent<EnemyController>().ApplyKnockback(hitDirection, knockBack);
            other.gameObject.GetComponent<StatusEffectController>().ApplyFireStatusEffect(_damage, duration);

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

