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
        //Rebel
        CreateCharacterButton("Sprites/PlayerSprites/Guevara", "Guevara", "Sprites/WeaponSprites/Molotov");
        //Encroacher
        CreateCharacterButton("Sprites/PlayerSprites/Bourglay", "Bourglay", "Sprites/WeaponSprites/Machete");
        //Criminal
        CreateCharacterButton("Sprites/PlayerSprites/Degtyarev", "Degtyarev", "Sprites/WeaponSprites/Glock");
        //Usurper
        CreateCharacterButton("Sprites/PlayerSprites/Baratheon", "Baratheon", "Sprites/WeaponSprites/LazerGun");
        //Survivor
        CreateCharacterButton("Sprites/PlayerSprites/Makwa", "Makwa", "Sprites/WeaponSprites/Mossberg");
        //Anarchist
        CreateCharacterButton("Sprites/PlayerSprites/Zeno", "Zeno", "Sprites/WeaponSprites/Grenade");
        //Nihilist
        CreateCharacterButton("Sprites/PlayerSprites/Tesla", "Tesla", "Sprites/AchievementSprites/fulmen");
        //Traveler
        CreateCharacterButton("Sprites/PlayerSprites/Erikson", "Erikson", "Sprites/WeaponSprites/Qimmiq");
        
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
}

