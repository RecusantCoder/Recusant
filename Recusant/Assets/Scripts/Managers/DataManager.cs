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
        Total, Achievement, Character, Setting, Upgrade
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
        Debug.Log("Persistent Data Path: " + path);
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
            case DataType.Upgrade:
                path = Application.persistentDataPath + "/upgrades.json";
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
            new Total("coinsTotal", 5),
            new Total("monstersKilled", 0)
        };

        List<Upgrade> upgrades = new List<Upgrade>
        {
            new Upgrade("Sprites/UpgradeSprites/Body_Armor", "Armor", "Armor up by 2 per lvl", new int[] { 2, 4, 6, 8, 10 }, 1),
            new Upgrade("Sprites/UpgradeSprites/coin", "Value", "Coin value up by 10% per lvl.", new int[] { 2, 4, 6, 8, 10 }, 1),
            new Upgrade("Sprites/UpgradeSprites/computer_chip", "Damage", "DMG up by 5 per lvl.", new int[] { 2, 4, 6, 8, 10 }, 1),
            new Upgrade("Sprites/UpgradeSprites/Exolegs", "Speed", "Speed up by 20% per lvl.", new int[] { 2, 4, 6, 8, 10 }, 1),
            new Upgrade("Sprites/UpgradeSprites/haurio", "Attraction", "Pickup Radius up by 20% per lvl", new int[] { 2, 4, 6, 8, 10 }, 1)
        };
        
        SaveData(achievements, DataType.Achievement);
        SaveData(characters, DataType.Character);
        SaveData(settings, DataType.Setting);
        SaveData(totals, DataType.Total);
        SaveData(upgrades, DataType.Upgrade);
    }
    
}
