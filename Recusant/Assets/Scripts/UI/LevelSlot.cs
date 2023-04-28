using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelSlot : MonoBehaviour
{
    public Item item;
    public Image icon;
    public Button pickButton;
    // Start is called before the first frame update
    void Start()
    {
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressedPickButton()
    {
        Debug.Log("Picked: " + item.name);

        PlayPickupSound();
        
        Inventory.instance.Add(item);
    }
    
    void PlayPickupSound()
    {
        if (item.name == "Mossberg")
        {
            //gunshot audio
            FindObjectOfType<AudioManager>().Play("shotgunPickup");
        } else if (item.name == "Glock")
        {
            FindObjectOfType<AudioManager>().Play("pistolPickup");
        } else if (item.name == "LazerGun")
        {
            FindObjectOfType<AudioManager>().Play("lazerGunPickup");
        }
        
    }
}
