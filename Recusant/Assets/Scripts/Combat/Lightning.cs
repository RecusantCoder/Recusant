using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Lightning : MonoBehaviour
{
    public float lightningChanceToSave = 0.0f;
    private bool safe;
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();
        safe = IsSaved();
        Debug.Log(safe + " <-is safe, chance-> " + lightningChanceToSave);
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    { 
        //Debug.Log("Collided with " + other.name);
        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyStats>().Die();
        }
        if (other.transform.tag == "Breakable")
        {
            other.gameObject.GetComponent<Breakable>().Damaged();
        }
        if (other.transform.tag == "Orb")
        {
            if (!safe)
            {
                Destroy(other.gameObject);
            }
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

    bool IsSaved()
    {
        int randomNumber = Random.Range(1, 101);
        Debug.Log(randomNumber + " <-rng");
        if (randomNumber <= lightningChanceToSave)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
}
