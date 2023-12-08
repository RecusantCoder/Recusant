using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONSave : MonoBehaviour
{
    #region Singleton
    
    public static JSONSave instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            // Another instance of JSONSave exists, destroy this one
            Destroy(gameObject);
            return;
        }

        // Set this instance as the singleton instance
        instance = this;

        // Ensure the GameManager persists across scenes
        DontDestroyOnLoad(gameObject);
    }
    
    #endregion
    
    private Total coins;
    private string path = "";
    private string persistentPath = "";
    
    private void Start()
    {
        Debug.Log("JSONSAVE Start called!");
        CreateCoinsData();
        SetPaths();
        SaveData();
    }

    private void CreateCoinsData()
    {
        //coins = new Total();
    }

    private void SetPaths()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        Debug.Log("Set paths");
    }

    public void SaveData()
    {
        string savePath = path;
        Debug.Log("Saving data at " + savePath);
        string json = JsonUtility.ToJson(coins);
        Debug.Log(json);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    public void LoadData()
    {
        using StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();


        Total coins = JsonUtility.FromJson<Total>(json);
        Debug.Log(coins.ToString());
    }

    public void SetCoinsValue(int value)
    {
        coins.value = value;
    }

    public int GetCoinsValue()
    {
        return coins.value;
    }
}
