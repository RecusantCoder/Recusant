using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    /*For finding components
     *
     * Debug.Log("List of components");
        GAMEOBJECTNAME = GameObject.Find("PlayerHealth");
        
        Component[] components = GAMEOBJECTNAME.GetComponents(typeof(Component));
        foreach(Component component in components) 
        { 
            Debug.Log(component.ToString());
        }
        Debug.Log("Finished list of components");
     */

    public GameObject player;
    private TextMeshProUGUI healthBarUI;
    private GameObject playerHealthUI;

    public float playerHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        playerHealthUI = GameObject.Find("PlayerHealth");
        healthBarUI = playerHealthUI.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        DustDevilBehaviour.OnTouched += DamagePlayer;
    }
    
    private void OnDisable()
    {
        DustDevilBehaviour.OnTouched -= DamagePlayer;
    }

    void DamagePlayer()
    {
        playerHealth -= 10;
        UpdatePlayerHealth(playerHealth);
    }

    void UpdatePlayerHealth(float healthChange)
    {
        healthBarUI.text = healthChange.ToString() + "%";

    }

    
}
