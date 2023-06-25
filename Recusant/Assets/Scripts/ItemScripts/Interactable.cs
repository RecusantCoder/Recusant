using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float pickupRadius;
    public Transform interactionTransform;	// The transform from where we interact in case you want to offset it

    private bool isFocus = false;
    private Transform player;

    private bool hasInteracted = false;

    private PlayerStats _playerStats;

    private void Start()
    {
        pickupRadius = PlayerManager.instance.player.GetComponent<PlayerStats>().pickupRadius.GetValue();
    }

    public virtual void Interact()
    {
        //This method is meant to be overwritten
    }
    
    

    private void Update()
    {
        updatePickupRadiusWithPickupRadiusModifier();
        if (isFocus && !hasInteracted)
        {
            float distance = Vector2.Distance(player.position, transform.position);
            
            if (distance <= pickupRadius)
            {
                Interact();
                //Debug.Log("Interact!");
                hasInteracted = true;
            }
        }
    }
    
    // Called when the object starts being focused
    public void OnFocused (Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    // Called when the object is no longer focused
    public void OnDefocused ()
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

    // Draw our radius in the editor
    void OnDrawGizmosSelected ()
    {
        if (interactionTransform == null)
            interactionTransform = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, pickupRadius);
    }
    
    private void updatePickupRadiusWithPickupRadiusModifier()
    {
        pickupRadius = PlayerManager.instance.player.GetComponent<PlayerStats>().pickupRadius.GetValue();
    }
}
