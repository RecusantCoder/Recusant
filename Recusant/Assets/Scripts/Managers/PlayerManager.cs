using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    
    #region Singleton
    
    public static PlayerManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of PlayerManager found!");
            return;
        }
        instance = this;
        
        player = GameObject.FindWithTag("Player");
    }
    
    #endregion
    

    public GameObject player;

    private void Update()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void KillPlayer()
    {
        Debug.Log("Player Died");
        GameManager.instance.EndGame();
    }
    
    private void OnDestroy()
    {
        Debug.Log("PlayerManager is being destroyed");
        // Perform any necessary cleanup here
    }
}
