using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustDevilBehaviour : MonoBehaviour
{
    private float sizeX = 1f;
    private bool isFat = false;
    private float sizeY = 1f;
    private float elapsed = 0f;
    public float sizeStep = 1f;
    public float maxSize = 10f;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //was using for action every second
        elapsed += Time.deltaTime;
        //if (elapsed >= 1f) {
            //elapsed = elapsed % 1f;
            
            //size
            if (sizeX <= maxSize && !isFat)
            {
                sizeX += sizeStep;
                if (sizeX >= maxSize)
                {
                    isFat = true;
                }
            }
            if (isFat)
            {
                sizeX -= sizeStep;
                if (sizeX <= 0)
                {
                    isFat = false;
                }
            }
        
            gameObject.transform.localScale = new Vector2(sizeX, sizeX);
            
            
        //}
        
        //rotation
        var rotationVector = transform.rotation.eulerAngles;
        if (rotationVector.z >= 359)
        {
            rotationVector.z = 0;
        }
        rotationVector.z += 45;
        transform.rotation = Quaternion.Euler(rotationVector);
        
        
    }
    
    //if player touched this
    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.name == "Player")
        {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            var direction = col.transform.position - transform.position;
            rb.AddForce(direction.normalized * 5000, ForceMode2D.Force);
            Debug.Log("REEEEE!!!!!");
            
        }
    }
}
