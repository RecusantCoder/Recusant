using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTrigger : MonoBehaviour
{
    public Animator animator;  // Reference to the Animator component
    public Button uiButton;    // Reference to the UI Button component

    private void Start()
    {
        // Attach the UI Button's click event to a custom method
        uiButton.onClick.AddListener(TriggerAnimation);
    }

    // Custom method to trigger the animation
    private void TriggerAnimation()
    {
        // Trigger the animation by setting the trigger parameter
        animator.SetTrigger("MyTrigger");
    }
}
