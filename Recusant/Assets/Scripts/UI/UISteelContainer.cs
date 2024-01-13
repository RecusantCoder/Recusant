using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISteelContainer : MonoBehaviour
{
    public Button openButton;
    public Button closeButton;

    public void EnableCloseButton()
    {
        closeButton.gameObject.SetActive(true);
        openButton.gameObject.SetActive(false);
    }

    public void DisableCloseButton()
    {
        closeButton.gameObject.SetActive(false);
        openButton.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        DisableCloseButton();
        openButton.interactable = true;
    }
    
    
}
