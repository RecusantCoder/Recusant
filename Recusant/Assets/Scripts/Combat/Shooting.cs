using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject lightningPrefab;
    public GameObject pelletPrefab;
    public GameObject lazerBeamPrefab;

    public float bulletForce = 20f;
    
    //Adding aiming
    public Joystick joystick;
    public Rigidbody2D rb;
    private Vector2 movement;
    public GameObject player;
    
    //Glock Timing
    private bool hasGlock = false;
    private float elapsedGlock = 0f;
    public float creationTimeGlock = 1f;
    private float timePassedGlock = 0f;
    
    //Lightning Timing
    private bool hasFulmen = false;
    private float elapsedFulmen = 0f;
    public float creationTimeFulmen = 3f;
    private float timePassedFulmen = 0f;
    
    //Mossberg Timing
    private bool hasMossberg = false;
    public int pellets = 10;
    private float elapsedMoss = 0f;
    public float creationTimeMoss = 1.5f;
    private float timePassedMoss = 0f;

    //LazerGun Timing
    private bool hasLazerGun = false;
    private float elapsedLazerGun = 0f;
    public float creationTimeLazerGun = 1f;
    private float timePassedLazerGun = 0f;

    private Inventory _inventory;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _inventory = Inventory.instance;
        
        // Subscribe to the OnItemRemoved event
        _inventory.OnItemRemoved += HandleItemRemoved;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in _inventory.items)
        {
            if (item.itemName == "Fulmen")
            {
                hasFulmen = true;
            }
            else if (item.itemName == "Mossberg")
            {
                hasMossberg = true;
            }
            else if (item.itemName == "Glock")
            {
                hasGlock = true;
            } 
            else if (item.itemName == "LazerGun")
            {
                hasLazerGun = true;
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
        
        ShootGlock();

        ShootMossberg();
        
        ShootLazerGun();

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
    
    private void HandleItemRemoved(Item item)
    {
        if (item.itemName == "Fulmen")
        {
            hasFulmen = false;
        }
        else if (item.itemName == "Mossberg")
        {
            hasMossberg = false;
        }
        else if (item.itemName == "Glock")
        {
            hasGlock = false;
        } else if (item.itemName == "LazerGun")
        {
            hasLazerGun = false;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        
        //gunshot audio
        FindObjectOfType<AudioManager>().Play("gunshot");
    }
    
    void ShootGlock()
    {
        if (hasGlock)
        {
            //was using for action every second
            elapsedGlock += Time.deltaTime;
            if (elapsedGlock >= 1f)
            {
                elapsedGlock = elapsedGlock % 1f;
                timePassedGlock++;
                if (timePassedGlock >= creationTimeGlock)
                {
                    Debug.Log("Fired Glock!");

                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

                    //gunshot audio
                    FindObjectOfType<AudioManager>().Play("pistolGunshot");
                
                    timePassedGlock = 0f;
                }
            }
        }
    }

    void LightningStrike()
    {
        GameObject lightning = Instantiate(lightningPrefab, firePoint.position, Quaternion.identity);

        //lightning audio
        FindObjectOfType<AudioManager>().Play("lightning");
    }

    void ShootMossberg()
    {
        if (hasMossberg)
        {
            //was using for action every second
            elapsedMoss += Time.deltaTime;
            if (elapsedMoss >= 1f)
            {
                elapsedMoss = elapsedMoss % 1f;
                timePassedMoss++;
                if (timePassedMoss >= creationTimeMoss)
                {
                    Debug.Log("Fired Mossberg!");

                    for (int i = 0; i < pellets; i++)
                    {
                        Quaternion rotationA = Quaternion.Euler(0f, 0f, Random.Range(-20f, 20f));
                        GameObject pellet = Instantiate(pelletPrefab, firePoint.position, firePoint.rotation * rotationA);
                        Rigidbody2D rb = pellet.GetComponent<Rigidbody2D>();
                        rb.AddForce(pellet.transform.up * bulletForce, ForceMode2D.Impulse);
                        
                    }

                    //gunshot audio
                    FindObjectOfType<AudioManager>().Play("shotgunGunshot");
                
                    timePassedMoss = 0f;
                }
            }
        }
    }
    
    void ShootLazerGun()
    {
        if (hasLazerGun)
        {
            //was using for action every second
            elapsedLazerGun += Time.deltaTime;
            if (elapsedLazerGun >= 1f)
            {
                elapsedLazerGun = elapsedLazerGun % 1f;
                timePassedLazerGun++;
                if (timePassedLazerGun >= creationTimeLazerGun)
                {
                    Debug.Log("Fired LazerGun!");
                    Quaternion rotationA = Quaternion.Euler(0f, 0f, 90f);
                    //Vector3 offsetA = new Vector3(1f, 0f, 0f);
                    GameObject lazerBeam = Instantiate(lazerBeamPrefab, firePoint.position, firePoint.rotation * rotationA);
                    Rigidbody2D rb = lazerBeam.GetComponent<Rigidbody2D>();
                    //rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

                    //gunshot audio
                    FindObjectOfType<AudioManager>().Play("lazerGunGunshot");
                
                    timePassedLazerGun = 0f;
                }
            }
        }
    }
}
