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
        SetupNameConversionList();
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
    
    
    private float spawnRadiusMin = 5f;
    private float spawnRadiusMax = 6f;
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
    //public bool gameEnded = false;
    private GameObject gameOverScreen;
    
    private bool yourBool;

    public enum EventName
    {
        Cluster,
        Boss,
        Circle
    }

    // Define a property for your boolean variable
    public bool gameEnded
    {
        get { return yourBool; }
        set
        {
            // Check if the value is actually changing
            if (yourBool != value)
            {
                // Log the change
                Debug.Log("YourBool changed to: " + value);

                // Set the new value
                yourBool = value;
            }
        }
    }
    
    [SerializeField]
    public List<Item> weaponsList;
    [SerializeField]
    public List<Item> evolutionWeaponsList;
    public Dictionary<string, int> weaponLevelCount;
    public List<Item> restrictedList;
    public List<GameObject> disabledObjects;

    private GameObject levelUpScreen;
    public LevelBar levelBar; //event from LevelBar
    public static event Action PlayerActionTakenEvent;
    
    public GameObject pauseScreen;
    private GameObject pauseButton;
    private GameObject steelContainerScreen;
    
    //Object Pooling
    public GameObject sasquets;
    public GameObject testingWobble;
    public GameObject zombie;
    public GameObject mushroom;
    public GameObject turtle;
    public GameObject smallplant;
    public GameObject mediumPlant;
    public GameObject largePlant;
    public GameObject boid;
    public GameObject blob;
    public GameObject shaman;
    public GameObject spirit;
    public GameObject boidNormal;

    public GameObject spiritCluster;
    

    private GameObject joystick;
    private GameObject itemLevels;
    
    private List<Coroutine> coroutineList = new List<Coroutine>();

    //Event for saving time with Achievement Manager
    public event Action OnMinutePassed;
    int lastMinuteTriggered = 0;
    
    public Dictionary<string, string> namesConversionList = new Dictionary<string, string>();



    // Start is called before the first frame update
    void Start()
    {
        //This was redundant, as restart is being called on scene loaded
        //StartEnemyCoroutines();
    }

    public void Restart()
    {
        disabledObjects = new List<GameObject>();
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
        FindSteelContainerScreen();
        FindTimerText();
        AssignPauseButton();
        FindItemLevelsMenu();
        
        StopEnemyCoroutines();
        StartEnemyCoroutines();
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
        
        //Calling when a minute has passed
        if (minutes > lastMinuteTriggered)
        {
            Debug.Log("Minute Unlocked!");
            OnMinutePassed.Invoke();
            lastMinuteTriggered = minutes;
        }
        
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

    private IEnumerator SpawnEvent(EventName eventName, GameObject enemy, int startTimeInSeconds, int amount)
    {
        // Wait until the specified start time.
        while (timer < startTimeInSeconds)
        {
            yield return null; // Wait for the next frame.
        }
        
        if (eventName == EventName.Boss)
        {
            SpawnBoss(enemy, player.transform.position + new Vector3(4.0f, 0.0f, 0.0f));
        } else if (eventName == EventName.Circle)
        {
            SpawnEnemiesInCircle(amount, enemy, spawnRadiusMin);
        } else if (eventName == EventName.Cluster)
        {
            SpawnCluster(enemy, amount);
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
            spawnedObject.SetActive(true);
            EnemyStats enemyStats = spawnedObject.GetComponent<EnemyStats>();
            enemyStats.ReMade();
            spawnedObject.transform.position = spawnPosition;
        }
    }
    
    public void SpawnEnemy(GameObject prefab, Vector3 position)
    {
        GameObject spawnedObject = ObjectPoolManager.Instance.GetObjectFromPool(prefab);
        if (spawnedObject != null)
        {
            spawnedObject.SetActive(true);
            EnemyStats enemyStats = spawnedObject.GetComponent<EnemyStats>();
            enemyStats.ReMade();
            spawnedObject.transform.position = position;
            
        }
    }
    
    public void SpawnBoss(GameObject prefab, Vector3 position)
    {
        GameObject spawnedObject = ObjectPoolManager.Instance.GetObjectFromPool(prefab);
        if (spawnedObject != null)
        {
            spawnedObject.SetActive(true);
            EnemyStats enemyStats = spawnedObject.GetComponent<EnemyStats>();
            enemyStats.ReMade();
            enemyStats.isBoss = true;
            spawnedObject.transform.position = position;
            
        }
    }

    public void SpawnEnemiesInCircle(int numberOfObjects, GameObject objectToSpawn, float radius)
    {
        if (numberOfObjects <= 0)
        {
            Debug.LogWarning("Number of objects should be greater than 0.");
            return;
        }

        float angleStep = 360f / numberOfObjects;

        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * angleStep;
            float radians = angle * Mathf.Deg2Rad;

            float x = transform.position.x + radius * Mathf.Cos(radians);
            float y = transform.position.y + radius * Mathf.Sin(radians);

            Vector2 spawnPosition = new Vector2(x, y);

            SpawnEnemy(objectToSpawn, spawnPosition);
            //Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    public void SpawnCluster(GameObject objectToSpawn, int numberOfObjects)
    {
        float angleStep = 360f / numberOfObjects;
        float radius = spawnRadiusMin;
        
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * angleStep;
            float radians = angle * Mathf.Deg2Rad;

            float x = transform.position.x + radius * Mathf.Cos(radians);
            float y = transform.position.y + radius * Mathf.Sin(radians);

            Vector2 spawnPosition = new Vector2(x, y);
            
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
    }


    //Managing UI
    public void EndGame()
    {
        if (gameEnded == false)
        {
            AudioManager.instance.Play("Lose");
            gameEnded = true;
            Debug.Log("GAME OVER");
            FindGameOverScreen();
            gameOverScreen.SetActive(true);
            UpdateReviveCount(PlayerManager.instance.player.GetComponent<CharacterStats>().revives.GetValue());
            AssignPauseButton();
            pauseButton.gameObject.SetActive(false);
            PauseGame();
        }
        else
        {
            Debug.Log("gameEnded = " + gameEnded);
            Debug.Log("i guess gameEnded is true?");
        }
    }

    //Should only be called if the player has a revive
    public void ContinueGame()
    {
        gameEnded = false;
        Debug.Log("In ContinueGame: " + gameEnded);
        Debug.Log("CONTINUING GAME");
        FindGameOverScreen();
        AssignPauseButton();
        pauseButton.gameObject.SetActive(true);
        ResumeGame();
        CharacterStats cs = PlayerManager.instance.player.GetComponent<CharacterStats>();
        cs.currentHealth = cs.maxHealth;
        cs.revives.RemoveModifier(1);
    }
    
    void UpdateReviveCount(int count)
    {
        Transform reviveCountTransform = gameOverScreen.transform.Find("ReviveCount");
        reviveCountTransform.gameObject.SetActive(true);
        Transform continueButton = gameOverScreen.transform.Find("Continue");
        continueButton.gameObject.SetActive(true);
        
        if (reviveCountTransform != null)
        {
            TextMeshProUGUI reviveCountText = reviveCountTransform.GetComponent<TextMeshProUGUI>();
            if (reviveCountText != null)
            {
                reviveCountText.text = "Revives: " + count.ToString();
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI component not found on ReviveCount child.");
            }
        }
        else
        {
            Debug.LogWarning("ReviveCount child not found under GameOverScreen.");
        }

        if (count < 1)
        {
            reviveCountTransform.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(false);
        }
    }

    public void HideGameOverScreen()
    {
        gameEnded = false;
        FindGameOverScreen();
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
        AddNewCoins();
        HideGameOverScreen();
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    public void PauseGame()
    {
        TargetAllByTag(false, "UIEffects");
        GameManager.instance.isPaused = true;
        Time.timeScale = 0;
    }
    
    public void ResumeGame()
    {
        TargetAllByTag(true, "UIEffects");
        GameManager.instance.isPaused = false;
        Time.timeScale = 1;
    }
    
    public bool IsGamePaused()
    {
        return GameManager.instance.isPaused;
    }

    public void TargetAllByTag(bool active, string tag)
    {
        if (active)
        {
            foreach (var disabledObj in disabledObjects)
            {
                disabledObj.SetActive(true);
            }
            disabledObjects.Clear();
        }
        else
        {
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.CompareTag(tag))
                {
                    disabledObjects.Add(obj);
                    obj.SetActive(false);
                }
            }
        }
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

        List<Item> shuffledWeapons = new List<Item>();

        if (Inventory.instance.items.Count == Inventory.instance.space)
        {
            //If inventory is full, only show weapons that are in inventory to level
            //This prevents player from getting new weapons when inventory is full
            print("inventory Size: " + Inventory.instance.items.Count());
            print("Items in Inventory.Instance: ");
            foreach (var item in Inventory.instance.items)
            {
                print(item.name);
                foreach (var weapon in weaponLevelCount)
                {
                    if (weaponLevelCount[item.itemName] != 10)
                    {
                        shuffledWeapons.Add(item);
                    }
                }
                shuffledWeapons = shuffledWeapons.OrderBy(item => Random.value).ToList();
            }
        }
        else
        {
            // Shuffle the weaponsList
            shuffledWeapons = weaponsList.OrderBy(item => Random.value).ToList();
        }
        

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

        //For debugging choosing an equipment on levelup
        //randomWeapons[2] = weaponsList[10];
        
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
    
    bool AreItemsUnique(List<Item> itemList)
    {
        HashSet<string> itemNames = new HashSet<string>();

        foreach (Item item in itemList)
        {
            if (!itemNames.Add(item.itemName))
            {
                // If the item name is already in the HashSet, it's not unique
                return false;
            }
        }

        // All item names are unique
        return true;
    }
    
    List<Item> GenerateUniqueItemList()
    {
        List<Item> uniqueItems = ThreeRandomItems();

        while (!AreItemsUnique(uniqueItems))
        {
            uniqueItems = ThreeRandomItems();
        }

        return uniqueItems;
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
        GameObject levelSlotPrefab = Resources.Load<GameObject>("PreFabs/UI/LevelSlot");

        List<Item> threeItems = GenerateUniqueItemList();
        
        
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

                Transform itemNameField = levelSlot.transform.Find("LevelButton/WeaponImage/ItemName");
                TextMeshProUGUI itemNameFieldText = itemNameField.GetComponent<TextMeshProUGUI>();

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
                        itemNameFieldText.text = namesConversionList.ContainsKey(threeItems[i].itemName) ? namesConversionList[threeItems[i].itemName] : threeItems[i].itemName;
                    }
                    else
                    {
                        descriptionText.text = " ";
                        itemNameFieldText.text = " ";
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
                        itemNameFieldText.text = threeItems[i].itemName;
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
                        itemNameFieldText.text = threeItems[i].itemName;
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
        AudioManager.instance.Play("pauseOn");
        Debug.Log("showing pause screen");
        
        FindGamePauseScreen();
        FindFloatingJoystick();
        FindItemLevelsMenu();
        
        if (IsGamePaused())
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
    
    public void ShowSteelContainerScreen()
    {
        Debug.Log("showing SteelContainer screen");
        FindSteelContainerScreen();
        FindFloatingJoystick();
        pauseButton.gameObject.SetActive(false);

        if (IsGamePaused())
        {
            Debug.Log("isPaused");
            steelContainerScreen.SetActive(false);
            joystick.SetActive(true);
            ResumeGame();
        }
        else
        {
            Debug.Log("else");
            // Set the steelContainerScreen to active
            steelContainerScreen.SetActive(true);
            joystick.SetActive(false);

            // Pause the game
            PauseGame();
        }
    }

    private void FindSteelContainerScreen()
    {
        //Finding SteelContainerScreen
        GameObject canvas = GameObject.FindWithTag("Canvas");
        Transform steelContainerTransform = canvas.transform.Find("SteelContainerScreen");
        steelContainerScreen = steelContainerTransform.gameObject;
        steelContainerScreen.SetActive(false);
    }
    
    public void HideSteelContainerScreen()
    {
        FindSteelContainerScreen();

        //Enable Pause Button
        AssignPauseButton();
        pauseButton.gameObject.SetActive(true);
        
        FindFloatingJoystick();
        joystick.SetActive(true);
        
        Debug.Log("Hiding SteelContainerScreen");
        ResumeGame();
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
    
    private void StartEnemyCoroutines()
    {
        //SpawnEnemiesInCircle(48, zombie, spawnRadiusMin);
        //SpawnBoss(testingWobble, player.transform.position + new Vector3(1.0f, 0.0f, 0.0f));
        
        //minute 0
        Coroutine enemyCoroutine0 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(blob, 1, 30*2, 0.1f, 15));
        
        //minute 1
        Coroutine enemyCoroutine1 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(zombie, 60, 60, 0.1f, 30));
        Coroutine enemyCoroutine2 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(blob, 60, 60, 0.1f, 30));
        Coroutine bossCoroutine0 = StartCoroutine(SpawnEvent(EventName.Boss, zombie, 60, 1));
        
        //minute 2
        Coroutine enemyCoroutine3 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(blob, 60*2, 60, 0.5f, 150));
        Coroutine bossCoroutine1 = StartCoroutine(SpawnEvent(EventName.Cluster, spiritCluster, 60 * 2, 3));

        //minute 3
        Coroutine enemyCoroutine5 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(blob, 180, 60, 0.1f, 40));
        
        //minute 4
        Coroutine enemyCoroutine6 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(blob, 240, 60, 0.1f, 30));
        Coroutine enemyCoroutine7 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(mushroom, 240, 60, 0.1f, 30));
        
        //minute 5
        Coroutine enemyCoroutine8 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(zombie, 300, 60, 0.1f, 10));
        Coroutine enemyCoroutine9 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(sasquets, 300, 60, 0.1f, 30));
        
        //minute 6
        Coroutine enemyCoroutine10 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(blob, 360, 60, 0.1f, 20));
        Coroutine enemyCoroutine11 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(zombie, 360, 60, 0.1f, 20));
        
        //minute 7
        Coroutine enemyCoroutine12 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(turtle, 420, 60, 0.1f, 15));
        Coroutine enemyCoroutine13 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(sasquets, 420, 60, 0.1f, 160));
        Coroutine enemyCoroutine14 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(mushroom, 420, 60, 0.1f, 80));
        
        //minute 8
        Coroutine enemyCoroutine15 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(zombie, 480, 60, 0.1f, 100));
        
        //minute 9
        Coroutine enemyCoroutine16 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(smallplant, 540, 60, 0.1f, 30));
        Coroutine enemyCoroutine17 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(blob, 540, 60, 0.1f, 30));
        
        //minute 10
        Coroutine enemyCoroutine18 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(mushroom, 600, 60, 0.1f, 100));
        Coroutine enemyCoroutine19 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(turtle, 600, 60, 0.1f, 10));
        
        //minute 11
        Coroutine enemyCoroutine20 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(testingWobble, 660, 60, 0.1f, 300));
        
        //minute 12
        
        //minute 13
        
        //minute 14
        
        //minute 15
        
        //minute 16
        
        //minute 17
        
        //minute 18
        
        //minute 19
        
        //minute 20
        
        //minute 21
        
        //minute 22
        
        //minute 23
        
        //minute 24
        
        //minute 25
        
        //minute 26
        
        //minute 27
        
        //minute 28
        
        //minute 29
        
        //minute 30
        
        
        coroutineList.Add(enemyCoroutine0);
        coroutineList.Add(enemyCoroutine1);
        coroutineList.Add(enemyCoroutine2);
        coroutineList.Add(enemyCoroutine3);
        //coroutineList.Add(enemyCoroutine4);
        coroutineList.Add(enemyCoroutine5);
        coroutineList.Add(enemyCoroutine6);
        coroutineList.Add(enemyCoroutine7);
        coroutineList.Add(enemyCoroutine8);
        coroutineList.Add(enemyCoroutine9);
        coroutineList.Add(enemyCoroutine10);
        coroutineList.Add(enemyCoroutine11);
        coroutineList.Add(enemyCoroutine12);
        coroutineList.Add(enemyCoroutine13);
        coroutineList.Add(enemyCoroutine14);
        coroutineList.Add(enemyCoroutine15);
        coroutineList.Add(enemyCoroutine16);
        coroutineList.Add(enemyCoroutine17);
        coroutineList.Add(enemyCoroutine18);
        coroutineList.Add(enemyCoroutine19);
        coroutineList.Add(enemyCoroutine20);
        
        coroutineList.Add(bossCoroutine0);
        coroutineList.Add(bossCoroutine1);
    }

    private void StopEnemyCoroutines()
    {
        if (coroutineList.Count > 0)
        {
            foreach (var coroutine in coroutineList)
            {
                StopCoroutine(coroutine);
            }
            coroutineList.Clear();
        }
    }

    private void AddNewCoins()
    {
        try
        {
            Debug.Log("Running Add New Coins");
            DataManager dataManager = DataManager.Instance;
            List<Total> loadedData = dataManager.LoadData<Total>(DataManager.DataType.Total);
            // Find the specific Total object by name
            Total totalToModify = loadedData.Find(t => t.name == "coinsTotal");
            
            if (totalToModify != null)
            {
                // Modify the value
                totalToModify.value += Mathf.RoundToInt(CoinCounter.instance.coinCount);

                // Save the modified list
                dataManager.SaveData(loadedData, DataManager.DataType.Total);

                Debug.Log($"Modified coins value");
            }
            else
            {
                Debug.Log($"Total not found.");
            }
        }
        catch (Exception e)
        {
            Debug.Log("Catch: " + e);
        }
    }

    public void SetupNameConversionList()
    {
        namesConversionList.Add("LazerGun", "Lazer Gun");
        namesConversionList.Add("Body_Armour", "Body Armor");
        namesConversionList.Add("Exolegs", "Exoskeleton Legs");
        namesConversionList.Add("Targeting_Computer", "Targeting Computer");
    }

}
