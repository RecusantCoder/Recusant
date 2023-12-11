using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonSounds : MonoBehaviour
{
    public Button myButton;
    public bool isBackButton;
    
    void Start()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(PlayButtonSound);
    }

    public void PlayButtonSound()
    {
        if (isBackButton)
        {
            AudioManager.instance.Play("Back");
        }
        else
        {
            AudioManager.instance.Play("Next");
        }
        Debug.Log("Played button sound!");
    }
    
}
