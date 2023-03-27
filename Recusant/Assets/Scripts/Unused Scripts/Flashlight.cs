using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flashlight : MonoBehaviour
{
    public Light2D myLight;
    public Joystick joystick;

    //Intensity variables
    public bool changeIntensity = false;
    public float intensitySpeed = 1.0f;
    public float maxIntensity = 10.0f;
    public bool repeatIntensity = false;
    
    // Color variables
    public bool changeColors = false;
    public float colorSpeed = 1.0f;
    public Color startColor;
    public Color endColor;
    public bool repeatColor = false;

    private float startTime;
    
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        myLight = gameObject.GetComponent<Light2D>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        LightDirection(joystick.Horizontal, joystick.Vertical);
    


    if (changeIntensity)
        {
            myLight.intensity = Mathf.PingPong(Time.time * intensitySpeed, maxIntensity);
        }

        if (changeColors)
        {
            float t = (Mathf.Sin(Time.time - startTime * colorSpeed));
            myLight.color = Color.Lerp(startColor, endColor, t);
        }
    }

    private void LightDirection(float joystickX, float joystickY)
    {
        var rotationVector = transform.rotation.eulerAngles;
        
        if (joystick.Horizontal >= 0.3f)
        {
            if (joystick.Vertical >= 0.3f)
            {
                rotationVector.z = 315;
            } else if (joystick.Vertical <= -0.3f)
            {
                rotationVector.z = 225;
            }
            else
            {
                rotationVector.z = 270;
            }
        } else if (joystick.Horizontal <= -0.3f)
        {
            if (joystick.Vertical >= 0.3f)
            {
                rotationVector.z = 45;
            } else if (joystick.Vertical <= -0.3f)
            {
                rotationVector.z = 135;
            }
            else
            {
                rotationVector.z = 90;
            }
        } 
        else
        {
            if (joystick.Vertical <= -0.3f)
            {
                rotationVector.z = 180;
            }
            else if (joystick.Vertical >= 0.3f)
            {
                rotationVector.z = 0;
            }
        }
        
        
        transform.rotation = Quaternion.Euler(rotationVector);
    }
    
    public void OnButtonPress()
    {
        if (myLight.color != Color.black)
        {
            myLight.color = (Color.black);
        }
        else
        {
            myLight.color = Color.yellow;
        }
       
    }
}
