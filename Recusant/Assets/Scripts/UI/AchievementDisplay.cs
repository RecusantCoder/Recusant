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
    
    public TMP_Text achievementText;
    public TMP_Text achievementDesc;
    public float slideSpeed = 100f;
    public float displayDuration = 2f;
    
    private RectTransform rectTransform;
    private Vector3 originalPosition;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        
        Invoke(nameof(SubscribeToEvents), 1f);
    }

    private void SubscribeToEvents()
    {
        GameManager.instance.GetComponent<AchievementManager>().OnAchievementUnlocked += DisplayAchievement;
    }

    private void DisplayAchievement(Achievement achievement)
    {
        Debug.Log("Achievement Unlocked: " + achievement.name);
        achievementText.text = "Unlocked: " + achievement.name;
        achievementDesc.text = achievement.description;
        StartCoroutine("SlideInAndOut");
    }
    
    private IEnumerator SlideInAndOut()
    {
        // Slide in
        float timer = 0f;
        while (timer < displayDuration)
        {
            rectTransform.anchoredPosition += Vector2.down * slideSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        // Wait for a short duration
        yield return new WaitForSeconds(displayDuration);

        // Slide out
        timer = 0f;
        while (timer < displayDuration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, originalPosition, timer / displayDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        // Reset position
        rectTransform.anchoredPosition = originalPosition;
    }

    private void OnDestroy()
    {
        GameManager.instance.GetComponent<AchievementManager>().OnAchievementUnlocked -= DisplayAchievement;
    }
}

