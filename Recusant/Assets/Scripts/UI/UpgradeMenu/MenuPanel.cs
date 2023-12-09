using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuPanel : MonoBehaviour
{
    public TMP_Text selectedUpgradePrice;
    public TMP_Text selectedDescription;
    public GameObject gridItem;
    public Transform gridContainer;
    
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
        
        GridItem gridItemScript = newGridItem.GetComponent<GridItem>();
        gridItemScript.upgrade = currentUpgrade;
        gridItemScript.selectedDescription = selectedDescription;
        gridItemScript.selectedUpgradePrice = selectedUpgradePrice;
    }
}
