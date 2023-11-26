using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager
{
    private static DataManager instance;

    private Dictionary<string, object> dataLists;
    private string dataPath;

    private DataManager()
    {
        dataLists = new Dictionary<string, object>();
        dataPath = Application.persistentDataPath + "/data.json";
    }

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

    public void LoadData()
    {
        if (File.Exists(dataPath))
        {
            Debug.Log("Data exists.");
            string json = File.ReadAllText(dataPath);
            JsonUtility.FromJsonOverwrite(json, dataLists);
        }
        else
        {
            Debug.Log("Setting up Default Data.");
            SetupDefaultData();
            SaveData();
        }
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(dataLists);
        File.WriteAllText(dataPath, json);
    }

    private void SetupDefaultData()
    {
        Debug.Log("In Setup");
        // Set up default data for different lists
        dataLists["achievements"] = new List<Achievement>
        {
            new Achievement("", false, "First Kill", "Kill your first monster."),
            new Achievement("", false, "Five Minute Hero", "Last more than five minutes.")
            // Add more achievements as needed
        };

        dataLists["characters"] = new List<Character>
        {
            new Character("Sprites/Degtyarev", false, "Degtyarev", "Auto aiming pistol."),
            new Character("Sprites/Makwa", true, "Makwa", "Shotgun with aiming.")
        };

        dataLists["settings"] = new List<Setting>
        {
            new Setting(true, "sound"),
            new Setting(true, "music")
        };

        dataLists["totals"] = new List<Total>
        {
            new Total(0, "coins")
        };

        // Add more lists as needed
    }

    public List<T> GetData<T>(string listKey)
    {
        if (dataLists.ContainsKey(listKey) && dataLists[listKey] is List<T>)
        {
            return (List<T>)dataLists[listKey];
        }

        // Return an empty list if the key doesn't exist or if the type is incorrect
        return new List<T>();
    }

    public void SetData<T>(string listKey, List<T> newData)
    {
        if (dataLists.ContainsKey(listKey))
        {
            dataLists[listKey] = newData;
        }
        else
        {
            dataLists.Add(listKey, newData);
        }
    }
}
