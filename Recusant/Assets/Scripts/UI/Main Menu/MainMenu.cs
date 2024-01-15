using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    private int localCoinCount;
    public TMP_Text coinDisplayText;

    private void Start()
    {
        LoadCoinCount();
    }
    
    public void PlayGame()
    {
        if (ChoiceManager.instance.chosenMapName != null)
        {
            if (ChoiceManager.instance.chosenMapName == ChoiceManager.MapNames.RedForest)
            {
                Debug.Log(ChoiceManager.instance.chosenMapName + " yeet");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    public void QuitGame()
    {
        
        Application.Quit();
        Debug.Log("QUIT!");
    }

    public void LoadCoinCount()
    {
        DataManager dataManager = DataManager.Instance;
        List<Total> loadedData = dataManager.LoadData<Total>(DataManager.DataType.Total);
        foreach (var total in loadedData)
        {
            if (total.name.Equals("coinsTotal"))
            {
                coinDisplayText.text = total.value + "";
            }
        }
        Debug.Log("Loaded Coin Count.");
    }

    private void MinusLocalCoinCount(int value)
    {
        localCoinCount -= value;
        coinDisplayText.text = localCoinCount + "";
    }
    
}
