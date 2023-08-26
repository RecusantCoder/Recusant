using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashExplosion : MonoBehaviour
{
    public int explosionDamage = 0;
    private float knockBackDuration = 10f;
    public Animator animator;
    private bool hasPlayedOnce;
    public GameObject animatorObject;
    

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.Play("FlashExplosion");
        hasPlayedOnce = false;
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();
        explosionDamage += PlayerManager.instance.player.GetComponent<PlayerStats>().damage.GetValue();
        Destroy(gameObject, 3.0f);
    }
    
    void Update()
    {
        if (!hasPlayedOnce && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.25)
        {
            hasPlayedOnce = true;
        }

        if (hasPlayedOnce)
        {
            animatorObject.SetActive(false);
        }
    }

    
    
    private void OnTriggerEnter2D(Collider2D other)
    { 
        //Debug.Log("Collided with " + other.name);
        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyStats>().TakeDamage(explosionDamage);
            
            // Calculate the hit direction based on the bullet's position and enemy's position
            Vector2 hitDirection = other.transform.position - transform.position;
            hitDirection.Normalize();
            other.gameObject.GetComponent<EnemyController>().ApplyKnockback(hitDirection, 0.01f, knockBackDuration);
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
