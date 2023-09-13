using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectController : MonoBehaviour
{
    public GameObject fireParticlePrefab;
    private Coroutine flameCoroutine;
    public GameObject slimePuddlePrefab;
    private Coroutine makeSlimePuddles;
    private Vector2 lastPosition;
    public bool isMushroom;
    public bool isOnFire;
    
    private void Start()
    {
        lastPosition = transform.position;
        if (isMushroom)
        {
            Debug.Log("I am a mushroom!");
            ApplySlimePuddlesStatusEffect();
        }
    }
    
    // Call this method to apply a fire status effect
    public void ApplyFireStatusEffect(int flameDamage, int flameDuration)
    {
        if (!isOnFire)
        {
            // Instantiate the fire particle effect at a fixed local position
            GameObject fireParticles = Instantiate(fireParticlePrefab, transform.position, Quaternion.identity);

            // Attach the fire particle effect to the current game object as a child
            fireParticles.transform.SetParent(transform);
            isOnFire = true;
        }

        if (flameCoroutine != null)
        {
            StopCoroutine(flameCoroutine); // Stop previous coroutine if it's still running
        }
        flameCoroutine = StartCoroutine(ApplyFlameDamage(flameDamage, flameDuration));

    }
    
    private IEnumerator ApplyFlameDamage(int damage, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            gameObject.GetComponent<EnemyStats>().TakeDamage(damage);
            yield return new WaitForSeconds(1f); // Wait for 1 second
            elapsedTime += 1f;
        }

        isOnFire = false;
        // Coroutine finished, reset reference
        flameCoroutine = null;
    }

    private void ApplySlimePuddlesStatusEffect()
    {
        if (makeSlimePuddles != null)
        {
            StopCoroutine(makeSlimePuddles);
        }
        makeSlimePuddles = StartCoroutine(MakeSlimePuddles());
    }
    
    private IEnumerator MakeSlimePuddles()
    {
        while (true)
        {
            float distanceMoved = Vector2.Distance(transform.position, lastPosition);
            if (distanceMoved >= 1.0f)
            {
                Debug.Log("Made a Slime Puddle.");
                // Instantiate a slime puddle prefab at the current enemy position
                GameObject slimePuddle = Instantiate(slimePuddlePrefab, transform.position, Quaternion.identity);
                
                // Destroy the slime puddle after 10 seconds
                Destroy(slimePuddle, 10.0f);

                // Update the last position
                lastPosition = transform.position;
            }

            yield return null; // Wait for the next frame
        }
    }
}
