using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterMovement : MonoBehaviour
{
    public Vector2 clusterDestination;
    
    void Start()
    {
        SetOppositeDestination(GameManager.instance.player);
        AssignChildrenClusterDestinations();
    }

    public void AssignChildrenClusterDestinations()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            EnemyController enemyController = child.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.inACluster = true;
                enemyController.clusterDestination = clusterDestination;
            }
        }
        
        foreach (Transform child in transform)
        {
            Debug.Log("Checking child: " + child.name);
            EnemyController enemyController = child.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.inACluster = true;
                enemyController.clusterDestination = clusterDestination;
                Debug.Log("assigned spirits!");
            }
        }
    }
    
    // Method to set clusterDestination opposite to the player's position
    public void SetOppositeDestination(Transform playerTransform)
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player transform is null. Make sure to assign the player's transform.");
            return;
        }

        Vector2 playerPosition = playerTransform.position;
        Vector2 currentPosition = transform.position;

        // Calculate the direction from the current position to the player
        Vector2 directionToPlayer = playerPosition - currentPosition;

        // Calculate the opposite point by adding this direction to the player's position
        clusterDestination = playerPosition + (directionToPlayer * 10);
    }
}