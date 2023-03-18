using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurboBehaviour : MonoBehaviour
{
    //public GameObject inventoryManagerScript;
    public InventoryManager m_someOtherScriptOnAnotherGameObject;
    
    // Start is called before the first frame update
    void Start()
    {
        //inventoryManagerScript.GetComponent<InventoryManager>().AddTurbo();
        m_someOtherScriptOnAnotherGameObject = GameObject.FindObjectOfType(typeof(InventoryManager)) as InventoryManager;
    }

    // Update is called once per frame
    void Update()
    {
        
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
