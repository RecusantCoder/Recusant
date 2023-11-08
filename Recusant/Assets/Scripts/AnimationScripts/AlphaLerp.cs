using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaLerp : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float duration = 4.0f;
    private Color startColor;

    private void Start()
    {
        // Ensure spriteRenderer is assigned.
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Store the starting color.
        startColor = spriteRenderer.color;
    }

    private void Update()
    {
        // Calculate the alpha value using PingPong function.
        float alpha = Mathf.PingPong(Time.time / duration, 1.0f) * 255.0f;

        // Update the spriteRenderer's color with the new alpha value.
        Color lerpedColor = new Color(startColor.r, startColor.g, startColor.b, alpha / 255.0f);
        spriteRenderer.color = lerpedColor;
    }
}
