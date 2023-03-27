using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour
{
    // Public variables show up in the Inspector
    public Vector3 RotateSpeed = new Vector3 (10.0F, 10.0F, 10.0F);
    public Vector3 WobbleAmount = new Vector3 (0.1F, 0.1F, 0.1F);
    public Vector3 WobbleSpeed = new Vector3 (0.5F, 0.5F, 0.5F);

    // Private variables do not show up in the Inspector
    private Transform tr;
    private Vector3 BasePosition;
    private Vector3 NoiseIndex = new Vector3();
    
    public float rotateTimeMax = 1000f;
    public float rotateTimeTotal = 0f;
    public bool otherWay = false;
    public GameObject player;

    // Use this for initialization
    void Start () {
		
        // https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html
        tr = GetComponent ("Transform") as Transform;

        BasePosition = tr.position;

        NoiseIndex.x = Random.value;
        NoiseIndex.y = Random.value;
        NoiseIndex.z = Random.value;
    }
	
    // Update is called once per frame
    void Update () {

        // 1. ROTATE
        // Rotate the cube by RotateSpeed, multiplied by the fraction of a second that has passed.
        // In other words, we want to rotate by the full amount over 1 second
        
        if (!otherWay)
        {
            if (rotateTimeTotal <= rotateTimeMax)
            {
                tr.Rotate (Time.deltaTime * RotateSpeed);
                rotateTimeTotal++;
            } else if (rotateTimeTotal >= rotateTimeMax)
            {
                otherWay = true;
                rotateTimeTotal = 0f;
            }
        }
        
        if (otherWay)
        {
            if (rotateTimeTotal <= rotateTimeMax)
            {
                tr.Rotate (Time.deltaTime * -RotateSpeed);
                rotateTimeTotal++;
            } else if (rotateTimeTotal >= rotateTimeMax)
            {
                otherWay = false;
                rotateTimeTotal = 0f;
            }
        }
        
        
        


        
    }
}
