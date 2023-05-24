using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;

    //Adding aiming
    public Joystick joystick;
    public Rigidbody2D rb;
    private Vector2 movement;
    public GameObject player;
    
    private Weapon currentWeapon;
    private Dictionary<string, Weapon> weapons;

    private Inventory _inventory;

    public Dictionary<string, int> weaponLevelCountLocal;
    
    //Autokilling enemies
    private float searchRadius = 5f; // Radius to search for enemies
    public Transform autoFirePoint; 
    
    // Start is called before the first frame update
    void Start()
    {
        _inventory = Inventory.instance;

        // Create instances of each weapon
        weapons = new Dictionary<string, Weapon>();
        weapons.Add("Glock", new Glock());
        weapons.Add("Fulmen", new Fulmen());
        weapons.Add("Mossberg", new Mossberg());
        weapons.Add("LazerGun", new LazerGun());
    }

    // Update is called once per frame
    void Update()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        
        weaponLevelCountLocal = GameManager.instance.weaponLevelCount;
        
        foreach (var item in _inventory.items)
        {
            if (item.itemName == "Fulmen")
            {
                weapons["Fulmen"].Shoot(firePoint, weaponLevelCountLocal["Fulmen"]);
            }
            else if (item.itemName == "Mossberg")
            {
                weapons["Mossberg"].Shoot(firePoint, weaponLevelCountLocal["Mossberg"]);
            }
            else if (item.itemName == "Glock")
            {
                // If an enemy is found, point towards it and shoot
                if (nearestEnemy != null)
                {
                    Vector2 direction = nearestEnemy.transform.position - autoFirePoint.position;
                    autoFirePoint.right = direction.normalized;

                    weapons["Glock"].Shoot(autoFirePoint, weaponLevelCountLocal["Glock"]);
                }
            } 
            else if (item.itemName == "LazerGun")
            {
                weapons["LazerGun"].Shoot(firePoint, weaponLevelCountLocal["LazerGun"]);
            }
        }
        

        movement.x = joystick.Horizontal;
        movement.y = joystick.Vertical;
        
    }
    
    private void FixedUpdate()
    {
        firePoint.transform.position = player.transform.position;
        autoFirePoint.transform.position = player.transform.position;
        
        //rotation
        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg -90f;
        if (angle != -90f)
        {
            rb.rotation = angle;
        }
    }
    
    
    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= searchRadius && distance < nearestDistance)
            {
                CircleCollider2D enemyCollider = enemy.GetComponent<CircleCollider2D>();
                if (enemyCollider != null)
                {
                    if (enemyCollider.isActiveAndEnabled)
                    {
                        nearestEnemy = enemy;
                        nearestDistance = distance;
                    }
                }
            }
        }

        return nearestEnemy;
    }
    
}
