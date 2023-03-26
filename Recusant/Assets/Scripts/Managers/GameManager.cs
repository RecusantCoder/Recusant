using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    
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
        SpawnSphereOnEdgeRandomly2D();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
    private void SpawnSphereOnEdgeRandomly2D()
    {
        float radius = 5f;
        Vector3 randomPos = Random.insideUnitSphere * radius;
        randomPos += player.transform.position;
        randomPos.y = 0f;
    
        Vector3 direction = randomPos - player.transform.position;
        direction.Normalize();
    
        float dotProduct = Vector3.Dot(player.transform.forward, direction);
        float dotProductAngle = Mathf.Acos(dotProduct / player.transform.forward.magnitude * direction.magnitude);
    
        randomPos.x = Mathf.Cos(dotProductAngle) * radius + player.transform.position.x;
        randomPos.y = Mathf.Sin(dotProductAngle * (Random.value > 0.5f ? 1f : -1f)) * radius + player.transform.position.y;
        randomPos.z = player.transform.position.z;
    
        //GameObject go = Instantiate(_spherePrefab, randomPos, Quaternion.identity);
        GameObject go = Instantiate(Resources.Load("Prefabs/Enemy", typeof(GameObject)), randomPos, Quaternion.identity) as GameObject; 

        go.transform.position = randomPos;
    }

    
}
