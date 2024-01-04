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

    void Start()
    {
        CheckUnlockedCharacters();
    }

    private void CreateCharacterButton(string imagePath, string name, string weaponImagePath)
    {
        GameObject characterSelect = Instantiate(buttonPrefab, gridPanel);
        Image image = characterSelect.transform.Find("Image").GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>(imagePath);
        TMP_Text text = characterSelect.transform.Find("Name").GetComponent<TMP_Text>();
        text.text = name;
        Image weaponImage = characterSelect.transform.Find("Weapon").GetComponent<Image>();
        weaponImage.sprite = Resources.Load<Sprite>(weaponImagePath);
        characterSelect.GetComponent<CharacterSelect>().playButton = playbutton;
    }

    private void CheckUnlockedCharacters()
    {
        List<Achievement> achievements = DataManager.Instance.LoadData<Achievement>(DataManager.DataType.Achievement);
        foreach (var achieve in achievements)
        {
            if (achieve.unlocked)
            {
                List<Character> characters = DataManager.Instance.LoadData<Character>(DataManager.DataType.Character);
                foreach (var character in characters)
                {
                    if (character.name.Equals(achieve.name) && character.unlocked)
                    {
                        CreateCharacterButton(character.imagePath, character.name, character.weaponImagePath);
                    }
                }
            }
        }
    }
}

