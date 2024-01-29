using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    public Image image;
    public float slideSpeed = 100f;
    public float displayDuration = 2f;
    public List<Achievement> achievementQueue = new List<Achievement>();
    
    private RectTransform rectTransform;
    private Vector3 originalPosition;
    public bool coroutineRunning = false;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        
        Invoke(nameof(SubscribeToEvents), 1f);
    }

    private void Update()
    {
        if (!coroutineRunning)
        {
            if (achievementQueue.Count > 0)
            {
                Achievement nextAchievement = achievementQueue[0];
        
                achievementText.text = "Unlocked: " + nextAchievement.name;
                achievementDesc.text = nextAchievement.description;
                image.sprite = Resources.Load<Sprite>(nextAchievement.imagePath);
                StartCoroutine("SlideInAndOut");
        
                // Remove the displayed achievement from the queue
                achievementQueue.RemoveAt(0);
            }
        }
    }

    private void SubscribeToEvents()
    {
        GameManager.instance.GetComponent<AchievementManager>().OnAchievementUnlocked += DisplayAchievement;
    }

    private void DisplayAchievement(Achievement achievement)
    {
        Debug.Log("Achievement added to queue: " + achievement.name);
        achievementQueue.Add(achievement);
    }
    
    private IEnumerator SlideInAndOut()
    {
        coroutineRunning = true;
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
        coroutineRunning = false;
    }

    private void OnDestroy()
    {
        GameManager.instance.GetComponent<AchievementManager>().OnAchievementUnlocked -= DisplayAchievement;
    }
}

