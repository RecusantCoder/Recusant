using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTrigger : MonoBehaviour
{
    public Animator animator;  
    public Button uiButton;
    public Button closeButton;
    public Image image;

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

    public void SetAnimationBackToImage()
    {
        image.sprite = Resources.Load<Sprite>("Sprites/Steel");
    }
    
}
