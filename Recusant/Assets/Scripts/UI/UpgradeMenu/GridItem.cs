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
    
    public void GridItemSelected()
    {
        selectedDescription.text = upgrade.description;
        selectedUpgradePrice.text = upgrade.price.ToString();
        
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
}
