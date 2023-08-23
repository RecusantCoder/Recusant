using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private Image weaponImage;
    
    public GameObject playButton;

    public void SetCharacter()
    {
        Debug.Log(nameText.text + " has been chosen with " + weaponImage.sprite.name);
        ChoiceManager.instance.chosenName = nameText.text;
        ChoiceManager.instance.chosenWeapon = weaponImage.sprite.name;
        
        playButton.SetActive(true);
    }
}
