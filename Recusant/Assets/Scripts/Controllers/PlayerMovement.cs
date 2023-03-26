using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Joystick joystick;
    private Vector2 movement;
    public Animator animator;
    
    //For interaction system
    public Interactable focus;
    public Transform pickupPoint;
    public float pickupRadius = 0.5f;
    public LayerMask itemLayers;
    
    public float spawnRadius = 10f;

    // Update is called once per frame
    void Update()
    { // Input
        //movement.x = Input.GetAxisRaw("Horizontal");
        movement.x = joystick.Horizontal;
        movement.y = joystick.Vertical;
        
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        Collider2D[] nearItems = Physics2D.OverlapCircleAll(pickupPoint.position, pickupRadius, itemLayers);
        foreach (Collider2D nearItem in nearItems)
        {
            Interactable interactable = nearItem.GetComponent<Interactable>();
            if (interactable != null)
            {
                SetFocus(interactable);
            }
        }


    }

    private void FixedUpdate()
    { //Movement
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
    
    /*
    //if player touches another collider
    void OnTriggerEnter2D(Collider2D col)
    {
        Interactable interactable = col.GetComponent<Interactable>();

        if (interactable != null)
        {
            SetFocus(interactable);
        }
    }*/
    
    // Set our focus to a new focus
    void SetFocus (Interactable newFocus)
    {
        // If our focus has changed
        if (newFocus != focus)
        {
            // Defocus the old one
            if (focus != null)
                focus.OnDefocused();

            focus = newFocus;	// Set our new focus
            //motor.FollowTarget(newFocus);	// Follow the new focus
        }
		
        newFocus.OnFocused(transform);
    }

    // Remove our current focus
    void RemoveFocus ()
    {
        if (focus != null)
            focus.OnDefocused();

        focus = null;
        //motor.StopFollowingTarget();
    }

    private void OnDrawGizmosSelected()
    {
        if (pickupPoint == null)
            return;
        Gizmos.DrawWireSphere(pickupPoint.position, pickupRadius);
        Gizmos.DrawWireSphere(gameObject.transform.position, spawnRadius);
    }
}
