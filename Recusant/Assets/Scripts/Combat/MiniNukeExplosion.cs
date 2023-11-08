using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniNukeExplosion : MonoBehaviour
{
    public int explosionDamage = 0;
    private Animator animator;
    private bool hasPlayedOnce;
    public int explosionRadius = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(explosionRadius, explosionRadius, explosionRadius);
        AudioManager.instance.Play("Explosion");
        
        animator = GetComponent<Animator>();
        hasPlayedOnce = false;
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();

    }
    
    void Update()
    {
        if (!hasPlayedOnce && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            hasPlayedOnce = true;
        }

        if (hasPlayedOnce)
        {
            Destroy(gameObject);
        }
    }

    
    
    private void OnTriggerEnter2D(Collider2D other)
    { 
        //Debug.Log("Collided with " + other.name);
        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyStats>().TakeDamage(explosionDamage);
            
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
        GameObject radiationZone = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/RadiationZone"), transform.position, transform.rotation);
        RadiationZone radiationZoneScript = radiationZone.GetComponent<RadiationZone>();
    }
}
