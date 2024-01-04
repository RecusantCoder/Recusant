using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class MenuPanel : MonoBehaviour
{
    public TMP_Text selectedUpgradePrice;
    public TMP_Text selectedDescription;
    public GameObject gridItem;
    public Transform gridContainer;
    public GameObject chosenGridItem;
    public GameObject mainMenu;
    
    void Start()
    {
        LoadDataAndCreateGridItems(DataManager.DataType.Upgrade);
    }

    public void LoadDataAndCreateGridItems(DataManager.DataType dataType)
    {
        DataManager dataManager = DataManager.Instance;

        if (dataType.Equals(DataManager.DataType.Upgrade))
        {
            List<Upgrade> loadedData = dataManager.LoadData<Upgrade>(dataType);
            
            foreach (var upgrade in loadedData)
            {
                CreateGridItem(upgrade.imagePath, upgrade.name, upgrade);
            }
        }
    }

    public void LoadCoinsAndMinusFromTotal(int value)
    {
        try
        {
            Debug.Log("Running LoadCoinsAndMinusFromTotal");
            DataManager dataManager = DataManager.Instance;
            List<Total> loadedData = dataManager.LoadData<Total>(DataManager.DataType.Total);
            // Find the specific Total object by name
            Total totalToModify = loadedData.Find(t => t.name == "coinsTotal");
            
            if (totalToModify != null)
            {
                // Modify the value
                totalToModify.value -= value;

                // Save the modified list
                dataManager.SaveData(loadedData, DataManager.DataType.Total);

                Debug.Log($"Minused coins value");
            }
            else
            {
                Debug.Log($"coinsTotal not found.");
            }
        }
        catch (Exception e)
        {
            Debug.Log("Catch: " + e);
        }
    }

    private void CreateGridItem(string imagePath, string name)
    {
        GameObject newGridItem = Instantiate(gridItem, gridContainer);
        Image image = newGridItem.transform.Find("Image").GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>(imagePath);
        TMP_Text text = newGridItem.transform.Find("Name").GetComponent<TMP_Text>();
        text.text = name;
    }
    
    private void CreateGridItem(string imagePath, string name, Upgrade currentUpgrade)
    {
        GameObject newGridItem = Instantiate(gridItem, gridContainer);
        Image image = newGridItem.transform.Find("Image").GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>(imagePath);
        TMP_Text text = newGridItem.transform.Find("Name").GetComponent<TMP_Text>();
        text.text = name;
        TMP_Text levelText = newGridItem.transform.Find("Level").GetComponent<TMP_Text>();
        levelText.text = "Level " + currentUpgrade.rank;
        
        GridItem gridItemScript = newGridItem.GetComponent<GridItem>();
        gridItemScript.upgrade = currentUpgrade;
        gridItemScript.selectedDescription = selectedDescription;
        gridItemScript.selectedUpgradePrice = selectedUpgradePrice;
        gridItemScript.menuPanel = gameObject.GetComponent<MenuPanel>();
        gridItemScript.GridItemSelected();
    }
    
    public void BuyUpgrade()
    {
        GridItem gridItem = chosenGridItem.GetComponent<GridItem>();
        MainMenu mainMenuScript = mainMenu.GetComponent<MainMenu>();
        
        int currentTotal = int.Parse(mainMenuScript.coinDisplayText.text);
        int upgradePrice = gridItem.GetCurrentPrice();
        if (currentTotal - upgradePrice >= 0)
        {
            if (gridItem.upgrade.rank != gridItem.upgrade.prices.Length)
            {
                Debug.Log("old upgrade rank!: " + gridItem.upgrade.rank);
                gridItem.upgrade.rank++;
                Debug.Log("new upgrade rank!: " + gridItem.upgrade.rank);
                gridItem.UpdateGridItemUI();
                
                //call the minus and save after checking the total
                //find main menu script and load the coins method
                
                LoadCoinsAndMinusFromTotal(upgradePrice);
                mainMenuScript.LoadCoinCount();
                LoadUpgradesAndModifyAndSave(gridItem.upgrade.name, gridItem.upgrade.rank);
                Debug.Log("Minused " + upgradePrice + " from " + currentTotal + " and saved new price and updated the ui to reflect the new total");
                
                AudioManager.instance.Play("Purchase");
            }
        }
    }

    public void LoadUpgradesAndModifyAndSave(string upgradeName, int upgradeRank)
    {
        try
        {
            Debug.Log("Running LoadUpgradesAndModifyAndSave");
            DataManager dataManager = DataManager.Instance;
            List<Upgrade> loadedData = dataManager.LoadData<Upgrade>(DataManager.DataType.Upgrade);
            // Find the specific Total object by name
            Upgrade upgradeToModify = loadedData.Find(t => t.name == upgradeName);
            
            if (upgradeToModify != null)
            {
                // Modify the value
                upgradeToModify.rank = upgradeRank;

                // Save the modified list
                dataManager.SaveData(loadedData, DataManager.DataType.Upgrade);

                Debug.Log($"Updated {upgradeName} to rank {upgradeRank}");
            }
            else
            {
                Debug.Log($"{upgradeName} not found.");
            }
        }
        catch (Exception e)
        {
            Debug.Log("Catch: " + e);
        }
    }

    public void HighlightGridItem(GridItem gridItem)
    {
        if (chosenGridItem != null)
        {
            chosenGridItem.GetComponent<GridItem>().Highlight(false);
        }
        
        chosenGridItem = gridItem.gameObject;
        gridItem.Highlight(true);
    }
    
}
