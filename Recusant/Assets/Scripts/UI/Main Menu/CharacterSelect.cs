using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private Image weaponImage;
    public GameObject playButton;
    
    public Image purchaseBackground;
    public TMP_Text priceText;
    public TMP_Text coinDisplayText;
    public CharacterSelectionManager characterSelectionManager;
    
    public bool isUnlocked;

    public Image imageSelect;

    private void Start()
    {
        GameObject coinDisplay = GameObject.Find("CoinDisplay");
        Transform amountTransform = coinDisplay.transform.Find("Amount");
        coinDisplayText = amountTransform.GetComponent<TMP_Text>();
        Highlight(false);
    }

    public void SetCharacter()
    {
        if (isUnlocked)
        {
            Debug.Log(nameText.text + " has been chosen with " + weaponImage.sprite.name);
            ChoiceManager.instance.chosenName = nameText.text;
            ChoiceManager.instance.chosenWeapon = weaponImage.sprite.name;
            characterSelectionManager.HighlightGridItem(this);
        
            playButton.SetActive(true);
        }
        else
        {
            ShowPurchaseBackground();
        }
    }

    public void ShowPurchaseBackground()
    {
        DataManager dataManager = DataManager.Instance;
        List<Total> loadedData = dataManager.LoadData<Total>(DataManager.DataType.Total);
        foreach (var total in loadedData)
        {
            if (total.name.Equals("coinsTotal"))
            {
                if (total.value - int.Parse(priceText.text) >= 0)
                {
                    purchaseBackground.gameObject.SetActive(true);
                }
            }
        }
    }

    public void HidePurchaseBackground()
    {
        purchaseBackground.gameObject.SetActive(false);
    }

    public void PurchaseCharacter()
    {
        DataManager dataManager = DataManager.Instance;
        List<Total> loadedData = dataManager.LoadData<Total>(DataManager.DataType.Total);
        foreach (var total in loadedData)
        {
            if (total.name.Equals("coinsTotal"))
            {
                if (total.value - int.Parse(priceText.text) >= 0)
                {
                    Debug.Log(total.value + " have total coins and " + int.Parse(priceText.text) + " price");
                    total.value -= int.Parse(priceText.text);
                    coinDisplayText.text = (total.value) + "";
                    dataManager.SaveData(loadedData, DataManager.DataType.Total);
                   
                }
            }
        }
        
        List<Character> characters = DataManager.Instance.LoadData<Character>(DataManager.DataType.Character);
        Character characterToModify = characters.Find(t => t.name == nameText.text);
        characterToModify.unlocked = true;
        DataManager.Instance.SaveData(characters, DataManager.DataType.Character);
        Debug.Log("Purchased and saved character unlock");
        HidePurchaseBackground();
        characterSelectionManager.ResetCharacterSelects();
    }
    
    public void Highlight(bool selected)
    {
        imageSelect.gameObject.SetActive(selected);
    }
}
