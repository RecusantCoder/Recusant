using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelSelectGridItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text descriptionText;
    [SerializeField]
    private Image levelImage;
    public Image imageSelect;
    public LevelSelectionMenu levelSelectionMenu;


    private void Start()
    {
        if (nameText.text.Equals("Red Forest"))
        {
            Highlight(true);
        }
        else
        {
            Highlight(false);
        }
    }

    public void LevelSelectGridItemSelected()
    {
        levelSelectionMenu.HighlightGridItem(this);
        if (nameText.text.Equals("Red Forest"))
        {
            ChoiceManager.instance.chosenMapName = ChoiceManager.MapNames.RedForest;
        }
        Debug.Log(nameText.text + " have been selected.");
    }

    public void SetupLevelSelectGridItem(string name, string description, string imagePath, GameObject passedLevelSelectionMenu)
    {
        nameText.text = name;
        descriptionText.text = description;
        levelImage.sprite = Resources.Load<Sprite>(imagePath);
        levelSelectionMenu = passedLevelSelectionMenu.gameObject.GetComponent<LevelSelectionMenu>();
    }
    
    public void Highlight(bool selected)
    {
        imageSelect.gameObject.SetActive(selected);
    }
}
