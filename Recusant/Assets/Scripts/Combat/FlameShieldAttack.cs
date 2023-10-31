using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameShieldAttack : MonoBehaviour
{
    public float flameShieldSpeed;
    
    public int _damage = 0;
    private float knockBack = 0.1f;
    public int duration = 10;
    private float knockBackDuration = 0.25f;
    public int penetrations = 0;

    public Transform firePointLocal;
    public float horizontal;
    private PlayerMovement _playerMovement;
    
    public Vector3 startScale = new Vector3(1, 1, 1);
    public Vector3 targetScale = new Vector3(3, 3, 3);
    public float scaleDuration = 2.0f;
    private bool currentlyScalingObject = false;
    
    private void Start()
    {
        _playerMovement = PlayerManager.instance.player.GetComponent<PlayerMovement>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");     
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        IgnoreBulletCollisions();
        
        //Adding damage modifier
        _damage = (int)(_damage * ((float)PlayerManager.instance.player.GetComponent<PlayerStats>().damage.GetValue() / 10 + 1));
    }

    private void Update()
    {
        if (!currentlyScalingObject)
        {
            currentlyScalingObject = true;
            StartCoroutine(ScaleObject());
        }
    }

    private void FixedUpdate()
    {
        transform.position = _playerMovement.transform.position;
    }

    public int Damage
    {
        get => _damage;
        set => _damage = value;
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    { 
        //Debug.Log("Collided with " + other.name);
        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyStats>().TakeDamage(_damage);
            
            // Calculate the hit direction based on the bullet's position and enemy's position
            Vector2 hitDirection = other.transform.position - transform.position;
            hitDirection.Normalize();

            // Apply knockback to the enemy
            other.gameObject.GetComponent<EnemyController>().ApplyKnockback(hitDirection, knockBack, knockBackDuration);
            
            // Apply fire damage over time
            int enemyHealth10Percent = other.gameObject.GetComponent<EnemyStats>().maxHealth / 10;
            other.gameObject.GetComponent<StatusEffectController>().ApplyFireStatusEffect(enemyHealth10Percent, duration);

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
        Debug.Log("Ran IgnoreBulletCollisions");
    }
    
    IEnumerator ScaleObject()
    {
        float timeElapsed = 0f;
        Vector3 currentScale = startScale;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 0, 180); // Rotate by 180 degrees (half rotation)

        while (timeElapsed < scaleDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, timeElapsed / scaleDuration);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        // Reverse the scale back to the starting size
        timeElapsed = 0f;
        startRotation = transform.rotation;
        targetRotation = transform.rotation * Quaternion.Euler(0, 0, 180);

        while (timeElapsed < scaleDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, timeElapsed / scaleDuration);
            transform.localScale = Vector3.Lerp(targetScale, startScale, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        currentlyScalingObject = false;
    }
}
