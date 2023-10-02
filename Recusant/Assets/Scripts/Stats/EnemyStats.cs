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
    private EnemyController _enemyController;
    private float previousMoveSpeed;

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
        orbPrefabsPaths = new List<string>();
        orbPrefabsPaths.Add("PreFabs/Orbs/RedOrb");
        orbPrefabsPaths.Add("PreFabs/Orbs/GreenOrb");
        orbPrefabsPaths.Add("PreFabs/Orbs/BlueOrb");
        orbPrefabsPaths.Add("PreFabs/Orbs/PinkOrb");
        orbPrefabsPaths.Add("PreFabs/Orbs/OrangeOrb");
        
        
        currentHealth = maxHealth;
        alreadyDied = false;
        previousMoveSpeed = _enemyController.moveSpeed;
    }
    
    private void Start()
    {
        
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
                /*int index = 0;
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
                
                */
                
                GameObject orb = Instantiate(Resources.Load<GameObject>(orbPrefabsPaths[0]), transform.position, transform.rotation);
            }

            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            if (circleCollider != null)
            {
                circleCollider.enabled = false;
            }

            
            _enemyController.moveSpeed = 0;
            _enemyController.isDead = true;

            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        //Debug.Log("Calling Fade out");
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
        _enemyController.SpecialActionOnDeath();
        FindAndDestroyAttachedStatusEffects();
        ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
    }

    private void FindAndDestroyAttachedStatusEffects()
    {
        string childObjectName = "TinyFlames";
        
        // Use Transform.Find to search for the child by name.
        Transform childTransform = transform.Find(childObjectName);

        // Check if the childTransform is found.
        if (childTransform != null)
        {
            // Destroy the child object.
            Destroy(childTransform.gameObject);
        }
        else
        {
            //Debug.LogWarning("Child object not found: " + childObjectName);
        }
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
        
        _enemyController.moveSpeed = previousMoveSpeed;
        _enemyController.isDead = false;
    }

    


    private void Update()
    {
        //Debug.Log(gameObject.name + " health is " + currentHealth);
    }
}
