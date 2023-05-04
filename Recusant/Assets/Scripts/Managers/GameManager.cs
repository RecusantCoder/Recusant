using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

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
        
        // Ensure there is only one instance of this script in the scene
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        // Make this object persistent across scenes
        DontDestroyOnLoad(gameObject);
    }
    
    #endregion
    
    public float spawnRadius = 5f;
    public int amountToSpawn = 1;
    private bool isPaused = false;
    
    
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
    
    //for game end
    public bool gameEnded = false;
    private GameObject gameOverScreen;
    
    //for LevelUp Screen
    [SerializeField]
    public List<Item> weaponsList;
    public Dictionary<string, int> weaponLevelCount;
    private GameObject levelUpScreen;
    public LevelBar levelBar; //event from LevelBar

    // Start is called before the first frame update
    void Start()
    {
        //player = PlayerManager.instance.player.transform;
        //HideGameOverScreen();
        Debug.Log("New Start");
        
        //Finding GameOverScreen
        FindGameOverScreen();
        
        //Setting up LevelUp Screen
        weaponLevelCount = new Dictionary<string, int>();
        FindLevelUpScreen();
        LevelBar.OnLevelUp += LevelUpHandler;

    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindWithTag("Player").transform;
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
        if (Mathf.FloorToInt(timer) > 0 && Mathf.FloorToInt(timer) <= 60)
        {
            for (int i = 0; i < amountToSpawn; i++)
                {
                    SpawnEnemy("Prefabs/TestingWobble");
                    creations++;
                    Debug.Log(creations);
                }
        }
        
        if (Mathf.FloorToInt(timer) > 0 && Mathf.FloorToInt(timer) <= 60)
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                SpawnEnemy("Prefabs/Sasqets");
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
    
    //Managing UI
    public void EndGame()
    {
        if (gameEnded == false)
        {
            gameEnded = true;
            Debug.Log("GAME OVER");
            FindGameOverScreen();
            gameOverScreen.SetActive(true);
            PauseGame();
        }
    }

    public void HideGameOverScreen()
    {
        gameEnded = false;
        gameOverScreen.SetActive(false);
        Debug.Log("Called HideGameOverScreen");
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        HideGameOverScreen();
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
    }
    
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
    }
    
    public bool IsGamePaused()
    {
        return isPaused;
    }

    public void QuitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
        Debug.Log("QUIT!");
    }
    
    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        Debug.Log("GameManager is being destroyed");
        // Perform any necessary cleanup here
    }
    
    private void ObjectDestroyed()
    {
        //This doesnt work as its a child object of canvas
        //gameOverScreen = GameObject.FindWithTag("GameOverScreen");
        FindGameOverScreen();
    }

    private void FindGameOverScreen()
    {
        //Finding GameOverScreen
        GameObject canvas = GameObject.FindWithTag("Canvas");
        Transform gameOverScreenTransform = canvas.transform.Find("GameOverScreen");
        gameOverScreen = gameOverScreenTransform.gameObject;
        gameOverScreen.GetComponent<ObjectDestroyedEvent>().OnDestroyed.AddListener(ObjectDestroyed);
        gameOverScreen.SetActive(false);
    }

    public List<Item> ThreeRandomItems()
    {
        List<Item> chosenItems = new List<Item>();

        while (chosenItems.Count < 3)
        {
            int randomIndex = UnityEngine.Random.Range(0, weaponsList.Count);
            Item chosenItem = weaponsList[randomIndex];
            if (!chosenItems.Contains(chosenItem))
            {
                chosenItems.Add(chosenItem);
            }
        }

        return chosenItems;
    }
    
    private void FindLevelUpScreen()
    {
        //Finding LevelUpScreen
        GameObject canvas = GameObject.FindWithTag("Canvas");
        Transform levelUpScreenTransform = canvas.transform.Find("LevelUpScreen");
        levelUpScreen = levelUpScreenTransform.gameObject;
        //gameOverScreen.GetComponent<ObjectDestroyedEvent>().OnDestroyed.AddListener(ObjectDestroyed);
        levelUpScreen.SetActive(false);
    }
    
    public void LevelUp()
    {
        Debug.Log("LevelUp method");
        FindLevelUpScreen();
        levelUpScreen.SetActive(true);

        // Find the child object by tag
        Transform levelSlotParent = levelUpScreen.transform.Find("LevelSlotParent");

        // Instantiate the LevelSlot prefab as a child of the LevelSlotParent
        GameObject levelSlotPrefab = Resources.Load<GameObject>("Prefabs/LevelSlot");

        List<Item> threeItems = ThreeRandomItems();
        for (int i = 0; i < 3; i++)
        {
            GameObject levelSlot = Instantiate(levelSlotPrefab, levelSlotParent);
            LevelSlot levelSlotScript = levelSlot.GetComponent<LevelSlot>();
            levelSlotScript.item = threeItems[i];
            
            // Assuming you have a reference to the parent LevelSlot
            Transform tallyImages = levelSlot.transform.Find("LevelButton/WeaponImage/TallyImages");
            Transform weaponLevelImage1 = tallyImages.transform.Find("WeaponLevelImage1");
            Transform weaponLevelImage2 = tallyImages.transform.Find("WeaponLevelImage2");
            
            Image weaponLevelImage1Image = weaponLevelImage1.GetComponent<Image>();
            Image weaponLevelImage2Image = weaponLevelImage2.GetComponent<Image>();
            
            //adds a weapon to weaponLevelCount if not picked before
            if (!weaponLevelCount.ContainsKey(threeItems[i].itemName))
            {
                weaponLevelCount.Add(threeItems[i].itemName, 0);
            }

            //sets weapon level tally image in levelslot    
            if (weaponLevelCount.ContainsKey(threeItems[i].itemName))
            {
                int weaponLevel = weaponLevelCount[threeItems[i].itemName];
                if (weaponLevel == 0)
                {
                    weaponLevelImage1Image.sprite = Resources.Load<Sprite>("Sprites/confusedItem");
                }
                else
                {
                    String tallySpritePath = "Sprites/tally" + weaponLevel;
                    weaponLevelImage1Image.sprite = Resources.Load<Sprite>(tallySpritePath);
                }
            }
            

        }
        PauseGame();
    }

    public void HideLevelUpScreen()
    {
        //Finding LevelUpScreen
        GameObject canvas = GameObject.FindWithTag("Canvas");
        Transform levelUpScreenTransform = canvas.transform.Find("LevelUpScreen");
        levelUpScreen = levelUpScreenTransform.gameObject;
        // Find the child object by tag
        Transform levelSlotParent = levelUpScreen.transform.Find("LevelSlotParent");
        for (int i = 0; i < levelSlotParent.childCount; i++)
        {
            Destroy(levelSlotParent.GetChild(i).gameObject);
        }
        
        FindLevelUpScreen();
        Debug.Log("Hiding LevelUpScreen");
        ResumeGame();
    }
    
    void LevelUpHandler(int newLevel)
    {
        Debug.Log("LISTENED! Player leveled up to level " + newLevel);
        // Do whatever else you need to do when the player levels up
        
        LevelUp();
    }
    

    public void LevelSelectedWeapon(string weaponName)
    {
        weaponLevelCount[weaponName]++;
    }

}
