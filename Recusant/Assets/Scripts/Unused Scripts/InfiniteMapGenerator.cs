using System.Collections.Generic;
using UnityEngine;

public class InfiniteMapGenerator : MonoBehaviour
{
    public GameObject prefab;
    public int initialCount = 10;
    public float offset = 10f;

    private Transform playerTransform;
    private float spawnZ = 0f;
    private float despawnZ = 0f;
    private Queue<GameObject> objectQueue = new Queue<GameObject>();

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < initialCount; i++)
        {
            SpawnObject();
        }
    }

    void Update()
    {
        if (playerTransform.position.z > (spawnZ - offset))
        {
            SpawnObject();
        }

        if (objectQueue.Count > 0 && playerTransform.position.z > (despawnZ + offset))
        {
            DestroyObject();
        }
    }

    private void SpawnObject()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.transform.position = Vector3.forward * spawnZ;
        spawnZ += obj.transform.localScale.z;
        objectQueue.Enqueue(obj);
    }

    private void DestroyObject()
    {
        GameObject obj = objectQueue.Dequeue();
        Destroy(obj);
        despawnZ += obj.transform.localScale.z;
    }
}

