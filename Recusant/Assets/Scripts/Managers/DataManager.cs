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
            new Achievement("Sprites/AchievementSprites/Exolegs", false, "Exolegs", "Reach level 5."),
            new Achievement("Sprites/AchievementSprites/haurio", false, "Haurio", "Reach level 15."),
            new Achievement("Sprites/AchievementSprites/Body_Armor", false, "Body_Armor", "Reach level 30."),
            new Achievement("Sprites/AchievementSprites/flashbang", false, "Flashbang", "Reach level 60."),
            new Achievement("Sprites/PlayerSprites/Tesla", false, "Tesla", "Reach level 90."),
            new Achievement("Sprites/AchievementSprites/grenadeFull", false, "Grenade", "Last more than 5 minutes."),
            new Achievement("Sprites/AchievementSprites/LazerGun", false, "LazerGun", "Last more than 15 minutes."),
            new Achievement("Sprites/PlayerSprites/Zeno", false, "Zeno", "Last more than 30 minutes."),
            new Achievement("Sprites/AchievementSprites/molotov", false, "Molotov", "Get a Glock to level 10."),
            new Achievement("Sprites/AchievementSprites/flamethrower", false, "Flamethrower", "Get a Molotov to level 10."),
            new Achievement("Sprites/AchievementSprites/Fleshy", false, "Fleshy", "Find a Fleshy on the map."),
            new Achievement("Sprites/AchievementSprites/fulmen", false, "Fulmen", "Pickup an Amulet."),
            new Achievement("Sprites/PlayerSprites/Guevara", false, "Guevara", "Get Flamethrower to level 10."),
            new Achievement("Sprites/AchievementSprites/computer_chip", false, "Targeting_Computer", "Kill 100 enemies."),
            new Achievement("Sprites/AchievementSprites/qimmiq2", false, "Qimmiq", "Kill 1000 enemies."),
            new Achievement("Sprites/PlayerSprites/Erikson", false, "Erikson", "Kill 10000 enemies")
            // Add more achievements as needed
        };

        List<Character> characters = new List<Character>
        {
            new Character("Sprites/PlayerSprites/Degtyarev", false, "Degtyarev", "Auto aiming pistol.", "Sprites/WeaponSprites/Glock"),
            new Character("Sprites/PlayerSprites/Makwa", true, "Makwa", "Shotgun with aiming.", "Sprites/WeaponSprites/Mossberg"),
            new Character("Sprites/PlayerSprites/Bourglay", false, "Bourglay", "Machete slices sideways.", "Sprites/WeaponSprites/Machete"),
            new Character("Sprites/PlayerSprites/Baratheon", false, "Baratheon", "Lazer pierces enemies.", "Sprites/WeaponSprites/LazerGun"),
            new Character("Sprites/PlayerSprites/Guevara",false, "Guevara","Molotov ignites enemies", "Sprites/WeaponSprites/Molotov"),
            new Character("Sprites/PlayerSprites/Zeno",false, "Zeno","Throws grenades backwards", "Sprites/WeaponSprites/Grenade"),
            new Character("Sprites/PlayerSprites/Tesla",false, "Tesla","Fulmen zaps everything", "Sprites/AchievementSprites/fulmen"),
            new Character("Sprites/PlayerSprites/Erikson",false, "Erikson","Qimmiq is good boi", "Sprites/WeaponSprites/Qimmiq")
        };

        List<Setting> settings = new List<Setting>
        {
            new Setting(true, "sound"),
            new Setting(true, "music")
        };

        List<Total> totals = new List<Total>
        {
            new Total("coinsTotal", 5),
            new Total("monstersKilled", 0),
            new Total("timePlayedInMinutes", 0)
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
