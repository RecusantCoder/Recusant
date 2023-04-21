using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionManager : MonoBehaviour
{
    public GameObject[] sectionPrefabs;
    private Transform playerTransform;
    private Vector2 spawn = new Vector2(0f, 0f);
    private float sectionLength = 10.24f;
    private int numOfSectionsOnScreen = 3;
    private bool startPoint = false;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < numOfSectionsOnScreen; i++)
        {
            SpawnSection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (playerTransform.position.y > (spawn.y - numOfSectionsOnScreen * sectionLength))
        {
            SpawnSection();
        }*/
    }

    private void SpawnSection(int prefabIndex = -1)
    {
        if (!startPoint)
        {
            GameObject go;
            go = Instantiate(sectionPrefabs[0]) as GameObject;
            go.transform.SetParent(transform);
            go.transform.position = playerTransform.position;
            spawn = playerTransform.position;
            spawn.x += sectionLength;
            //spawn.y += sectionLength;
            startPoint = true;
        }
        else
        {
            GameObject go;
            go = Instantiate(sectionPrefabs[0]) as GameObject;
            go.transform.SetParent(transform);
            go.transform.position = spawn;
            spawn.x += sectionLength;
            //spawn.y += sectionLength;
        }
    }

    //This returns the sprite and objects size
    public void getSpriteSize()
    {
        // Get the Sprite Renderer component of the GameObject
        SpriteRenderer spriteRenderer = sectionPrefabs[0].GetComponent<SpriteRenderer>();

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
