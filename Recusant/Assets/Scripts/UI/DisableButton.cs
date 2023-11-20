using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisableButton : MonoBehaviour
{
    
    public Button myButton;
    
    public void ButtonPressed()
    {
        myButton.interactable = false; // Disable the button
    }
    
    public void EnableButton()
    {
        myButton.interactable = true; // Re-enable the button
    }
    
    private void OnEnable()
    {
        EnableButton();
    }
}
