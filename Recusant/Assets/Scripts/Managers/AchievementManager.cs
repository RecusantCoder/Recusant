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
    
    
    //Events
    public event Action<string> OnAchievementUnlocked;
    
    
    //Thresholds
    private const int MONSTER_KILL_THRESHOLD_1 = 1;
    
    
    public void Start()
    {
        // Subscribe to events or perform other initialization tasks
        // For example, you might want to subscribe to an event that triggers when a condition is met in your game.
        KillCounter.instance.OnKill += HandleMonsterKilled;
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
            OnAchievementUnlocked?.Invoke(achievementName);

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
            CheckMonsterKillingAchievements(monstersKilledTotal.value);
        }
    }

    private void CheckMonsterKillingAchievements(int totalMonstersKilled)
    {
        if (totalMonstersKilled >= MONSTER_KILL_THRESHOLD_1)
        {
            UnlockAchievement("First Kill");
        }
        
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events to avoid memory leaks
        KillCounter.instance.OnKill -= HandleMonsterKilled;
    }
}
