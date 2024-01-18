using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform gridPanel;
    [SerializeField] public GameObject playbutton;
    public GameObject chosenCharacterSelect;

    void Start()
    {
        CheckUnlockedCharacters();
    }

    public void ResetCharacterSelects()
    {
        foreach (Transform child in gridPanel.transform)
        {
            Destroy(child.gameObject);
        }
        CheckUnlockedCharacters();
    }

    private void CreateCharacterButton(string imagePath, string name, string weaponImagePath, bool isUnlocked)
    {
        GameObject characterSelect = Instantiate(buttonPrefab, gridPanel);
        Image image = characterSelect.transform.Find("Image").GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>(imagePath);
        TMP_Text text = characterSelect.transform.Find("Name").GetComponent<TMP_Text>();
        text.text = name;
        Image weaponImage = characterSelect.transform.Find("Weapon").GetComponent<Image>();
        weaponImage.sprite = Resources.Load<Sprite>(weaponImagePath);
        CharacterSelect characterSelectScript = characterSelect.GetComponent<CharacterSelect>();
        characterSelectScript.playButton = playbutton;
        characterSelectScript.isUnlocked = isUnlocked;
        characterSelectScript.characterSelectionManager = this;

        if (!isUnlocked)
        {
            if (name.Equals("Bourglay"))
            {
                characterSelectScript.priceText.text = "100";
            } else if (name.Equals("Degtyarev"))
            {
                characterSelectScript.priceText.text = "200";
            } else if (name.Equals("Baratheon"))
            {
                characterSelectScript.priceText.text = "300";
            }
            characterSelectScript.priceText.gameObject.SetActive(true);
        }
    }

    private void CheckUnlockedCharacters()
    {
        int count = 0;
        List<Character> characters = DataManager.Instance.LoadData<Character>(DataManager.DataType.Character);
        foreach (var character in characters)
        {
            if (character.unlocked)
            {
                count++;
                CreateCharacterButton(character.imagePath, character.name, character.weaponImagePath, character.unlocked);
            }
            else
            {
                if (character.name.Equals("Bourglay") || character.name.Equals("Degtyarev") || character.name.Equals("Baratheon"))
                {
                    count++;
                    CreateCharacterButton(character.imagePath, character.name, character.weaponImagePath, character.unlocked);
                }
            }
        }
        AdjustContentWidth(count);
    }
    
    public void HighlightGridItem(CharacterSelect characterSelect)
    {
        if (chosenCharacterSelect != null)
        {
            chosenCharacterSelect.GetComponent<CharacterSelect>().Highlight(false);
        }
        
        chosenCharacterSelect = characterSelect.gameObject;
        characterSelect.Highlight(true);
    }
    
    void AdjustContentWidth(int numOfGridCharacters)
    {
        RectTransform content = gridPanel.GetComponent<RectTransform>();
        Vector2 sizeDelta = content.sizeDelta;
        sizeDelta.x = numOfGridCharacters * 600;
        Debug.Log("sizedelta x: " + sizeDelta.x);
        content.sizeDelta = sizeDelta;
    }
}

