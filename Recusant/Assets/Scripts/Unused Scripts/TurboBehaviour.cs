using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurboBehaviour : MonoBehaviour
{
    //public GameObject inventoryManagerScript;
    public InventoryManager m_someOtherScriptOnAnotherGameObject;
    private float elapsed = 0f;
    private float timePassed = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        //inventoryManagerScript.GetComponent<InventoryManager>().AddTurbo();
        m_someOtherScriptOnAnotherGameObject = GameObject.FindObjectOfType(typeof(InventoryManager)) as InventoryManager;
    }

    // Update is called once per frame
    void Update()
    {
        //was using for action every second
        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            
            elapsed = elapsed % 1f;
            timePassed++;
            if (timePassed >= 3)
            {
                Destroy(gameObject);
            }
        }

    }
    
    //if player touched this
    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.name == "Player")
        {
            m_someOtherScriptOnAnotherGameObject.AddTurbo();
            Debug.Log("Turbo Collected!");
            Destroy(gameObject);
        }
    }
}
