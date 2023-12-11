using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonSounds : MonoBehaviour
{
    //public Button myButton;
    public bool isBackButton;
    
    void Start()
    {
        //myButton.onClick.AddListener(PlayButtonSound);
    }

    public void PlayButtonSound()
    {
        if (isBackButton)
        {
            AudioManager.instance.Play("lowsound");
        }
        else
        {
            AudioManager.instance.Play("pauseOn");
        }
        Debug.Log("Played button sound!");
    }

    public void PlayLowSound()
    {
        AudioManager.instance.Play("pauseOn");
        Debug.Log("PlayedPauseOn sound");
    }
}
