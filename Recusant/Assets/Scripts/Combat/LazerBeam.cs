using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBeam : MonoBehaviour
{
    public int lazerDamage = 1000;
    
    //Timing
    private float elapsed = 0f;
    private float creationTime = 0.1f;
    private float timePassed = 0f;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();
        lazerDamage += PlayerManager.instance.player.GetComponent<PlayerStats>().damage.GetValue();
    }

    private void Update()
    {
        //was using for action every second
        elapsed += Time.deltaTime;
        if (elapsed >= 0.1f)
        {
            elapsed = elapsed % 1f;
            timePassed = timePassed + 0.1f;
            if (timePassed >= creationTime)
            {
                Destroy(gameObject);
                
                timePassed = 0f;
            }
        }
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    { 
        //Debug.Log("Collided with " + other.name);
        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyStats>().TakeDamage(lazerDamage);

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
