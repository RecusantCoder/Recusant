using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet : Interactable
{
    public string thisName;
    private GameObject _player;
    private bool isPickedUp = false;
    

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Interact()
    {
        base.Interact();
        if (!isPickedUp)
        {
            isPickedUp = true;
            PickUp();
        }
    }

    void PickUp()
    {
        AchievementManager.Instance.PowerUpFound("Amulet");
        StartCoroutine(MoveToPlayerAndDestroy());
    }
    
    IEnumerator MoveToPlayerAndDestroy()
    {
        Transform playerTransform = _player.transform;
        float speed = 2f; // Adjust the speed at which the object moves towards the player
        
        while (Vector3.Distance(transform.position, playerTransform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
            yield return null;
        }

        AudioManager.instance.Play("pickup");

        StartCoroutine(KillAndTarget());
    }
    
    private IEnumerator KillAndTarget()
    {
        KillAllEnemies();
        yield return new WaitForSeconds(1.0f);
        TargetAllOrbs();
        Destroy(gameObject);
    }

    public void TargetAllOrbs()
    {
        GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("Orb");

        foreach (GameObject obj in gameObjectsWithTag)
        {
            Orb orbScript = obj.GetComponent<Orb>();
            if (orbScript != null)
            {
                orbScript.Interact();
            }
            else
            {
                Debug.LogError("Orb script not found on object: " + obj.name);
            }
        }
    }

    public void KillAllEnemies()
    {
        GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject obj in gameObjectsWithTag)
        {
            EnemyStats enemyStatsScript = obj.GetComponent<EnemyStats>();
            if (enemyStatsScript != null)
            {
                enemyStatsScript.Die();
            }
            else
            {
                Debug.LogError("EnemyStats script not found on object: " + obj.name);
            }
        }
    }
}
