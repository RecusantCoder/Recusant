using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class KillCounter : MonoBehaviour
{
    #region Singleton
    
    public static KillCounter instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of KillCounter found!");
            return;
        }
        instance = this;
    }
    
    #endregion
    
    
    [SerializeField]
    public TMP_Text myTextElement;
    public int killCount = 0;
    public event Action OnKill;

    private void Start()
    {
        myTextElement.text = 0 + "";
        
        
    }

    public void EnemyKilled()
    {
        killCount++;
        myTextElement.text = killCount + "";
        OnKill?.Invoke();
    }
}
