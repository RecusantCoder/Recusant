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

    private void OnEnable()
    {
        //LoadCoinCount();
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        
        Application.Quit();
        Debug.Log("QUIT!");
    }

    private void NewLoadCoinCount()
    {
        try
        {
            Debug.Log("New load coin count");
            JSONSave.instance.LoadData();
            coinDisplayText.text = JSONSave.instance.GetCoinsValue() + "";
        }
        catch (Exception e)
        {
            Debug.Log("error in NewLoadCoinCount: " + e);
        }
    }

    private void LoadCoinCount()
    {
        coinDisplayText.text = DataManager.Instance.GetDataManagerCoinsTotal().value + "";
        Debug.Log("Loaded Coin Count.");
    }

    private void MinusLocalCoinCount(int value)
    {
        localCoinCount -= value;
        coinDisplayText.text = localCoinCount + "";
    }
    
    /*private void SaveToDataManager()
    {
        bool noTargetFound = false;
        try
        {
            Debug.Log("Running SaveToDataManager in MainMenu");
            DataManager.Instance.LoadData();
            List<Total> totals = DataManager.Instance.GetData<Total>("totals");
            if (totals.Count == 0)
            {
                Debug.Log("totals.count is 0");
                noTargetFound = true;
            }
            foreach (var total in totals)
            {
                if (total.name == "coins")
                {
                    total.value = localCoinCount;
                }
                else
                {
                    noTargetFound = true;
                }
            }

            if (noTargetFound)
            {
                //Total newTotal = new Total(0, "coins");
                //totals.Add(newTotal);
                Debug.Log("No target found.");
            }
            
            DataManager.Instance.SetData("totals", totals);
            DataManager.Instance.SaveData();
        }
        catch (Exception e)
        {
            Debug.Log("Catch: " + e);
        }
    }*/
    
}
