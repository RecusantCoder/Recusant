using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustDevilBehaviour : MonoBehaviour
{
    private float sizeX = 1f;
    private bool isFat = false;
    //private float sizeY = 1f;
    private float elapsed = 0f;
    public float sizeStep = 1f;
    public float maxSize = 10f;
    private float randomCreationTime;
    public GameObject turbo;
    private Vector2 imHere;
    private float timePassed = 0f;
    
    //for events
    public delegate void TouchedByPlayer();

    public static event TouchedByPlayer OnTouched;
    
    

    // Start is called before the first frame update
    void Start()
    {
        randomCreationTime = Random.Range(6.0f, 60.0f);
       
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
            if (timePassed >= randomCreationTime)
            {
                Debug.Log("Made a Turbo!");
                imHere = transform.position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
                
                //Instantiate(turbo, imHere, Quaternion.identity);
                GameObject instance = Instantiate(Resources.Load("Prefabs/Turbo_Artifact", typeof(GameObject)), imHere, Quaternion.identity) as GameObject;   
                timePassed = 0f;
                randomCreationTime = Random.Range(6.0f, 60.0f);
            }
        }

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

            //Creates event
            if (OnTouched != null)
            {
                OnTouched();
            }
        }
    }
}
