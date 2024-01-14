using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class LevelSelectionMenu : MonoBehaviour
{
    public Transform gridContainer;
    public GameObject levelSelectGridItem;
    public GameObject chosenGridItem;
    
    // Start is called before the first frame update
    void Start()
    {
        CreateGridItem("Sprites/UiSprites/RedForest", "Red Forest", "A cursed forest filled with all manner of monsters.");
    }
    
    private void CreateGridItem(string imagePath, string name, string description)
    {
        GameObject newGridItem = Instantiate(levelSelectGridItem, gridContainer);
        newGridItem.GetComponent<LevelSelectGridItem>().SetupLevelSelectGridItem(name, description, imagePath, gameObject);
    }
    
    
    public void HighlightGridItem(LevelSelectGridItem gridItem)
    {
        if (chosenGridItem != null)
        {
            chosenGridItem.GetComponent<LevelSelectGridItem>().Highlight(false);
        }
        
        chosenGridItem = gridItem.gameObject;
        gridItem.Highlight(true);
    }
}
