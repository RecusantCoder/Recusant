using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleExplosion : MonoBehaviour
{
    public int explosionDamage = 0;
    private float knockBackDuration = 2f;
    public Animator animator;
    private bool hasPlayedOnce;
    public GameObject animatorObject;
    

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.Play("FlashExplosion");
        hasPlayedOnce = false;
        //GameObject player = GameObject.FindGameObjectWithTag("Player");     
        //Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();
        //explosionDamage += PlayerManager.instance.player.GetComponent<PlayerStats>().damage.GetValue();
        Destroy(gameObject, 1.0f);
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
        if (other.transform.tag == "Player")
        {
            Debug.Log("Turtle Explode!!!");
            other.gameObject.GetComponent<PlayerStats>().TakeDamage(explosionDamage);
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
