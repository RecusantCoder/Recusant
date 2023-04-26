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
        
        bool wasPickedUp = Inventory.instance.Add(item);
        if (wasPickedUp)
            Destroy(gameObject);

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
        }
        
    }
}
