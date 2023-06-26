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

    //plays pickup sound, and sends selected item to GameManager, and Adds to Inventory
    public void PressedPickButton()
    {
        Debug.Log("Picked: " + item.name);
        Debug.Log(item.icon + " item's icon");

        PlayPickupSound();

        GameManager.instance.LevelSelectedWeapon(item.name);

        //adds item to inventory if not already there
        if (!Inventory.instance.items.Contains(item))
        {
            Debug.Log("added from levelslot");
            Inventory.instance.Add(item, false);
        }
        
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
