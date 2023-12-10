using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridItem : MonoBehaviour
{
    public Image gridItemBackground;
    
    public Upgrade upgrade;
    
    public TMP_Text selectedUpgradePrice;
    public TMP_Text selectedDescription;

    public MenuPanel menuPanel;
    
    public void GridItemSelected()
    {
        selectedDescription.text = upgrade.description;
        selectedUpgradePrice.text = GetCurrentPrice().ToString();
        menuPanel.chosenGridItem = gameObject;
        
        StartCoroutine(ButtonClickEffect());
    }
    
    private IEnumerator ButtonClickEffect()
    {
        // Change button color to a pressed state
        gridItemBackground.color = Color.gray;

        // Wait for a short duration
        yield return new WaitForSeconds(0.1f);

        // Reset the button color to its original state
        gridItemBackground.color = Color.white;
    }

    public int GetCurrentPrice()
    {
        return upgrade.prices[upgrade.rank - 1];
    }

    public void UpdateGridItemUI()
    {
        selectedUpgradePrice.text = GetCurrentPrice().ToString();
        TMP_Text levelText = transform.Find("Level").GetComponent<TMP_Text>();
        levelText.text = "Level " + upgrade.rank;
    }
    
}
