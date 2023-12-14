using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AchievementDisplay : MonoBehaviour
{
    #region Singleton
    
    public static AchievementDisplay instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of AchievementDisplay found!");
            return;
        }
        instance = this;
    }
    
    #endregion
    
    
    [SerializeField]
    public TMP_Text myTextElement;
    

    private void Start()
    {
        AchievementManager.Instance.OnAchievementUnlocked += DisplayAchievement;
    }

    private void DisplayAchievement(string achievementName)
    {
        Debug.Log("Achievement Unlocked: " + achievementName);
    }

    private void OnDestroy()
    {
        AchievementManager.Instance.OnAchievementUnlocked -= DisplayAchievement;
    }
}

