using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    
    #region Singleton
    
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of GameManager found!");
            return;
        }
        instance = this;
    }
    
    #endregion
    
    public float spawnRadius = 5f;
    public int amountToSpawn = 1;
    
    //For timer
    private float timer = 0.0f;
    [SerializeField]
    public TMP_Text myTextElement;
    private int lastSecond = 0;

    private int creations = 0;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        //Timer
        timer += Time.deltaTime;
        // Check if a second has passed
        int currentSecond = Mathf.FloorToInt(timer);
        if (currentSecond != lastSecond)
        {
            lastSecond = currentSecond;
            enemySpawnInfo();
        }
        
        
        myTextElement.text = timeConvert(timer);
        /*if (Mathf.FloorToInt(timer) % 1 == 0)
        {
            enemySpawnInfo();
        }*/
    }

    private string timeConvert(float time)
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        return niceTime;
    }

    private void enemySpawnInfo()
    {
        if (Mathf.FloorToInt(timer) > 0 && Mathf.FloorToInt(timer) <= 30)
        {
            for (int i = 0; i < amountToSpawn; i++)
                {
                    SpawnEnemy("Prefabs/TestingWobble");
                    creations++;
                    Debug.Log(creations);
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
