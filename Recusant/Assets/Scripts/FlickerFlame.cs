using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickerFlame : MonoBehaviour
{
    private Light2D myLight;
    
    void Start()
    {
        myLight = GetComponent<Light2D>();
        Component[] components = gameObject.GetComponents(typeof(Component));
        foreach(Component component in components) {
            Debug.Log(component.ToString());
        }
        
    }

    // Update is called once per frame
    private void Update()
    {
        
        myLight.color = Color.blue;
        Debug.Log(myLight.name);
        
        
    }
}
