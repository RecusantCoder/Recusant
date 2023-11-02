using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniNukeAttack : MonoBehaviour
{
    public int miniNukeAttackDamage = 0;
    public int penetrations = 0;
    public int miniNukeAttackRadius;
    public Vector3 miniNukeSpawnPosition;
    public Vector3 miniNukeEndPosition;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();
        
        //Adding damage modifier
        miniNukeAttackDamage = (int)(miniNukeAttackDamage * ((float)PlayerManager.instance.player.GetComponent<PlayerStats>().damage.GetValue() / 10 + 1));
        //Destroy(gameObject, 0.5f);
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, miniNukeEndPosition);
        if (distance < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyStats>().TakeDamage(miniNukeAttackDamage);
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
        Debug.Log("MiniNuke destroyed at: " +miniNukeEndPosition + " and was spawned at: " + miniNukeSpawnPosition);
        GameObject explosion = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/MiniNukeExplosion"), transform.position, transform.rotation);
        //Modify the values on the mininukeexplosion
        MiniNukeExplosion miniNukeExplosionScript = explosion.GetComponent<MiniNukeExplosion>();
        miniNukeExplosionScript.explosionDamage += miniNukeAttackDamage;
        miniNukeExplosionScript.explosionRadius += miniNukeAttackRadius;
    }
    
    
}
