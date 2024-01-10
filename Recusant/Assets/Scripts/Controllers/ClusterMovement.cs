using System;
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
        Destroy(gameObject, 20f);
    }

    public void AssignChildrenClusterDestinations()
    {
        foreach (Transform child in transform)
        {
            GameObject newChild = GameManager.instance.SpawnEnemyWithReturn(GameManager.instance.spirit, child.transform.position);
            Destroy(child.gameObject);
            
            EnemyController enemyController = newChild.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.inACluster = true;
                enemyController.clusterDestination = clusterDestination;
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

    private void OnDestroy()
    {
        Debug.Log("Cluster destroyed at spawning: " + transform.position + " and player at position: " + GameManager.instance.player.transform.position);
    }
}
