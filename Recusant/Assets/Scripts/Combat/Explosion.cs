using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int explosionDamage = 0;
    private Animator animator;
    private bool hasPlayedOnce;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        hasPlayedOnce = false;
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        
        explosionDamage += PlayerManager.instance.player.GetComponent<PlayerStats>().damage.GetValue();
        
    }

    // Update is called once per frame
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
    }
}
