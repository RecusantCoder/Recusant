using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownGrenade : MonoBehaviour
{
    public int grenadeDamage;
    public int penetrations = 0;
    
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
    }

    void IgnoreBulletCollisions()
    {
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        foreach (Bullet bullet in bullets)
        {
            if (bullet != null && bullet != this)
            {
                Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
                if (bulletCollider != null)
                {
                    Physics2D.IgnoreCollision(grenadeCollider, bulletCollider);
                }
            }
        }
    
        Grenade[] grenades = FindObjectsOfType<Grenade>();
        foreach (Grenade grenade in grenades)
        {
            if (grenade != null && grenade != this)
            {
                Collider2D grenadeCollider = grenade.GetComponent<Collider2D>();
                if (grenadeCollider != null)
                {
                    Physics2D.IgnoreCollision(grenadeCollider, grenadeCollider);
                }
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
