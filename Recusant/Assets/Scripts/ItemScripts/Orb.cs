using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : Interactable
{
    public float xpDrop;
    public string thisName;
    
    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    void PickUp()
    {
        LevelBar.instance.AddExperience(xpDrop);
        FindObjectOfType<AudioManager>().Play("pickup");
        Destroy(gameObject);

    }
}
