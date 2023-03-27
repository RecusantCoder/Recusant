using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private float elapsed = 0f;
    public float creationTime;
    private float timePassed = 0f;
    public float spawnRadius = 5f;
    
    /*For finding components
     *
     * Debug.Log("List of components");
        GAMEOBJECTNAME = GameObject.Find("PlayerHealth");
        
        Component[] components = GAMEOBJECTNAME.GetComponents(typeof(Component));
        foreach(Component component in components) 
        { 
            Debug.Log(component.ToString());
        }
        Debug.Log("Finished list of components");
     */

    public Transform player;


    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.player.transform;

        //randomCreationTime = Random.Range(6.0f, 60.0f);
        creationTime = 10f;

    }

    // Update is called once per frame
    void Update()
    {
        //was using for action every second
        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            elapsed = elapsed % 1f;
            timePassed++;
            if (timePassed >= creationTime)
            {
                for (int i = 0; i < 50; i++)
                {
                    SpawnEnemy("Prefabs/TestingWobble");
                }
                
                
                timePassed = 0f;
                creationTime = 10f;
            }
        }
    }

    
    
    private void SpawnEnemy(String enemyFilePath)
    {
        //float radius = 5f;
        Vector3 randomPos = Random.insideUnitSphere * spawnRadius;
        randomPos += player.transform.position;
        randomPos.y = 0f;
    
        Vector3 direction = randomPos - player.transform.position;
        direction.Normalize();
    
        float dotProduct = Vector3.Dot(player.transform.forward, direction);
        float dotProductAngle = Mathf.Acos(dotProduct / player.transform.forward.magnitude * direction.magnitude);
    
        randomPos.x = Mathf.Cos(dotProductAngle) * spawnRadius + player.transform.position.x;
        randomPos.y = Mathf.Sin(dotProductAngle * (Random.value > 0.5f ? 1f : -1f)) * spawnRadius + player.transform.position.y;
        randomPos.z = player.transform.position.z;
    
        //GameObject go = Instantiate(_spherePrefab, randomPos, Quaternion.identity);
        GameObject go = Instantiate(Resources.Load(enemyFilePath, typeof(GameObject)), randomPos, Quaternion.identity) as GameObject; 

        go.transform.position = randomPos;
    }

    
}
