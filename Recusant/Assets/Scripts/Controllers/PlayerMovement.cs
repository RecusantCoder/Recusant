using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
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

    private PlayerStats playerStats;

    public float lastNonZeroInput;

    public ParticleSystem dust;

    public Vector2 Movement
    {
        get => movement;
        private set => movement = value;
    }


    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        GameObject JoyStickObject = GameObject.FindWithTag("Joystick");
        joystick = JoyStickObject.GetComponent<FloatingJoystick>();
        CheckChoiceManager();
    }

    // Update is called once per frame
    void Update()
    {
        // Input
        //movement.x = Input.GetAxisRaw("Horizontal");
        movement.x = joystick.Horizontal;
        movement.y = joystick.Vertical;
        
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        

        Collider2D[] nearItems = Physics2D.OverlapCircleAll(pickupPoint.position, pickupRadius, itemLayers);
        //Debug.Log("Nearby items count: " + nearItems.Length);

        foreach (Collider2D nearItem in nearItems)
        {
            Interactable interactable = nearItem.GetComponent<Interactable>();
            if (interactable != null)
            {
                SetFocus(interactable);
            }
        }
        
        updateMoveSpeedWithSpeedModifier();
        updatePickupRadiusWithPickupRadiusModifier();
        
        //Debug.Log(joystick.Horizontal + " <-horizontal and vertical-> " + joystick.Vertical);
        if (joystick.Horizontal != 0.00)
        {
            lastNonZeroInput = joystick.Horizontal;
            if (lastNonZeroInput <= 0.01)
            {
                animator.SetBool("isGoingLeft", true);
                dust.Play();
            }
            else
            {
                animator.SetBool("isGoingLeft", false);
                dust.Play();
            }
        }
        
        
    }

    private void FixedUpdate()
    { //Movement
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        //Debug.Log(moveSpeed + " movespeed");
        
        //rotation
        //float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg -90f;
        //rb.rotation = angle;
        
        
    }
    
    
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
        //Gizmos.DrawWireSphere(gameObject.transform.position, spawnRadius);
    }

    private void updateMoveSpeedWithSpeedModifier()
    {
        float speed = 1;
        if (playerStats.speed.GetValue() != 1)
        {
            speed = (float)playerStats.speed.GetValue() / 10 + 1 - 0.1f;
            speed -= (float)playerStats.weight.GetValue() / 10;
        }
        if (moveSpeed != speed)
        {
            moveSpeed = speed;
        }
    }
    
    private void updatePickupRadiusWithPickupRadiusModifier()
    {
        float pickupRadiusLocal = playerStats.pickupRadius.GetValue() * 0.32f + 0.32f;
        if (pickupRadius != pickupRadiusLocal)
        {
            pickupRadius = pickupRadiusLocal;
        }
        
        //Debug.Log("Pickup radius in playerMovement: " + pickupRadius + " and pickupRadiusLocal: " +  pickupRadiusLocal);
    }

    private void CheckChoiceManager()
    {
        if (ChoiceManager.instance.chosenName == "Degtyarev")
        {
            animator.SetBool("isDegtyarev", true);
        }

        if (ChoiceManager.instance.chosenName == "Makwa")
        {
            animator.SetBool("isMakwa", true);
        }

        if (ChoiceManager.instance.chosenWeapon == "MossbergSprite")
        { 
            GameObject chosenWeapon = Instantiate(Resources.Load<GameObject>("PreFabs/Pickups/Mossberg"), transform.position, transform.rotation);
        } 
        else if (ChoiceManager.instance.chosenWeapon == "GlockSprite")
        {
            GameObject chosenWeapon = Instantiate(Resources.Load<GameObject>("PreFabs/Pickups/Glock"), transform.position, transform.rotation);
        }
    }
}
