using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public void SetMusicVolume(float volume)
    {
        Debug.Log("Music Vol: " + volume);
    }

    public void SetSoundVolume(float volume)
    {
        Debug.Log("Sound Vol: " + volume);
    }
}
