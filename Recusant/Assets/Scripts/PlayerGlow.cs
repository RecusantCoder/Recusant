using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerGlow : MonoBehaviour
{
    public Light myLight;
    //Range variables
    public bool changeRange = false;
    public float rangeSpeed = 1.0f;
    public float maxRange = 10.0f;
    public bool repeatRange = false;
    
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
        myLight = gameObject.GetComponent<Light>();
        startTime = Time.time;
        gameObject.transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector2(player.transform.position.x, player.transform.position.y);

        if (changeRange)
        {
            myLight.range = Mathf.PingPong(Time.time * rangeSpeed, maxRange);
        }

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
}
