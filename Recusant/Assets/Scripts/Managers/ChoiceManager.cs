using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class ChoiceManager : MonoBehaviour
{
    public static ChoiceManager instance;
    
    //This variable should be moved to a proper instanced and non destroyable
    public string chosenName;
    public string chosenWeapon;
    public MapNames chosenMapName;

    public enum MapNames
    {
        RedForest
    }
    
    
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
    }
    
    private void OnDestroy()
    {
        // Run code here when the object is destroyed
        Debug.Log("ChoiceManager has been destroyed!");
    }
}

