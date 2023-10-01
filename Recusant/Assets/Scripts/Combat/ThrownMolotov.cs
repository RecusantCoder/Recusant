using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownMolotov : MonoBehaviour
{
    public int grenadeDamage;
    public int penetrations = 0;
    public int effectRadius = 0;
    public float thrownMolotovDuration = 0;
    private int shotOffset = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();
        /*
        //Adding damage modifier
        grenadeDamage = (int)(grenadeDamage * ((float)PlayerManager.instance.player.GetComponent<PlayerStats>().damage.GetValue() / 10 + 1));
        */
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
        AudioManager.instance.Play("Flame1");
        Debug.Log("Destroyed ThrownMolotov");
        for (int i = 0; i < 36; i++)
        {
            Vector3 offsetFirepoint = gameObject.transform.eulerAngles;
            offsetFirepoint.z += shotOffset;
            gameObject.transform.eulerAngles = offsetFirepoint;
            
            GameObject flame = Instantiate(Resources.Load<GameObject>("PreFabs/Projectiles/Flame"), gameObject.transform.position, gameObject.transform.rotation);
            Rigidbody2D rb = flame.GetComponent<Rigidbody2D>();
        
            rb.AddForce(flame.transform.up * 1.0f, ForceMode2D.Impulse);

            // Get the bullet script component and change its damage amount
            Flame flameScript = flame.GetComponent<Flame>();
            flameScript.Damage = flameScript.Damage += grenadeDamage;
            flameScript.firePointLocal = gameObject.transform;
            
            shotOffset += shotOffset + 5;
        }
    }
}
