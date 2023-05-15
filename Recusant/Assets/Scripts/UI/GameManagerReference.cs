using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerReference : MonoBehaviour
{
    private GameManager gameManager;
    public Button button;

    
    private void Awake()
    {
        gameManager = GameManager.instance;
        
        try
        {
        button = GetComponent<Button>();
        
        button.onClick.AddListener(gameManager.MainMenu);

        

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    private void Start()
    {
        // Find and assign the initial reference to the Game Manager
        gameManager = GameManager.instance;
        
    }

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update the reference to the Game Manager when a new scene is loaded
        gameManager = GameManager.instance;
    }

    // Any other methods or logic related to the GameManager reference can be added here
}

