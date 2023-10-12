using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    
    public Sound[] sounds;
    public static AudioManager instance;
    
    //This variable should be moved to a proper instanced and non destroyable
    public String chosenName;
    
    private float previousMusicVolume;
    private float previousSoundVolume;

    
    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        //Put a main theme here
        Play("DrivingSong1");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
    
    public void SetMusicVolume(float volume)
    {
        Debug.Log("Music at: " + volume);
        foreach (var s in AudioManager.instance.sounds)
        {
            if (s.isMusic)
            {
                s.source.volume = volume;
                PlayerPrefs.SetFloat("volumeMusic", volume);
            }
        }
    }

    public void SetSoundVolume(float volume)
    {
        Debug.Log("Sound at: " + volume);
        foreach (var s in AudioManager.instance.sounds)
        {
            if (s == null)
            {
               Debug.Log("The sound is null"); 
            }
            if (s.isMusic == false)
            {
                if (s.source == null)
                {
                    Debug.Log("The s.source is null"); 
                }
                s.source.volume = volume;
                PlayerPrefs.SetFloat("volumeSound", volume);
            }
        }
    }
    
    private void OnDestroy()
    {
        // Run code here when the object is destroyed
        Debug.Log("AudioManager has been destroyed!");
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded for AudioManager");
        if (scene.name == "MainMenu")
        {
            // Mute all sounds except "DrivingSong1"
            SetMusicVolume(0.1f);
            SetSoundVolume(0);
            Debug.Log("SceneLoaded MainMenu AudioManager");
        }
        else
        {
            // Restore the previous audio volumes
            SetMusicVolume(0.1f);
            SetSoundVolume(0.1f);
            Debug.Log("Sceneloaded other AudioManager");
        }
    }


}
