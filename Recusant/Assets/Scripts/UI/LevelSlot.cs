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
            AudioManager.instance.Play("shotgunPickup");
        } else if (item.name == "Glock")
        {
            AudioManager.instance.Play("pistolPickup");
        } else if (item.name == "LazerGun")
        {
            AudioManager.instance.Play("lazerGunPickup");
        } else if (item.name == "Machete")
        {
            AudioManager.instance.Play("MachetePickup");
        } else if (item.name == "Grenade" || item.name == "Flashbang")
        {
            AudioManager.instance.Play("GrenadePickup");
        } else if (item.name == "Flamethrower" || item.name == "Molotov")
        {
            AudioManager.instance.Play("FlamethrowerPickup");
        } else if (item.name == "Qimmiq")
        {
            AudioManager.instance.Play("deepBark");
        } else if (item.name == "Fulmen")
        {
            AudioManager.instance.Play("LightningPickup");
        } else if (item.name == "Targeting_Computer" || item.name == "Exolegs")
        {
            AudioManager.instance.Play("TechPickup");
        } else if (item.name == "Helmet" || item.name == "Body_Armour")
        {
            AudioManager.instance.Play("ArmourPickup");
        } else if (item.name == "Fleshy")
        {
            AudioManager.instance.Play("FleshyPickup");
        } else if (item.name == "Haurio")
        {
            AudioManager.instance.Play("HaurioPickup");
        }
    }
    
}
