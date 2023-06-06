using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    #region Singleton
    
    public static LevelBar instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of LevelBar found!");
            return;
        }
        instance = this;
    }
    
    #endregion
    
    public Slider slider;
    [SerializeField]
    public TMP_Text myTextElement;

    private int playerLevel;
    private float playerExperience;

    private List<float> levels;
    
    //To GameManager to show LevelUpScreen
    public delegate void LevelUpEventHandler(int newLevel);
    public static event LevelUpEventHandler OnLevelUp;

    public bool playerActionTaken = false;


    private void Start()
    {
        myTextElement.text = "LVL 0";
        
        setupLevels();
        playerLevel = 0;
        playerExperience = 0;

        SetExperience(0);
        SetMaxExperience();
        SetMinExperience();

        playerActionTaken = true;


    }

    private void Update()
    {
        if (playerExperience >= levels[playerLevel] && playerActionTaken)
        {
            playerActionTaken = false;
            FindObjectOfType<AudioManager>().Play("levelup");
            playerLevel++;
            SetMaxExperience();
            SetMinExperience();
            SetExperience(playerExperience);

            myTextElement.text = "LVL " + playerLevel;

            Debug.Log("Level up! Now Level " + playerLevel + " TotalXP: " + playerExperience);
            Debug.Log("Xp for next Level = " + levels[playerLevel]);

            // Invoke the event
            if (OnLevelUp != null)
            {
                OnLevelUp(playerLevel);
                Debug.Log("Called OnLevelUp");
            }
        }
    }


    private void OnEnable()
    {
        // Subscribe to the event only if not already subscribed
        if (!IsSubscribed(OnLevelUp))
        {
            // Subscribe to the event when this object is enabled
            GameManager.PlayerActionTakenEvent += OnPlayerActionTaken;
            // Subscribe to the level up event
            OnLevelUp += OnLevelUp;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the event only if subscribed
        if (IsSubscribed(OnLevelUp))
        {
            // Unsubscribe from the event when this object is disabled
            GameManager.PlayerActionTakenEvent -= OnPlayerActionTaken;
            // Unsubscribe from the level up event
            OnLevelUp -= OnLevelUp;
        }
    }

    private void OnPlayerActionTaken()
    {
        playerActionTaken = true;
    }

    public void SetExperience(float playerXp)
    {
        slider.value = playerXp;
    }

    public void SetMaxExperience()
    {
        slider.maxValue = levels[playerLevel];
    }
    
    public void SetMinExperience()
    {
        if (playerLevel == 0)
        {
            slider.minValue = 0;
        }
        else
        {
            //slider.minValue = levels[playerLevel] - levels[playerLevel - 1];
            slider.minValue = levels[playerLevel - 1];
        }

        
    }

    public void AddExperience(float xp)
    {
        playerExperience += xp;
        slider.value = playerExperience;
        Debug.Log("Added " + xp + " TotalXP: " + playerExperience);
        Debug.Log("Min XP: " + slider.minValue + " Max XP: " + slider.maxValue);
        if (playerLevel >= 2)
        {
            Debug.Log("MinXPValue: " + levels[playerLevel-1]  + " MaxXPValue: " + levels[playerLevel]);
        }
        
    }
    
    private bool IsSubscribed(LevelUpEventHandler handler)
    {
        if (OnLevelUp != null)
        {
            // Get the invocation list of the event
            Delegate[] subscribers = OnLevelUp.GetInvocationList();

            // Iterate over the invocation list and check if the given handler is already subscribed
            foreach (Delegate subscriber in subscribers)
            {
                if (subscriber.Equals(handler))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void setupLevels()
    {
        levels = new List<float>();

        int modulus = 10;

        for (int i = 1; i < 201; i++)
        {
            if (i%modulus == 0)
            {
                modulus += 10;
            }
            float threshold = i * modulus;
            levels.Add(threshold);
            //Debug.Log(threshold + " " + i);
        }
    }
}
