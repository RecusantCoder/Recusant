using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform gridPanel;

    void Start()
    {
        CreateCharacterButton("Sprites/Degtyarev", "Degtyarev");
        CreateCharacterButton("Sprites/Makwa", "Makwa");
    }

    private void CreateCharacterButton(string imagePath, string name)
    {
        GameObject characterSelect = Instantiate(buttonPrefab, gridPanel);
        Image image = characterSelect.transform.Find("Image").GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>(imagePath);
        TMP_Text text = characterSelect.transform.Find("Name").GetComponent<TMP_Text>();
        text.text = name;
    }
}

