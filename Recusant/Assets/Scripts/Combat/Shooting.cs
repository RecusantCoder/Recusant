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
                weapons["Glock"].Shoot(firePoint, weaponLevelCountLocal["Glock"]);
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
        
        //rotation
        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg -90f;
        if (angle != -90f)
        {
            rb.rotation = angle;
        }
    }
    
}
