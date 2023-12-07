using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager
{
    private static DataManager instance;
    private string dataPath;

    private Total dataManagerCoinsTotal;

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

    public void LoadCoinsTotal()
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
    }
}
