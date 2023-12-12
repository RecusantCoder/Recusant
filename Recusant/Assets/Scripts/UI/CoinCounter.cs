using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    #region Singleton
    
    public static CoinCounter instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of CoinCounter found!");
            return;
        }
        instance = this;
    }
    
    #endregion
    
    
    [SerializeField]
    public TMP_Text myTextElement;
    public float coinCount = 0;

    private void Start()
    {
        myTextElement.text = 0 + "";
    }

    //Takes the amount of coins found and converts it to a float
    //This float is multiplied by the modifier which is a percentage
    //then the float value is set to an int for the coincounter UI
    //ie. a modifier of 2 is a 20% coin increase
    public void CoinsFound(int amount)
    {
        float floatAmount = amount;
        float valueModifier = floatAmount * PlayerManager.instance.player.GetComponent<PlayerStats>().value.GetValue();
        valueModifier = (valueModifier / 10) + 1;
        floatAmount *= valueModifier;
        coinCount += floatAmount;
        myTextElement.text = Mathf.RoundToInt(coinCount) + "";
    }
}
