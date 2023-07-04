using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownGrenade : MonoBehaviour
{
    public int grenadeDamage;
    public int penetrations = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Grenade Thrown Started Beginning");
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();
        
        //Adding damage modifier
        grenadeDamage += PlayerManager.instance.player.GetComponent<PlayerStats>().damage.GetValue();
        
        Debug.Log("Grenade Thrown Started End");
        
        Destroy(gameObject, 1f);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyStats>().TakeDamage(grenadeDamage);
        }
        if (other.transform.tag == "Breakable")
        {
            other.gameObject.GetComponent<Breakable>().Damaged();
        }
        
        if (penetrations == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            penetrations--;
        }
        Debug.Log("Collided with " + other.name);
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
        Debug.Log("Destroyed ThrownGrenade");
        GameObject explosion = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/Explosion"), transform.position, transform.rotation);
        //Modify the values on the thrownGrenade
        Explosion explosionScript = explosion.GetComponent<Explosion>();
        explosionScript.explosionDamage = grenadeDamage;
    }
}
