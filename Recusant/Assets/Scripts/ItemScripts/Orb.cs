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
        Debug.Log("Picking Up " + thisName);
        LevelBar.instance.AddExperience(xpDrop);
        Destroy(gameObject);

    }
}
