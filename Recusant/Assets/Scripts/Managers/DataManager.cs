using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager
{
    private static DataManager instance;
    
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataManager();
            }
            return instance;
        }
    }

    public enum DataType
    {
        Total, Achievement, Character, Setting
    }
    
    [Serializable]
    public class DataListWrapper<T>
    {
        public List<T> dataList;
    }
    
    public void SaveData<T>(List<T> dataList, DataType dataType)
    {
        string path = GetPath(dataType);
        
        // Wrap the list in a wrapper class
        DataListWrapper<T> wrapper = new DataListWrapper<T> { dataList = dataList };
        
        string json = JsonUtility.ToJson(wrapper);
        File.WriteAllText(path, json);

        Debug.Log($"Saved {typeof(T).Name} list: {json}");
    }

    public List<T> LoadData<T>(DataType dataType) where T : class
    {
        string path = GetPath(dataType);

        if (!File.Exists(path))
        {
            Debug.Log("No data file found.");
            SetupDefaultData();
        }
        string json = File.ReadAllText(path);
        
        // Deserialize the wrapper and get the list
        DataListWrapper<T> wrapper = JsonUtility.FromJson<DataListWrapper<T>>(json);
        
        List<T> loadedDataList = wrapper.dataList;
        Debug.Log($"Loaded {typeof(T).Name} list: {json}");
        return loadedDataList;
    }
    
    private string GetPath(DataType dataType)
    {
        string path = Application.persistentDataPath + "/data.json";
        switch (dataType)
        {
            case DataType.Total:
                path = Application.persistentDataPath + "/totals.json";
                break;
            case DataType.Achievement:
                path = Application.persistentDataPath + "/achievements.json";
                break;
            case DataType.Character:
                path = Application.persistentDataPath + "/characters.json";
                break;
            case DataType.Setting:
                path = Application.persistentDataPath + "/settings.json";
                break;
            default:
                Debug.Log("Default datatype");
                break;
        }
        return path;
    }

    private void SetupDefaultData()
    {
        Debug.Log("In SetupDefaultData");
        // Set up default data for different lists
        List<Achievement> achievements = new List<Achievement>
        {
            new Achievement("", false, "First Kill", "Kill your first monster."),
            new Achievement("", false, "Five Minute Hero", "Last more than five minutes.")
            // Add more achievements as needed
        };

        List<Character> characters = new List<Character>
        {
            new Character("Sprites/Degtyarev", false, "Degtyarev", "Auto aiming pistol."),
            new Character("Sprites/Makwa", true, "Makwa", "Shotgun with aiming.")
        };

        List<Setting> settings = new List<Setting>
        {
            new Setting(true, "sound"),
            new Setting(true, "music")
        };

        List<Total> totals = new List<Total>
        {
            new Total("coinsTotal", 0)
        };
        
        SaveData(achievements, DataType.Achievement);
        SaveData(characters, DataType.Character);
        SaveData(settings, DataType.Setting);
        SaveData(totals, DataType.Total);
    }

    /*public void LoadCoinsTotal()
    {
        string path = Application.persistentDataPath + "/data.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Total loadedData = JsonUtility.FromJson<Total>(json);
            Debug.Log("Loaded coinsTotal: " + loadedData.name + " " + loadedData.value);
            dataManagerCoinsTotal = loadedData;
        }
        else
        {
            Debug.Log("No coins total found.");
        }
    }

    public void SaveCoinsTotal(int value)
    {
        Total coinsTotal = new Total();
        coinsTotal.name = "coinsTotal";
        coinsTotal.value = value;
        string json = JsonUtility.ToJson(coinsTotal);
        File.WriteAllText(Application.persistentDataPath + "/data.json", json);
    }

    public Total GetDataManagerCoinsTotal()
    {
        LoadCoinsTotal();
        if (dataManagerCoinsTotal == null)
        {
            Debug.Log("dataManagerCoinsTotal was null");
            SaveCoinsTotal(0);
        }
        return dataManagerCoinsTotal;
    }*/
}
