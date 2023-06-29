using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownGrenade : MonoBehaviour
{
    public int grenadeDamage;
    private int penetrations = 100;
    
    private Collider2D grenadeCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        
        grenadeCollider = GetComponent<Collider2D>();
        IgnoreBulletCollisions();
        
        //Adding damage modifier
        grenadeDamage += PlayerManager.instance.player.GetComponent<PlayerStats>().damage.GetValue();
        
        Destroy(gameObject, 1f);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Enemy")
        {
            //other.gameObject.GetComponent<EnemyStats>().TakeDamage(bulletDamage);
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
    }

    void IgnoreBulletCollisions()
    {
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        foreach (Bullet bullet in bullets)
        {
            if (bullet != this)
            {
                Physics2D.IgnoreCollision(grenadeCollider, bullet.GetComponent<Collider2D>());
            }
        }
        
        Grenade[] grenades = FindObjectsOfType<Grenade>();
        foreach (Grenade grenade in grenades)
        {
            if (grenade != this)
            {
                Physics2D.IgnoreCollision(grenadeCollider, grenade.GetComponent<Collider2D>());
            }
        }
    }
}
