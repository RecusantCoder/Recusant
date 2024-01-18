using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class AchievementsPanel : MonoBehaviour
{
    public GameObject gridItem;
    public Transform gridContainer;
    
    void Start()
    {
        LoadDataAndCreateGridItems(DataManager.DataType.Achievement);
    }

    public void LoadDataAndCreateGridItems(DataManager.DataType dataType)
    {
        
        DataManager dataManager = DataManager.Instance;
        
        if (dataType.Equals(DataManager.DataType.Achievement))
        {
            List<Achievement> loadedData = dataManager.LoadData<Achievement>(dataType);
            
            AdjustContentHeight(loadedData.Count);
            
            foreach (var achievement in loadedData)
            {
                CreateGridItem(achievement);
            }
        }
    }
    
    private void CreateGridItem(Achievement achievement)
    {
        GameObject newGridItem = Instantiate(gridItem, gridContainer);
        Image image1 = newGridItem.transform.Find("UnlockedImage").GetComponent<Image>();
        image1.sprite = achievement.unlocked ? Resources.Load<Sprite>("Sprites/AchievementSprites/Checked") : Resources.Load<Sprite>("Sprites/AchievementSprites/Unchecked");
        Image image2 = newGridItem.transform.Find("ItemImage").GetComponent<Image>();
        image2.sprite = Resources.Load<Sprite>(achievement.imagePath);
        TMP_Text text = newGridItem.transform.Find("Description").GetComponent<TMP_Text>();
        text.text = achievement.description;
    }
    
    void AdjustContentHeight(int numOfGridItems)
    {
        RectTransform content = gridContainer.GetComponent<RectTransform>();
        Vector2 sizeDelta = content.sizeDelta;
        sizeDelta.y = numOfGridItems * 200;
        Debug.Log("sizedelta y: " + sizeDelta.y);
        content.sizeDelta = sizeDelta;
    }
    
    
}

