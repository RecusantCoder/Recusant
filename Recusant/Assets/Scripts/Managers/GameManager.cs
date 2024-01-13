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
    public List<GameObject> disabledObjects = new List<GameObject>();

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
        //disabledObjects = new List<GameObject>();
        ResumeGame();

        timer = 0.0f;
        
        Debug.Log("Restart");
        
        player = GameObject.FindWithTag("Player").transform;

        //Setting up LevelUp Screen
        weaponLevelCount = new Dictionary<string, int>();
        FindLevelUpScreen();
        LevelBar.OnLevelUp += LevelUpHandler;
        Debug.Log("Subbed to LevelUp Handler");
        
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
        
        Debug.Log("Spawning at " + startTimeInSeconds);
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
    
    public GameObject SpawnEnemyWithReturn(GameObject prefab, Vector3 position)
    {
        GameObject spawnedObject = ObjectPoolManager.Instance.GetObjectFromPool(prefab);
        if (spawnedObject != null)
        {
            spawnedObject.SetActive(true);
            EnemyStats enemyStats = spawnedObject.GetComponent<EnemyStats>();
            enemyStats.ReMade();
            spawnedObject.transform.position = position;
            
        }

        return spawnedObject;
    }
    
    public void SpawnBoss(GameObject prefab, Vector3 position)
    {
        GameObject spawnedObject = ObjectPoolManager.Instance.GetObjectFromPool(prefab);
        if (spawnedObject != null)
        {
            EnemyStats enemyStats = spawnedObject.GetComponent<EnemyStats>();
            enemyStats.ReMade();
            enemyStats.isBoss = true;
            spawnedObject.transform.position = position;
            spawnedObject.SetActive(true);
            
        }
    }

    public void SpawnEnemiesInCircle(int numberOfObjects, GameObject objectToSpawn, float radius)
    {
        if (numberOfObjects <= 0)
        {
            Debug.LogWarning("Number of objects should be greater than 0.");
            return;
        }

        float angleStep = 360f / (numberOfObjects - 1);

        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * angleStep;
            float radians = angle * Mathf.Deg2Rad;

            float x = transform.position.x + radius * Mathf.Cos(radians);
            float y = transform.position.y + radius * Mathf.Sin(radians);

            Vector3 spawnPosition = new Vector3(x, y, GameManager.instance.player.position.z) + GameManager.instance.player.position;

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

            Vector3 spawnPosition = new Vector3(x, y, GameManager.instance.player.position.z) + GameManager.instance.player.position;
            
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            Debug.Log("Spawned cluster!");
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
        GameManager.instance.isPaused = false;
        Time.timeScale = 1;
        TargetAllByTag(true, "UIEffects");
    }
    
    public bool IsGamePaused()
    {
        return GameManager.instance.isPaused;
    }

    public void TargetAllByTag(bool active, string tag)
    {
        if (active)
        {
            foreach (var disabledObj in instance.disabledObjects)
            {
                disabledObj.SetActive(true);
            }
            instance.disabledObjects.Clear();
        }
        else
        {
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.CompareTag(tag))
                {
                    instance.disabledObjects.Add(obj);
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
        
        Debug.Log("Weapon adding started.");
        
        // CHANGING THIS: Check if the weapon's itemName is not in weaponLevelCount or its value is not 10
        // This should now add weapons that are under level 10, has to check if they are in the weaponLevelCount
        // before checking level or it will give error, also will add weapons not discovered yet as choices
        foreach (Item weapon in shuffledWeapons)
        {
            if ((weaponLevelCount.ContainsKey(weapon.itemName) && weaponLevelCount[weapon.itemName] <= 9) || !weaponLevelCount.ContainsKey(weapon.itemName))
            {
                randomWeapons.Add(weapon);
                count++;
                
                Debug.Log("Weapon added to random weapons: " + weapon.itemName);

                if (count >= 3)
                {
                    Debug.Log("Weapon adding ended.");
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
        Debug.Log("Calling FindLevelUpScreen ");
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

        Debug.Log("Calling GenerateUniqueItemList ");
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
        //Coroutine testCoroutine = StartCoroutine(SpawnEvent(EventName.Circle, smallplant, 13, 360));
        
        //minute 0
        Coroutine enemyCoroutine0 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(blob, 61, 30*2, 0.1f, 15));
        
        //minute 1
        Coroutine enemyCoroutine1 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(zombie, 60, 60, 0.1f, 30));
        Coroutine enemyCoroutine2 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(blob, 60, 60, 0.1f, 30));
        
        Coroutine eventCoroutine0 = StartCoroutine(SpawnEvent(EventName.Boss, zombie, 60, 1));
        
        //minute 2
        Coroutine enemyCoroutine3 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(blob, 60*2, 60, 0.5f, 150));
        
        Coroutine eventCoroutine1 = StartCoroutine(SpawnEvent(EventName.Cluster, spiritCluster, 60*2, 3));

        //minute 3
        Coroutine enemyCoroutine4 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(boidNormal, 60*3, 60, 0.1f, 40));
        
        Coroutine eventCoroutine2 = StartCoroutine(SpawnEvent(EventName.Boss, zombie, 60*3, 1));
        Coroutine eventCoroutine3 = StartCoroutine(SpawnEvent(EventName.Cluster, spiritCluster, 60*3, 2));
        
        //minute 4
        Coroutine enemyCoroutine5 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(boidNormal, 60*4, 60, 0.1f, 40));
        Coroutine enemyCoroutine6 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(spirit, 60*4, 60, 0.1f, 30));
        
        Coroutine eventCoroutine4 = StartCoroutine(SpawnEvent(EventName.Cluster, spiritCluster, 60*4, 2));
        
        //minute 5
        Coroutine enemyCoroutine7 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(mushroom, 60*5, 60, 0.1f, 10));
        
        Coroutine eventCoroutine5 = StartCoroutine(SpawnEvent(EventName.Circle, smallplant, 60*5, 1));
        Coroutine eventCoroutine6 = StartCoroutine(SpawnEvent(EventName.Boss, testingWobble, 60*5, 1));
        
        //minute 6
        Coroutine enemyCoroutine8 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(zombie, 60*6, 60, 0.1f, 20));
        Coroutine enemyCoroutine9 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(mushroom, 60*56, 60, 0.1f, 20));
        
        Coroutine eventCoroutine7 = StartCoroutine(SpawnEvent(EventName.Cluster, spiritCluster, 60*6, 2));
        
        //minute 7
        Coroutine enemyCoroutine10 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(blob, 60*7, 60, 0.1f, 160));
        Coroutine enemyCoroutine11 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(mushroom, 60*7, 60, 0.1f, 80));
        
        Coroutine eventCoroutine8 = StartCoroutine(SpawnEvent(EventName.Boss, zombie, 60*7, 1));
        Coroutine eventCoroutine9 = StartCoroutine(SpawnEvent(EventName.Cluster, spiritCluster, 60*7, 7));
        
        //minute 8
        Coroutine enemyCoroutine12 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(zombie, 60*8, 60, 0.1f, 100));
        Coroutine enemyCoroutine13 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(sasquets, 60*8, 60, 0.1f, 1));
        
        Coroutine eventCoroutine10 = StartCoroutine(SpawnEvent(EventName.Cluster, spiritCluster, 60*8, 3));
        
        //minute 9
        Coroutine enemyCoroutine14 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(zombie, 60*9, 60, 0.1f, 30));
        Coroutine enemyCoroutine15 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(sasquets, 60*9, 60, 0.1f, 30));
        
        Coroutine eventCoroutine11 = StartCoroutine(SpawnEvent(EventName.Boss, zombie, 60*9, 1));
        Coroutine eventCoroutine12 = StartCoroutine(SpawnEvent(EventName.Cluster, spiritCluster, 60*9, 3));
        
        //minute 10
        Coroutine enemyCoroutine16 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(mushroom, 60*10, 60, 0.1f, 20));
        
        Coroutine eventCoroutine13 = StartCoroutine(SpawnEvent(EventName.Boss, testingWobble, 60*10, 1));
        Coroutine eventCoroutine14 = StartCoroutine(SpawnEvent(EventName.Circle, smallplant, 60*10, 2));
        
        //minute 11
        Coroutine enemyCoroutine17 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(boidNormal, 60*11, 60, 0.1f, 300));
        
        Coroutine eventCoroutine15 = StartCoroutine(SpawnEvent(EventName.Boss, zombie, 60*11, 1));
        Coroutine eventCoroutine16 = StartCoroutine(SpawnEvent(EventName.Cluster, spiritCluster, 60*11, 2));
        
        //minute 12
        Coroutine enemyCoroutine18 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(shaman, 60*12, 60, 0.1f, 20));
        Coroutine enemyCoroutine19 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(spirit, 60*12, 60, 0.1f, 20));
        Coroutine enemyCoroutine20 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(boidNormal, 60*12, 60, 0.1f, 20));

        Coroutine eventCoroutine17 = StartCoroutine(SpawnEvent(EventName.Boss, zombie, 60*12, 1));
        Coroutine eventCoroutine18 = StartCoroutine(SpawnEvent(EventName.Cluster, spiritCluster, 60*12, 2));
        
        //minute 13
        Coroutine enemyCoroutine21 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(shaman, 60*13, 60, 0.1f, 150));
        Coroutine enemyCoroutine22 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(spirit, 60*13, 60, 0.1f, 5020));
        
        Coroutine eventCoroutine19 = StartCoroutine(SpawnEvent(EventName.Cluster, spiritCluster, 60*13, 2));
        
        //minute 14
        Coroutine enemyCoroutine23 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(sasquets, 60*14, 60, 0.1f, 20));
        Coroutine enemyCoroutine24 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(shaman, 60*14, 60, 0.1f, 20));

        Coroutine eventCoroutine20 = StartCoroutine(SpawnEvent(EventName.Boss, zombie, 60*14, 1));
        
        //minute 15
        Coroutine enemyCoroutine25 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(sasquets, 60*15, 60, 0.1f, 100));
        Coroutine enemyCoroutine26 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(shaman, 60*15, 60, 0.1f, 100));
        Coroutine enemyCoroutine27 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(mushroom, 60*15, 60, 0.1f, 100));
        
        Coroutine eventCoroutine21 = StartCoroutine(SpawnEvent(EventName.Boss, testingWobble, 60*15, 1));
        Coroutine eventCoroutine38 = StartCoroutine(SpawnEvent(EventName.Circle, smallplant, 60*15, 1));
        
        //minute 16
        Coroutine enemyCoroutine28 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(testingWobble, 60*16, 60, 0.1f, 100));
        Coroutine enemyCoroutine29 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(mushroom, 60*16, 60, 0.1f, 200));
        
        Coroutine eventCoroutine22 = StartCoroutine(SpawnEvent(EventName.Boss, zombie, 60*16, 1));
        
        //minute 17
        Coroutine enemyCoroutine30 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(testingWobble, 60*17, 60, 0.1f, 20));
        
        //minute 18
        Coroutine enemyCoroutine31 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(testingWobble, 60*18, 60, 0.1f, 60));
        Coroutine enemyCoroutine32 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(mushroom, 60*18, 60, 0.1f, 60));
        
        Coroutine eventCoroutine23 = StartCoroutine(SpawnEvent(EventName.Boss, zombie, 60*18, 1));
        
        //minute 19
        Coroutine enemyCoroutine33 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(testingWobble, 60*19, 60, 0.1f, 100));
        Coroutine enemyCoroutine34 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(mushroom, 60*19, 60, 0.1f, 100));

        //minute 20
        Coroutine enemyCoroutine35 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(testingWobble, 60*20, 60, 0.1f, 100));
        Coroutine enemyCoroutine36 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(mushroom, 60*20, 60, 0.1f, 100));
        Coroutine enemyCoroutine37 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(sasquets, 60*20, 60, 0.1f, 100));
        
        Coroutine eventCoroutine24 = StartCoroutine(SpawnEvent(EventName.Boss, testingWobble, 60*20, 1));
        Coroutine eventCoroutine25 = StartCoroutine(SpawnEvent(EventName.Cluster, spiritCluster, 60*20, 2));
        Coroutine eventCoroutine26 = StartCoroutine(SpawnEvent(EventName.Cluster, spiritCluster, 60*20, 2));
        
        //minute 21
        Coroutine enemyCoroutine38 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(smallplant, 60*21, 60, 0.1f, 300));
        
        Coroutine eventCoroutine27 = StartCoroutine(SpawnEvent(EventName.Boss, testingWobble, 60*21, 1));
        Coroutine eventCoroutine28 = StartCoroutine(SpawnEvent(EventName.Boss, zombie, 60*21, 1));
        
        //minute 22
        Coroutine enemyCoroutine39 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(smallplant, 60*22, 60, 0.1f, 200));
        Coroutine enemyCoroutine40 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(testingWobble, 60*22, 60, 0.1f, 200));
        
        Coroutine eventCoroutine29 = StartCoroutine(SpawnEvent(EventName.Boss, zombie, 60*22, 1));
        
        //minute 23
        Coroutine enemyCoroutine41 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(smallplant, 60*23, 60, 0.1f, 300));
        Coroutine enemyCoroutine42 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(testingWobble, 60*23, 60, 0.1f, 300));

        
        Coroutine eventCoroutine30 = StartCoroutine(SpawnEvent(EventName.Boss, zombie, 60*23, 1));
        
        //minute 24
        Coroutine enemyCoroutine43 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(smallplant, 60*24, 60, 0.1f, 300));
        Coroutine enemyCoroutine44 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(testingWobble, 60*24, 60, 0.1f, 300));

        
        Coroutine eventCoroutine31 = StartCoroutine(SpawnEvent(EventName.Boss, testingWobble, 60*24, 1));
        
        //minute 25
        Coroutine enemyCoroutine45 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(testingWobble, 60*25, 60, 0.1f, 100));
        
        Coroutine eventCoroutine39 = StartCoroutine(SpawnEvent(EventName.Circle, smallplant, 60*25, 6));
        
        //minute 26
        Coroutine enemyCoroutine46 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(smallplant, 60*26, 60, 0.1f, 150));
        Coroutine enemyCoroutine47 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(testingWobble, 60*26, 60, 0.1f, 150));
        
        //minute 27
        Coroutine enemyCoroutine48 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(mushroom, 60*27, 60, 0.1f, 600));
        Coroutine enemyCoroutine49 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(testingWobble, 60*27, 60, 0.1f, 300));
        
        Coroutine eventCoroutine32 = StartCoroutine(SpawnEvent(EventName.Boss, zombie, 60*27, 1));
        Coroutine eventCoroutine33 = StartCoroutine(SpawnEvent(EventName.Cluster, spiritCluster, 60*27, 2));
        Coroutine eventCoroutine34 = StartCoroutine(SpawnEvent(EventName.Cluster, spiritCluster, 60*27, 2));
        
        //minute 28
        Coroutine enemyCoroutine50 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(turtle, 60*28, 60, 0.1f, 300));
        Coroutine enemyCoroutine51 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(sasquets, 60*28, 60, 0.1f, 300));

        //minute 29
        Coroutine enemyCoroutine52 = StartCoroutine(SpawnEnemyStartingAtTimeForDurationAtIntervalsAndAmounts(sasquets, 60*29, 60, 0.1f, 600));
        
        Coroutine eventCoroutine35 = StartCoroutine(SpawnEvent(EventName.Boss, zombie, 60*29, 1));
        Coroutine eventCoroutine36 = StartCoroutine(SpawnEvent(EventName.Cluster, spiritCluster, 60*29, 2));
        Coroutine eventCoroutine37 = StartCoroutine(SpawnEvent(EventName.Cluster, spiritCluster, 60*29, 2));
        
        //minute 30
        //idk kill player or rescue or something
        
        
        coroutineList.Add(enemyCoroutine0);
        coroutineList.Add(enemyCoroutine1);
        coroutineList.Add(enemyCoroutine2);
        coroutineList.Add(enemyCoroutine3);
        coroutineList.Add(enemyCoroutine4);
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
        coroutineList.Add(enemyCoroutine21);
        coroutineList.Add(enemyCoroutine22);
        coroutineList.Add(enemyCoroutine23);
        coroutineList.Add(enemyCoroutine24);
        coroutineList.Add(enemyCoroutine25);
        coroutineList.Add(enemyCoroutine26);
        coroutineList.Add(enemyCoroutine27);
        coroutineList.Add(enemyCoroutine28);
        coroutineList.Add(enemyCoroutine29);
        coroutineList.Add(enemyCoroutine30);
        coroutineList.Add(enemyCoroutine31);
        coroutineList.Add(enemyCoroutine32);
        coroutineList.Add(enemyCoroutine33);
        coroutineList.Add(enemyCoroutine34);
        coroutineList.Add(enemyCoroutine35);
        coroutineList.Add(enemyCoroutine36);
        coroutineList.Add(enemyCoroutine37);
        coroutineList.Add(enemyCoroutine38);
        coroutineList.Add(enemyCoroutine39);
        coroutineList.Add(enemyCoroutine40);
        coroutineList.Add(enemyCoroutine41);
        coroutineList.Add(enemyCoroutine42);
        coroutineList.Add(enemyCoroutine43);
        coroutineList.Add(enemyCoroutine44);
        coroutineList.Add(enemyCoroutine45);
        coroutineList.Add(enemyCoroutine46);
        coroutineList.Add(enemyCoroutine47);
        coroutineList.Add(enemyCoroutine48);
        coroutineList.Add(enemyCoroutine49);
        coroutineList.Add(enemyCoroutine50);
        coroutineList.Add(enemyCoroutine51);
        coroutineList.Add(enemyCoroutine52);
        
        
        
        coroutineList.Add(eventCoroutine0);
        coroutineList.Add(eventCoroutine1);
        coroutineList.Add(eventCoroutine2);
        coroutineList.Add(eventCoroutine3);
        coroutineList.Add(eventCoroutine4);
        coroutineList.Add(eventCoroutine5);
        coroutineList.Add(eventCoroutine6);
        coroutineList.Add(eventCoroutine7);
        coroutineList.Add(eventCoroutine8);
        coroutineList.Add(eventCoroutine9);
        coroutineList.Add(eventCoroutine10);
        coroutineList.Add(eventCoroutine11);
        coroutineList.Add(eventCoroutine12);
        coroutineList.Add(eventCoroutine13);
        coroutineList.Add(eventCoroutine14);
        coroutineList.Add(eventCoroutine15);
        coroutineList.Add(eventCoroutine16);
        coroutineList.Add(eventCoroutine17);
        coroutineList.Add(eventCoroutine18);
        coroutineList.Add(eventCoroutine19);
        coroutineList.Add(eventCoroutine20);
        coroutineList.Add(eventCoroutine21);
        coroutineList.Add(eventCoroutine22);
        coroutineList.Add(eventCoroutine23);
        coroutineList.Add(eventCoroutine24);
        coroutineList.Add(eventCoroutine25);
        coroutineList.Add(eventCoroutine26);
        coroutineList.Add(eventCoroutine27);
        coroutineList.Add(eventCoroutine28);
        coroutineList.Add(eventCoroutine29);
        coroutineList.Add(eventCoroutine30);
        coroutineList.Add(eventCoroutine31);
        coroutineList.Add(eventCoroutine32);
        coroutineList.Add(eventCoroutine33);
        coroutineList.Add(eventCoroutine34);
        coroutineList.Add(eventCoroutine35);
        coroutineList.Add(eventCoroutine36);
        coroutineList.Add(eventCoroutine37);
        coroutineList.Add(eventCoroutine38);
        coroutineList.Add(eventCoroutine39);
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
