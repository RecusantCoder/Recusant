using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public GameObject orbPrefab;
    public float fadeDuration = 1.0f; // Duration of the fade in seconds
    private SpriteRenderer spriteRenderer;
    private int startingHealth;
    public bool alreadyDied;

    private void Start()
    {

        startingHealth = currentHealth;
        alreadyDied = false;
    }
    
    public override void Die()
    {
        if (alreadyDied == false)
        {
            alreadyDied = true;
            
            base.Die();
        
            KillCounter.instance.EnemyKilled();

            GameObject orb = Instantiate(orbPrefab, transform.position, transform.rotation);
            
            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            if (circleCollider != null)
            {
                circleCollider.enabled = false;
            }
        
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        Color startColor = spriteRenderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Fully transparent color

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            spriteRenderer.color = Color.Lerp(startColor, endColor, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = endColor; // Ensure the final color is set correctly
        ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
    }

    public void ReMade() //for being returned from the object pool
    {
        Transform visualsChild = gameObject.transform.Find("Wobble/Visuals");
        if (visualsChild != null)
        {
            spriteRenderer = visualsChild.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        currentHealth = startingHealth;
        alreadyDied = false;
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider != null)
        {
            circleCollider.enabled = true;
        }
    }


    private void Update()
    {
        //Debug.Log(gameObject.name + " health is " + currentHealth);
    }
}
