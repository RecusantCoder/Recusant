using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public List<string> orbPrefabsPaths;
    public float fadeDuration = 1.0f; // Duration of the fade in seconds
    private SpriteRenderer spriteRenderer;
    private int startingHealth;
    public bool alreadyDied;

    private void Start()
    {
        orbPrefabsPaths = new List<string>();
        orbPrefabsPaths.Add("PreFabs/Orbs/RedOrb");
        orbPrefabsPaths.Add("PreFabs/Orbs/GreenOrb");
        orbPrefabsPaths.Add("PreFabs/Orbs/BlueOrb");
        orbPrefabsPaths.Add("PreFabs/Orbs/PinkOrb");
        orbPrefabsPaths.Add("PreFabs/Orbs/OrangeOrb");
        
        
        currentHealth = maxHealth;
        alreadyDied = false;
    }
    
    public override void Die()
    {
        if (alreadyDied == false)
        {
            alreadyDied = true;
            
            base.Die();
        
            KillCounter.instance.EnemyKilled();

            int randomNumber = Random.Range(1, 301);
            if (randomNumber > 200)
            {
                int index = 0;
                if (randomNumber > 250)
                {
                    index = 1;
                }

                if (randomNumber > 275)
                {
                    index = 2;
                }

                if (randomNumber > 285)
                {
                    index = 3;
                }

                if (randomNumber > 295)
                {
                    index = 4;
                }
                GameObject orb = Instantiate(Resources.Load<GameObject>(orbPrefabsPaths[index]), transform.position, transform.rotation);
            }

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

        currentHealth = maxHealth;
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
