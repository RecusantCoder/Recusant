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
    
    public float previousMusicVolume;
    public float previousSoundVolume;

    public GameObject currentMusic;
    public bool firstStart;

    
    
    private void Awake()
    {
        // If an instance already exists, destroy the new one
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Otherwise, set this as the instance and make it persistent
        instance = this;
        DontDestroyOnLoad(gameObject);

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
        //Play("DrivingSong1");
    }

    public void Play(string name)
    {
        if (name.Equals("DrivingSong1"))
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            s.source.Play();
        }
        else
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            GameObject soundGameObject = new GameObject("Sound_" + name);
            soundGameObject.transform.SetParent(transform);
            if (soundGameObject.name.Contains("RecusantTheme"))
            {
                currentMusic = soundGameObject;
                Debug.Log("assigned current music");
            }
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
    
            // Set the properties of the AudioSource
            audioSource.clip = s.clip;
            if (soundGameObject.name.Contains("RecusantTheme"))
            {
                Debug.Log("assigned music volume to recusant theme: " + PlayerPrefs.GetFloat("volumeMusic"));
                audioSource.volume = PlayerPrefs.GetFloat("volumeMusic");
            }
            else
            {
                audioSource.volume = PlayerPrefs.GetFloat("volumeSound");
            }
            audioSource.pitch = s.pitch;
            audioSource.loop = s.loop;

            if (FindExistingSoundInChildren(audioSource.clip.name))
            {
                
            }
            else
            {
                // Play the sound
                audioSource.Play();
            }
            

            // Destroy the GameObject after the sound has finished playing
            if (!soundGameObject.name.Contains("RecusantTheme"))
            {
                Destroy(soundGameObject, s.clip.length);
            }
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        Debug.Log("Music at: " + volume);
        if (AudioManager.instance.currentMusic != null)
        {
            AudioManager.instance.currentMusic.GetComponent<AudioSource>().volume = volume;
            Debug.Log("Changed recusant theme volume");
        }

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
            if (!s.isMusic)
            {
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
        
        if (PlayerPrefs.HasKey("volumeMusic"))
        {
            // The "volumeMusic" key exists in PlayerPrefs
            float existingVolume = PlayerPrefs.GetFloat("volumeMusic");
            Debug.Log("Existing volume for 'volumeMusic': " + existingVolume);
        }
        else
        {
            // The "volumeMusic" key does not exist in PlayerPrefs
            Debug.Log("'volumeMusic' key not found in PlayerPrefs");
        }
        
        // Store previous volumes
        previousMusicVolume = PlayerPrefs.GetFloat("volumeMusic", 0.1f);
        previousSoundVolume = PlayerPrefs.GetFloat("volumeSound", 0.1f);
        
        SetMusicVolume(previousMusicVolume);
        SetSoundVolume(previousSoundVolume);
        
        Debug.Log("music volume: " + previousMusicVolume + " sound volume: " + previousSoundVolume);
        
        // Check the current scene and play music accordingly
        switch (scene.name)
        {
            case "Level1":
                Play("RecusantTheme");
                break;
            case "MainMenu":
                if (!instance.firstStart)
                {
                    Play("Intro");
                    instance.firstStart = true;
                }
                Destroy(currentMusic);
                break;
            default:
                break;
            // Add more cases for other scenes as needed
        }
    }

    private bool FindExistingSoundInChildren(string audioClipName)
    {
        int numOfSounds = 0;
        
        foreach (Transform child in transform)
        {
            AudioSource childAudioSource = child.GetComponent<AudioSource>();
            if (childAudioSource != null && childAudioSource.clip != null && childAudioSource.clip.name == audioClipName)
            {
                numOfSounds++;
                if (numOfSounds > 1)
                {
                    Debug.Log("childAudioSource.clip.name : " + childAudioSource.clip.name + " and audioClipName: " + audioClipName);
                    return true;
                }
            }
        }

        // Return null if no existing sound is found
        return false;
    }

}
