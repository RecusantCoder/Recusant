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
    public int coinCount = 0;

    private void Start()
    {
        myTextElement.text = 0 + "";
    }

    public void CoinsFound(int amount)
    {
        coinCount += amount;
        myTextElement.text = coinCount + "";
    }
}
