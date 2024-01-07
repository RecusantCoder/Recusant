using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;


    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    void PickUp()
    {
        Debug.Log("Picking Up " + item.name);

        PlayPickupSound();
        
        bool wasPickedUp = Inventory.instance.Add(item, true);

        if (wasPickedUp)
            Destroy(gameObject);
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
