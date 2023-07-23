using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;

    public void SetCharacter()
    {
        Debug.Log(nameText.text + " has been chosen");
        AudioManager.instance.chosenName = nameText.text;
    }
}
