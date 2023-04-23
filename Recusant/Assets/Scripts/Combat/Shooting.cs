using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject lightningPrefab;

    public float bulletForce = 20f;
    
    //Adding aiming
    public Joystick joystick;
    public Rigidbody2D rb;
    private Vector2 movement;
    public GameObject player;
    
    //Timing
    private float elapsed = 0f;
    public float creationTime = 1f;
    private float timePassed = 0f;
    
    //Lightning Timing
    private bool hasFulmen = false;
    private float elapsedFulmen = 0f;
    public float creationTimeFulmen = 3f;
    private float timePassedFulmen = 0f;

    private Inventory _inventory;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _inventory = Inventory.instance;
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
            if (timePassed >= creationTime)
            {
                //Debug.Log("Fired Bullet!");
                    Shoot();
                
                timePassed = 0f;
            }
        }

        foreach (var item in _inventory.items)
        {
            if (item.itemName == "Fulmen")
            {
                hasFulmen = true;
            }
        }
        

        if (hasFulmen)
        {
            //was using for action every second
            elapsedFulmen += Time.deltaTime;
            if (elapsedFulmen >= 1f)
            {
                elapsedFulmen = elapsedFulmen % 1f;
                timePassedFulmen++;
                if (timePassedFulmen >= creationTimeFulmen)
                {
                    Debug.Log("Lightning Strike");
                    LightningStrike();

                    timePassedFulmen = 0f;
                }
            }
        }



        /*if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Lightning Strike");
            LightningStrike();
        }*/
        
        movement.x = joystick.Horizontal;
        movement.y = joystick.Vertical;
        
    }
    
    private void FixedUpdate()
    {
        firePoint.transform.position = player.transform.position;
        
        //rotation
        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg -90f;
        if (angle != -90f)
        {
            rb.rotation = angle;
        }
        
        //Debug.Log("The Angle is " + angle);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        
        //gunshot audio
        FindObjectOfType<AudioManager>().Play("gunshot");
    }

    void LightningStrike()
    {
        GameObject lightning = Instantiate(lightningPrefab, firePoint.position, Quaternion.identity);

        //lightning audio
        FindObjectOfType<AudioManager>().Play("lightning");
    }
}
