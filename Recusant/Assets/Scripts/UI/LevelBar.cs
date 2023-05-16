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
       
        
        levels.Add(83);
        levels.Add(174);
        levels.Add(276);
        levels.Add(388);
        levels.Add(512);
        levels.Add(650);
        levels.Add(801);
        levels.Add(969);
        levels.Add(1154);
        levels.Add(1358);
        levels.Add(1584);
        levels.Add(1833);
        levels.Add(2107);
        levels.Add(2411);
        levels.Add(2746);
        levels.Add(3115);
        levels.Add(3523);
        levels.Add(3973);
        levels.Add(4470);
        levels.Add(5018);
        levels.Add(5624);
        levels.Add(6291);
        levels.Add(7028);
        levels.Add(7842);
        levels.Add(8740);
        levels.Add(9730);
        levels.Add(10824);
        levels.Add(12031);
        levels.Add(13363);
        levels.Add(14833);
        levels.Add(16456);
        levels.Add(18247);
        levels.Add(20224);
        levels.Add(22406);
        levels.Add(24815);
        levels.Add(27473);
        levels.Add(30408);
        levels.Add(33648);
        levels.Add(37224);
        levels.Add(41171);
        levels.Add(45529);
        levels.Add(50339);
        levels.Add(55649);
        levels.Add(61512);
        levels.Add(67983);
        levels.Add(75127);
        levels.Add(83014);
        levels.Add(91721);
        levels.Add(101333);
        levels.Add(111945);
        levels.Add(123660);
        levels.Add(136594);
        levels.Add(150872);
        levels.Add(166636);
        levels.Add(184040);
        levels.Add(203254);
        levels.Add(224466);
        levels.Add(247886);
        levels.Add(273742);
        levels.Add(302288);
        levels.Add(333804);
        levels.Add(368599);
        levels.Add(407015);
        levels.Add(449428);
        levels.Add(496254);
        levels.Add(547953);
        levels.Add(605032);
        levels.Add(668051);
        levels.Add(737627);
        levels.Add(814445);
        levels.Add(899257);
        levels.Add(992895);
        levels.Add(1096278);
        levels.Add(1210421);
        levels.Add(1336443);
        levels.Add(1475581);
        levels.Add(1629200);
        levels.Add(1798808);
        levels.Add(1986068);
        levels.Add(2192818);
        levels.Add(2421087);
        levels.Add(2673114);
        levels.Add(2951373);
        levels.Add(3258594);
        levels.Add(3597792);
        levels.Add(3972294);
        levels.Add(4385776);
        levels.Add(4842295);
        levels.Add(5346332);
        levels.Add(5902831);
        levels.Add(6517253);
        levels.Add(7195629);
        levels.Add(7944614);
        levels.Add(8771558);
        levels.Add(9684577);
        levels.Add(10692629);
        levels.Add(11805606);
        levels.Add(13034431);
        
    }
}
