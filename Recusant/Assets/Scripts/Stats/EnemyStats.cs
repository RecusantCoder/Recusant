using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyStats : CharacterStats
{
    public List<string> orbPrefabsPaths;
    public float fadeDuration = 1.0f; // Duration of the fade in seconds
    private SpriteRenderer spriteRenderer;
    private int startingHealth;
    public bool alreadyDied;
    private EnemyController _enemyController;
    private float previousMoveSpeed;
    public bool wasProcessed;
    public bool isBoss;

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
        if (_enemyController != null)
        {
            previousMoveSpeed = _enemyController.moveSpeed;
        }
    }
    
    private void Start()
    {
        wasProcessed = false;
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
                if (wasProcessed)
                {
                    Debug.Log("Orb made with processed");
                    orb.GetComponent<Orb>().movingToPlayer = true;
                }
            }

            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            if (circleCollider != null)
            {
                circleCollider.enabled = false;
            }

            
            _enemyController.moveSpeed = 0;
            _enemyController.isDead = true;

            StartCoroutine(FadeOut());

            if (isBoss)
            {
                GameObject steelContainer = Instantiate(Resources.Load<GameObject>("PreFabs/PowerUps/SteelContainer"),
                    transform.position, transform.rotation);
                isBoss = false;
            }
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

        _enemyController.isKnockbackActive = false;
        _enemyController.moveSpeed = previousMoveSpeed;
        _enemyController.isDead = false;
        wasProcessed = false;
        _enemyController.isProcessed = false;
        _enemyController.passedCheckpoint = false;
    }
    
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        AudioManager.instance.Play("Impact1");
    }

    private void OnEnable()
    {
        if (isBoss)
        {
            Debug.Log("I am a boss!");
            currentHealth = 50 * LevelBar.instance.playerLevel;
            gameObject.transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }


    private void Update()
    {
        //Debug.Log(gameObject.name + " health is " + currentHealth);
    }
}
