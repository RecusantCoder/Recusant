using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    private static AchievementManager instance;
    public static AchievementManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AchievementManager();
            }
            return instance;
        }
    }
    
    private List<Achievement> achievements;
    private int localMinutesPassed = 0;
    private int localLevelsCount = 0;

    [SerializeField] private List<Item> unlockableWeapons;
    
    
    //Events
    public event Action<Achievement> OnAchievementUnlocked;
    
    
    //Thresholds for kills
    private const int MONSTER_KILL_THRESHOLD_1 = 1;
    private const int MONSTER_KILL_THRESHOLD_2 = 1000;
    private const int MONSTER_KILL_THRESHOLD_3 = 10000;
    
    //Thresholds for time
    private const int MINUTES_PASSED_THRESHOLD_1 = 5;
    private const int MINUTES_PASSED_THRESHOLD_2 = 15;
    private const int MINUTES_PASSED_THRESHOLD_3 = 30;
    
    //Thresholds for Levels
    private const int LEVEL_THRESHOLD_1 = 5;
    private const int LEVEL_THRESHOLD_2 = 15;
    private const int LEVEL_THRESHOLD_3 = 30;
    private const int LEVEL_THRESHOLD_4 = 60;
    private const int LEVEL_THRESHOLD_5 = 90;
    
    
    
    public void Start()
    {
        // Subscribe to events or perform other initialization tasks
        // For example, you might want to subscribe to an event that triggers when a condition is met in your game.
        KillCounter.instance.OnKill += HandleMonsterKilled;
        GameManager.instance.OnMinutePassed += HandleMinutePassed;
        LevelBar.instance.PlayerHasLevelledUp += HandleLevelUp;
        Inventory.instance.ItemWasAdded += HandleInventoryChange;
        AddUnlockedWeaponsToGMWeaponsList();
    }
    
    public void UnlockAchievement(string achievementName)
    {
        List<Achievement> achievements = DataManager.Instance.LoadData<Achievement>(DataManager.DataType.Achievement);
        Achievement achievement = achievements.Find(achievement => achievement.name == achievementName);

        if (achievement != null && !achievement.unlocked)
        {
            achievement.unlocked = true;
            
            DataManager.Instance.SaveData(achievements, DataManager.DataType.Achievement);
            
            // Invoke the event to notify that an achievement is unlocked
            OnAchievementUnlocked?.Invoke(achievement);

            // Perform any other actions when an achievement is unlocked
            Debug.Log($"Achievement Unlocked: {achievementName}");
        }
    }
    
    private void HandleMonsterKilled()
    {
        List<Total> totals = DataManager.Instance.LoadData<Total>(DataManager.DataType.Total);
        Total monstersKilledTotal = totals.Find(total => total.name == "monstersKilled");

        if (monstersKilledTotal != null)
        {
            // Increment the total number of monsters killed
            monstersKilledTotal.value++;

            // Save the updated total back to DataManager
            DataManager.Instance.SaveData(totals, DataManager.DataType.Total);

            // Check conditions for unlocking achievements related to killing monsters
            if (monstersKilledTotal.value == MONSTER_KILL_THRESHOLD_1)
            {
                UnlockAchievement("Targeting_Computer");
            } else if (monstersKilledTotal.value == MONSTER_KILL_THRESHOLD_2)
            {
                UnlockAchievement("Qimmiq");
            } else if (monstersKilledTotal.value == MONSTER_KILL_THRESHOLD_3)
            {
                UnlockAchievement("Erikson");
            }
        }
    }
    
    //handles a minute passing and updates the total time played
    //and current game time played up to 31 minutes
    private void HandleMinutePassed()
    {
        List<Total> totals = DataManager.Instance.LoadData<Total>(DataManager.DataType.Total);
        Total timePlayedTotal = totals.Find(total => total.name == "timePlayedInMinutes");

        if (timePlayedTotal != null)
        {
            timePlayedTotal.value++;
            DataManager.Instance.SaveData(totals, DataManager.DataType.Total);

            localMinutesPassed++;

            // Check conditions for unlocking achievements related to killing monsters
            if (localMinutesPassed == MINUTES_PASSED_THRESHOLD_1)
            {
                UnlockAchievement("Grenade");
            } else if (localMinutesPassed == MINUTES_PASSED_THRESHOLD_2)
            {
                UnlockAchievement("LazerGun");
            } else if (localMinutesPassed == MINUTES_PASSED_THRESHOLD_3)
            {
                UnlockAchievement("Zeno");
            }
        }
    }

    // checks for relevant achievement level thresholds
    private void HandleLevelUp()
    {
        localLevelsCount++;
        if (localLevelsCount == LEVEL_THRESHOLD_1)
        {
            UnlockAchievement("Exolegs");
        } else if (localLevelsCount == LEVEL_THRESHOLD_2)
        {
            UnlockAchievement("Haurio");
        } else if (localLevelsCount == LEVEL_THRESHOLD_3)
        {
            UnlockAchievement("Body_Armor");
        } else if (localLevelsCount == LEVEL_THRESHOLD_4)
        {
            UnlockAchievement("Flashbang");
        } else if (localLevelsCount == LEVEL_THRESHOLD_5)
        {
            UnlockAchievement("Jacobi");
        }
    }

    // checks for weapon upgrade or power up related achievements
    private void HandleInventoryChange(string itemName)
    {
        int itemLevel = GameManager.instance.weaponLevelCount[itemName];
        if (itemName.Equals("Glock"))
        {
            if (itemLevel == 10)
            {
                UnlockAchievement("Molotov");
            }
        } else if (itemName.Equals("Molotov"))
        {
            if (itemLevel == 10)
            {
                UnlockAchievement("Flamethrower");
            }
        } else if (itemName.Equals("Flamethrower"))
        {
            if (itemLevel == 10)
            {
                UnlockAchievement("Guevara");
            }
        } else if (itemName.Equals("Fleshy"))
        {
            UnlockAchievement("Fleshy");
        }
    }

    public void PowerUpFound(string powerUpName)
    {
        if (powerUpName.Equals("Amulet"))
        {
            UnlockAchievement("Fulmen");
        }
    }

    //Adds unlocked weapons to the GameManager's weapons list
    public void AddUnlockedWeaponsToGMWeaponsList()
    {
        List<Achievement> achievements = DataManager.Instance.LoadData<Achievement>(DataManager.DataType.Achievement);
        foreach (var achieve in achievements)
        {
            if (achieve.unlocked)
            {
                foreach (var item in unlockableWeapons)
                {
                    if (achieve.name.Equals(item.itemName))
                    {
                        if (!GameManager.instance.weaponsList.Contains(item))
                        {
                            GameManager.instance.weaponsList.Add(item);
                            Debug.Log("purple Added " + item + " to GM Weapons List");
                        }
                    }
                }
            }
        }
    }
    
    private void OnDestroy()
    {
        Debug.Log("Achievement Manager was destroyed");
        // Unsubscribe from events to avoid memory leaks
        KillCounter.instance.OnKill -= HandleMonsterKilled;
        GameManager.instance.OnMinutePassed -= HandleMinutePassed;
        LevelBar.instance.PlayerHasLevelledUp -= HandleLevelUp;
        Inventory.instance.ItemWasAdded -= HandleInventoryChange;
    }
}
