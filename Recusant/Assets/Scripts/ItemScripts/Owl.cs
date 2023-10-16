using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owl : Interactable
{
    public string thisName;
    private GameObject _player;
    private bool isPickedUp = false;
    public Transform visual;
    public bool shouldRotate = true;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(RotateVisualContinuously());
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

        StartCoroutine(FreezeForXSeconds(12));
    }
    
    private IEnumerator RotateVisualContinuously()
    {
        float rotationAmount = -30.0f;
        float rotationDuration = 1.0f; // 1 second

        while (shouldRotate)
        {
            Quaternion targetRotation = visual.rotation * Quaternion.Euler(0, 0, rotationAmount);
            float startTime = Time.time;

            while (Time.time - startTime < rotationDuration)
            {
                visual.rotation = Quaternion.Slerp(visual.rotation, targetRotation, (Time.time - startTime) / rotationDuration);
                yield return null;
            }

            yield return null; // Optionally, you can insert a small delay between rotations
        }

        if (!shouldRotate)
        {
            visual.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private IEnumerator FreezeForXSeconds(int seconds)
    {
        shouldRotate = false;
        int count = seconds;

        while (count > 1)
        {
            count--;
            FreezeAllEnemies();
            yield return new WaitForSeconds(1.0f);
        }
        Destroy(gameObject);
    }

    public void FreezeAllEnemies()
    {
        GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject obj in gameObjectsWithTag)
        {
            EnemyController enemyControllerScript = obj.GetComponent<EnemyController>();
            if (enemyControllerScript != null)
            {
                // Calculate the hit direction based on the bullet's position and enemy's position
                Vector2 hitDirection = obj.transform.position - _player.transform.position;
                hitDirection.Normalize();
                obj.gameObject.GetComponent<EnemyController>().ApplyKnockback(hitDirection, 0.01f, 1.0f);
            }
            else
            {
                Debug.LogError("EnemyController script not found on object: " + obj.name);
            }
        }
    }
}
