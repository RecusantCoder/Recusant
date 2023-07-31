using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    public List<GameObject> prefabs;
    public int poolSize = 100;

    private Dictionary<GameObject, List<GameObject>> objectPools;

    private void Awake()
    {
        Instance = this;
        InitializePools();
    }

    private void InitializePools()
    {
        objectPools = new Dictionary<GameObject, List<GameObject>>();

        foreach (GameObject prefab in prefabs)
        {
            List<GameObject> objectPool = new List<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                objectPool.Add(obj);
            }

            objectPools.Add(prefab, objectPool);
        }
    }

    public GameObject GetObjectFromPool(GameObject prefab)
    {
        if (objectPools.ContainsKey(prefab))
        {
            List<GameObject> objectPool = objectPools[prefab];

            for (int i = 0; i < objectPool.Count; i++)
            {
                if (!objectPool[i].activeInHierarchy)
                {
                    return objectPool[i];
                }
            }
        }

        // If the pool is empty or the prefab is not found in the dictionary, you can handle it based on your specific requirements.
        return null;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        // Optionally, reset any relevant properties of the object here.

        // Determine the prefab of the object and return it to the corresponding pool.
        foreach (var pool in objectPools)
        {
            if (pool.Value.Contains(obj))
            {
                obj.transform.SetParent(transform); // Set the object's parent to the ObjectPoolManager for organization.
                return;
            }
        }
    }

}