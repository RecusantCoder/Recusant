using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickerFlame : MonoBehaviour
{
    private Light2D myLight;
    
    //Intensity variables
    //public bool changeIntensity = false;
    public float intensitySpeed = 1.0f;
    public float maxIntensity = 10.0f;
    //public bool repeatIntensity = false;
    
    void Start()
    {
        myLight = GetComponent<Light2D>();
        
        //Use to see components of gameObject the code is attached to
        /*Component[] components = gameObject.GetComponents(typeof(Component));
        foreach(Component component in components) {
            Debug.Log(component.ToString());
        }*/
        
    }

    // Update is called once per frame
    private void Update()
    {
        
        myLight.intensity = Mathf.PingPong(Time.time * intensitySpeed, maxIntensity);
        
        
        
    }
}
