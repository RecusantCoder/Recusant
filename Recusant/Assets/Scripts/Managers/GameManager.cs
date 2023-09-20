using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        if (instance != null && instance != this)
        {
            // Another instance of GameManager exists, destroy this one
            Destroy(gameObject);
            return;
        }

        // Set this instance as the singleton instance
        instance = this;

        // Ensure the GameManager persists across scenes
        DontDestroyOnLoad(gameObject);
    }
    
    #endregion
    
    
    private float spawnRadiusMin = 7f;
    private float spawnRadiusMax = 14f;
    public int amountToSpawn = 1;
    private bool isPaused = false;
    
    //For timer
    private float timer = 0.0f;
    [SerializeField]
    public TMP_Text timerText;
    private int lastSecond = 0;

    private int creations = 0;

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
    public static event Action PlayerActionTakenEvent;
    
    public GameObject pauseScreen;
    private GameObject pauseButton;
    
    //Object Pooling
    public GameObject sasquets;
    public GameObject testingWobble;
    public GameObject iceSnake;
    public GameObject zombie;
    public GameObject mushroom;
    public GameObject turtle;
    public GameObject smallplant;
    public GameObject mediumPlant;
    public GameObject largePlant;

    private GameObject joystick;
    private GameObject itemLevels;
    



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(sasquets, 0, 60, 0.1f, 15));
        
        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(turtle, 60, 60, 1, 15));
        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(sasquets, 60, 60, 0.1f, 30));
        
        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(sasquets, 120, 60, 1, 50));

        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(iceSnake, 180, 60, 1, 40));
        
        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(iceSnake, 240, 60, 1, 30));
        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(mushroom, 240, 60, 1, 30));
        
        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(zombie, 300, 60, 1, 10));
        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(sasquets, 300, 60, 1, 30));
        
        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(iceSnake, 360, 60, 1, 20));
        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(zombie, 360, 60, 1, 20));
        
        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(turtle, 420, 60, 1, 15));
        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(sasquets, 420, 60, 1, 80));
        
        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(zombie, 480, 60, 1, 100));
        
        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(smallplant, 540, 60, 1, 15));
        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(iceSnake, 540, 60, 1, 30));
        
        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(mushroom, 600, 60, 1, 10));
        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(turtle, 600, 60, 1, 10));

        StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(testingWobble, 660, 60, 1, 300));
    }

    public void Restart()
    {
        ResumeGame();

        timer = 0.0f;
        
        Debug.Log("Restart");
        
        player = GameObject.FindWithTag("Player").transform;

        //Setting up LevelUp Screen
        weaponLevelCount = new Dictionary<string, int>();
        FindLevelUpScreen();
        LevelBar.OnLevelUp += LevelUpHandler;
        
        FindGameOverScreen();
        FindGamePauseScreen();
        FindTimerText();
        AssignPauseButton();
        FindItemLevelsMenu();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Timer
        timer += Time.deltaTime;
        timerText.text = timeConvert(timer);
    }

    private string timeConvert(float time)
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        return niceTime;
    }

    private IEnumerator SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(GameObject enemy, int startTimeInSeconds, int duration, float interval, int amount)
    {
        // Wait until the specified start time.
        while (timer < startTimeInSeconds)
        {
            yield return null; // Wait for the next frame.
        }
        
        while (timer <= startTimeInSeconds + duration)
        {
            // Calculate the number of active enemies.
            int activeEnemyCount = ObjectPoolManager.Instance.CountActiveObjectsInPool(enemy);
        
            // If the active enemy count is less than the desired amount, spawn more enemies.
            if (activeEnemyCount < amount)
            {
                // Spawn an enemy.
                SpawnEnemy(enemy);
            }
        
            // Wait for the specified interval before the next spawn.
            yield return new WaitForSeconds(interval);
        }
    }

    public void SpawnEnemy(GameObject prefab)
    {
        float minDistance = spawnRadiusMin; // Minimum distance from the player
        float maxDistance = spawnRadiusMax; // Maximum distance from the player

        Vector3 spawnPosition;

        do
        {
            float angle = Random.Range(0f, 360f);
            float distance = Random.Range(minDistance, maxDistance);

            Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 spawnOffset = rotation * Vector3.right * distance;

            // Adjust the range for the Y-axis offset
            float yOffset = Random.Range(-maxDistance, maxDistance) * 0.5f; // Use a smaller range by multiplying by 0.5f

            // Add the adjusted Y-axis offset
            spawnOffset += Vector3.up * yOffset;

            spawnPosition = player.transform.position + spawnOffset;

            // Set the Z position to be the same as the player's
            spawnPosition.z = player.transform.position.z;
        }
        while (Vector3.Distance(spawnPosition, player.transform.position) < minDistance);

        GameObject spawnedObject = ObjectPoolManager.Instance.GetObjectFromPool(prefab);
        if (spawnedObject != null)
        {
            EnemyStats enemyStats = spawnedObject.GetComponent<EnemyStats>();
            enemyStats.ReMade();
            spawnedObject.transform.position = spawnPosition;
            spawnedObject.SetActive(true);
        }
    }
    
    public void SpawnEnemy(GameObject prefab, Vector3 position)
    {
        GameObject spawnedObject = ObjectPoolManager.Instance.GetObjectFromPool(prefab);
        if (spawnedObject != null)
        {
            EnemyStats enemyStats = spawnedObject.GetComponent<EnemyStats>();
            enemyStats.ReMade();
            spawnedObject.transform.position = position;
            spawnedObject.SetActive(true);
        }
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
            AssignPauseButton();
            pauseButton.gameObject.SetActive(false);
            PauseGame();
        }
    }

    public void HideGameOverScreen()
    {
        gameEnded = false;
        gameOverScreen.SetActive(false);
        Debug.Log("Called HideGameOverScreen");
    }
    
    private void FindGameOverScreen()
    {
        //Finding GameOverScreen
        GameObject canvas = GameObject.FindWithTag("Canvas");
        Transform gameOverScreenTransform = canvas.transform.Find("GameOverScreen");
        if (gameOverScreenTransform != null)
        {
            Debug.Log("found game over screen");
        }
        else
        {
            Debug.Log("its null dude");
        }
        gameOverScreen = gameOverScreenTransform.gameObject;
        gameOverScreen.GetComponent<ObjectDestroyedEvent>().OnDestroyed.AddListener(ObjectDestroyed);
        gameOverScreen.SetActive(false);
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
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

    public List<Item> ThreeRandomItems()
    {
        List<Item> randomWeapons = new List<Item>();
        int count = 0;
        
        // Shuffle the weaponsList
        List<Item> shuffledWeapons = weaponsList.OrderBy(item => Random.value).ToList();

        foreach (Item weapon in shuffledWeapons)
        {
            // Check if the weapon's itemName is not in weaponLevelCount or its value is not 10
            if (!weaponLevelCount.ContainsKey(weapon.itemName) || weaponLevelCount[weapon.itemName] != 10)
            {
                randomWeapons.Add(weapon);
                count++;

                if (count >= 3)
                {
                    break;
                }
            }
        }

        randomWeapons[2] = weaponsList[10];
        return randomWeapons;
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
        FindLevelUpScreen();
        levelUpScreen.SetActive(true);

        //Disable pause button and joystick until level up is over
        pauseButton.gameObject.SetActive(false);
        FindFloatingJoystick();
        joystick.SetActive(false);

        // Find the child object by tag
        Transform levelSlotParent = levelUpScreen.transform.Find("LevelSlotParent");

        // Instantiate the LevelSlot prefab as a child of the LevelSlotParent
        GameObject levelSlotPrefab = Resources.Load<GameObject>("Prefabs/LevelSlot");

        List<Item> threeItems = ThreeRandomItems();
        
        Debug.Log(threeItems.Count + " is threeItems count");

        for (int i = 0; i < threeItems.Count; i++)
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
                
                //Setting description in LevelSlot UI
                Transform description = levelSlot.transform.Find("LevelButton/WeaponImage/Description");
                TextMeshProUGUI descriptionText = description.GetComponent<TextMeshProUGUI>();

                Debug.Log("WL " + weaponLevel + " name " + threeItems[i].itemName);
                
                if (weaponLevel == 0)
                {
                    weaponLevelImage1Image.sprite = Resources.Load<Sprite>("Sprites/confusedItem");
                    Color c = weaponLevelImage2Image.color;
                    c.a = 0.0f;
                    weaponLevelImage2Image.color = c;

                    if (threeItems[i].levelDescriptions.Count > 0)
                    {
                        descriptionText.text = threeItems[i].levelDescriptions[weaponLevel];
                    }
                    else
                    {
                        descriptionText.text = " ";
                    }
                }
                
                if (weaponLevel >= 1 && weaponLevel <= 5)
                {
                    String tallySpritePath = "Sprites/tally" + weaponLevel;
                    weaponLevelImage1Image.sprite = Resources.Load<Sprite>(tallySpritePath);
                    Color c = weaponLevelImage2Image.color;
                    c.a = 0.0f;
                    weaponLevelImage2Image.color = c;
                    
                    if (threeItems[i].levelDescriptions.Count > 0)
                    {
                        descriptionText.text = threeItems[i].levelDescriptions[weaponLevel];
                    }
                    else
                    {
                        descriptionText.text = " ";
                    }
                        
                }
                
                if (weaponLevel >= 6 && weaponLevel <= 10)
                {
                    weaponLevelImage1Image.sprite = Resources.Load<Sprite>("Sprites/tally5");
                        
                    Color c = weaponLevelImage2Image.color;
                    c.a = 1.0f;
                    weaponLevelImage2Image.color = c;
                    int weaponLevelMinus5 = weaponLevel - 5;
                    Debug.Log("WeaponLevel = " + weaponLevel + " wminus5 = " + weaponLevelMinus5);
                    String tallySpritePath = "Sprites/tally" + weaponLevelMinus5;
                    weaponLevelImage2Image.sprite = Resources.Load<Sprite>(tallySpritePath);
                    
                    if (threeItems[i].levelDescriptions.Count > 0)
                    {
                        descriptionText.text = threeItems[i].levelDescriptions[weaponLevel];
                    }
                    else
                    {
                        descriptionText.text = " ";
                    }
                }


            }

            PauseGame();
        }
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
        
        //Enable Pause Button
        AssignPauseButton();
        pauseButton.gameObject.SetActive(true);
        
        FindFloatingJoystick();
        joystick.SetActive(true);
        
        Debug.Log("Hiding LevelUpScreen");
        ResumeGame();
        if (PlayerActionTakenEvent != null)
        {
            PlayerActionTakenEvent.Invoke();
        }
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
    
    private void FindGamePauseScreen()
    {
        //Finding PauseScreen
        GameObject canvas = GameObject.FindWithTag("Canvas");
        Transform pauseScreenTransform = canvas.transform.Find("OptionsMenu");
        pauseScreen = pauseScreenTransform.gameObject;
        pauseScreen.SetActive(false);
    }

    private void FindFloatingJoystick()
    {
        GameObject canvas = GameObject.FindWithTag("Canvas");
        Transform joystickTransform = canvas.transform.Find("Floating Joystick");
        joystick = joystickTransform.gameObject;
        joystick.SetActive(false);
    }
    
    public void ShowPauseScreen()
    {
        Debug.Log("showing pause screen");
        
        FindGamePauseScreen();
        FindFloatingJoystick();
        FindItemLevelsMenu();
        
        if (isPaused)
        {
            pauseScreen.SetActive(false);
            joystick.SetActive(true);
            itemLevels.SetActive(false);
            ResumeGame();
        }
        else
        {
            // Set the pause screen to active
            pauseScreen.SetActive(true);

            joystick.SetActive(false);
            itemLevels.SetActive(true);
            
            // Pause the game
            PauseGame();
        }
    }

    public void AssignPauseButton()
    {
        GameObject canvas = GameObject.FindWithTag("Canvas");
        Transform pauseButtonTransform = canvas.transform.Find("PauseButton");
        if (pauseButtonTransform == null)
        {
            // Child object not found, try finding an inactive child
            pauseButtonTransform = canvas.transform.Find("/PauseButton");
        }
        pauseButton = pauseButtonTransform.gameObject;
    }
    
    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // This method is called whenever a new scene is loaded
        // Perform your scene-specific initialization here
        Debug.Log("Scene loaded: " + scene.name);
        
        if (scene.name == "Level1")
        {
            // Perform actions specific to "YourSceneName"
            Debug.Log("running OnSceneLoaded for Level1");
            Restart();
        }else if (scene.name == "MainMenu")
        {
            // Perform actions specific to "YourSceneName"
            Debug.Log("running OnSceneLoaded for MainMenu");
            ClearInventory();
        }
    }
    
    private void FindTimerText()
    {
        //Finding TimerText
        GameObject canvas = GameObject.FindWithTag("Canvas");
        Transform timerTransform = canvas.transform.Find("Timer");
        timerText = timerTransform.GetComponentInChildren<TMP_Text>();
    }

    private void ClearInventory()
    {
        Inventory.instance.items = new List<Item>();
    }
    
    private void FindItemLevelsMenu()
    {
        GameObject canvas = GameObject.FindWithTag("Canvas");
        Transform itemLevelsTransform = canvas.transform.Find("ItemLevels");
        itemLevels = itemLevelsTransform.gameObject;
        itemLevels.SetActive(false);
    }
    

}
