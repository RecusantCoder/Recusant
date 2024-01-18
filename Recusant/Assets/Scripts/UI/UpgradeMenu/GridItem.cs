using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridItem : MonoBehaviour
{
    public Image gridItemBackground;
    public Image imageSelect;
    
    public Upgrade upgrade;
    
    public TMP_Text selectedUpgradePrice;
    public TMP_Text selectedDescription;

    public MenuPanel menuPanel;

    private void Start()
    {
        Highlight(false);
    }

    public void GridItemSelected()
    {
        selectedDescription.text = upgrade.description;
        selectedUpgradePrice.text = GetCurrentPrice().ToString();
        menuPanel.HighlightGridItem(this);
        
        StartCoroutine(ButtonClickEffect());
    }
    
    private IEnumerator ButtonClickEffect()
    {
        // Change button color to a pressed state
        gridItemBackground.color = Color.yellow;
        // Wait for a short duration
        yield return new WaitForSeconds(0.2f);
        // Reset the button color to its original state
        gridItemBackground.color = Color.white;
    }

    public int GetCurrentPrice()
    {
        Debug.Log("GetCurrentPrice thinks upgrade rank is: " + upgrade.rank);
        if (upgrade.rank > upgrade.totalRanks)
        {
            return 0;
        }
        else
        {
            return upgrade.prices[upgrade.rank - 1];
        }
    }

    public void UpdateGridItemUI()
    {
        selectedUpgradePrice.text = GetCurrentPrice().ToString();
        TMP_Text levelText = transform.Find("Level").GetComponent<TMP_Text>();
        if (upgrade.rank > upgrade.totalRanks)
        {
            levelText.text = "Max Level";
        }
        else
        {
            levelText.text = "Level " + upgrade.rank;
        }
    }

    public void Highlight(bool selected)
    {
        imageSelect.gameObject.SetActive(selected);
    }
    
}
