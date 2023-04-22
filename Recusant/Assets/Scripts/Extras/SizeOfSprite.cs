using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeOfSprite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //This returns the sprite and objects size
    public void getSpriteSize()
    {
        // Get the Sprite Renderer component of the GameObject
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

// Get the Bounds of the Sprite Renderer
        Bounds bounds = spriteRenderer.bounds;

// Get the size of the Bounds in x and y dimensions
        float sizeX = bounds.size.x;
        float sizeY = bounds.size.y;
        
        // Get the Sprite component of the GameObject
        Sprite sprite = spriteRenderer.sprite;

// Get the size of the Sprite's rect in pixels
        float spriteWidth = sprite.rect.width;
        float spriteHeight = sprite.rect.height;
        
        Debug.Log("sizex: " + sizeX);
        Debug.Log("sizey: " + sizeY);
        Debug.Log("spriteWidth: " + spriteWidth);
        Debug.Log("spriteHeight: " + spriteHeight);
    }
}
