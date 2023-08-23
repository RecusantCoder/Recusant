using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectController : MonoBehaviour
{
    public GameObject fireParticlePrefab;
    private Coroutine flameCoroutine;
    
    // Call this method to apply a fire status effect
    public void ApplyFireStatusEffect(int flameDamage, int flameDuration)
    {
        // Instantiate the fire particle effect at a fixed local position
        GameObject fireParticles = Instantiate(fireParticlePrefab, transform.position, Quaternion.identity);

        // Attach the fire particle effect to the current game object as a child
        fireParticles.transform.SetParent(transform);
        
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

        // Coroutine finished, reset reference
        flameCoroutine = null;
    }
}
