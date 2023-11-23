using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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

    private Inventory _inventory;

    public Dictionary<string, int> weaponLevelCountLocal;
    
    //Autokilling enemies
    private float searchRadius = 3f; // Radius to search for enemies
    public Transform autoFirePoint;

    private Glock glockComponent;
    private LazerGun lazerGunComponent;
    private Fulmen fulmenComponent;
    private Mossberg mossbergComponent;
    private Grenade grenadeComponent;
    private Machete macheteComponent;
    private Flamethrower flamethrowerComponent;
    private Flashbang flashbangComponent;
    private Molotov molotovComponent;
    public Qimmiq qimmiqComponent;
    private Body_Armor bodyArmorComponent;
    private Exolegs exolegsComponent;
    private Fleshy fleshyComponent;
    private Haurio haurioComponent;
    private Helmet helmetComponent;
    private Targeting_Computer targetingComputerComponent;
    private SwoleSamoyed swoleSamoyedComponent;
    private FlameShield flameShieldComponent;
    private MiniNuke miniNukeComponent;
    public Haurifulminator haurifulminatorComponent;
    public Bosco boscoComponent;
    public DirectionalVectorDisruptor directionalVectorDisruptorComponent;
    public BladeStorm bladeStormComponent;
    
    //Flamethrower's private firepoint
    public Transform flamethrowerFirepoint;
    
    //Bladestorm's personal firepoint
    public Transform bladeStormFirepoint;
    
    // Start is called before the first frame update
    void Start()
    {
        _inventory = Inventory.instance;
        GameObject JoyStickObject = GameObject.FindWithTag("Joystick");
        joystick = JoyStickObject.GetComponent<FloatingJoystick>();

        // Create instances of each weapon
        glockComponent = autoFirePoint.gameObject.AddComponent<Glock>();
        lazerGunComponent = firePoint.gameObject.AddComponent<LazerGun>();
        fulmenComponent = firePoint.gameObject.AddComponent<Fulmen>();
        mossbergComponent = firePoint.gameObject.AddComponent<Mossberg>();
        grenadeComponent = firePoint.gameObject.AddComponent<Grenade>();
        macheteComponent = firePoint.gameObject.AddComponent<Machete>();
        flamethrowerComponent = flamethrowerFirepoint.gameObject.AddComponent<Flamethrower>();
        flashbangComponent = firePoint.gameObject.AddComponent<Flashbang>();
        molotovComponent = firePoint.gameObject.AddComponent<Molotov>();
        qimmiqComponent = firePoint.gameObject.AddComponent<Qimmiq>();
        bodyArmorComponent = firePoint.gameObject.AddComponent<Body_Armor>();
        exolegsComponent = firePoint.gameObject.AddComponent<Exolegs>();
        fleshyComponent = firePoint.gameObject.AddComponent<Fleshy>();
        haurioComponent = firePoint.gameObject.AddComponent<Haurio>();
        helmetComponent = firePoint.gameObject.AddComponent<Helmet>();
        targetingComputerComponent = firePoint.gameObject.AddComponent<Targeting_Computer>();
        swoleSamoyedComponent = firePoint.gameObject.AddComponent<SwoleSamoyed>();
        flameShieldComponent = firePoint.gameObject.AddComponent<FlameShield>();
        miniNukeComponent = firePoint.gameObject.AddComponent<MiniNuke>();
        haurifulminatorComponent = firePoint.gameObject.AddComponent<Haurifulminator>();
        boscoComponent = firePoint.gameObject.AddComponent<Bosco>();
        directionalVectorDisruptorComponent = firePoint.gameObject.AddComponent<DirectionalVectorDisruptor>();
        bladeStormComponent = bladeStormFirepoint.gameObject.AddComponent<BladeStorm>();
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
                fulmenComponent.Shoot(firePoint, weaponLevelCountLocal["Fulmen"]);
            }
            else if (item.itemName == "Mossberg")
            {
                mossbergComponent.Shoot(firePoint, weaponLevelCountLocal["Mossberg"]);
            }
            else if (item.itemName == "Glock")
            {
                // If an enemy is found, point towards it and shoot
                if (nearestEnemy != null)
                {
                    Vector2 direction = nearestEnemy.transform.position - autoFirePoint.position;
                    autoFirePoint.right = direction.normalized;

                    glockComponent.Shoot(autoFirePoint, weaponLevelCountLocal["Glock"], true);
                }
                else
                {
                    glockComponent.Shoot(autoFirePoint, weaponLevelCountLocal["Glock"], false);
                }
            } 
            else if (item.itemName == "LazerGun")
            {
                lazerGunComponent.Shoot(firePoint, weaponLevelCountLocal["LazerGun"]);
            }
            else if (item.itemName == "Grenade")
            {
                grenadeComponent.Shoot(firePoint, weaponLevelCountLocal["Grenade"]);
            } 
            else if (item.itemName == "Machete")
            {
                macheteComponent.Shoot(firePoint, weaponLevelCountLocal["Machete"]);
            }
            else if (item.itemName == "Flamethrower")
            {
                flamethrowerComponent.Shoot(flamethrowerFirepoint, weaponLevelCountLocal["Flamethrower"]);
            } 
            else if (item.itemName == "Flashbang")
            {
                flashbangComponent.Shoot(firePoint, weaponLevelCountLocal["Flashbang"]);
            } 
            else if (item.itemName == "Molotov")
            {
                molotovComponent.Shoot(firePoint, weaponLevelCountLocal["Molotov"]);
            } 
            else if (item.itemName == "Qimmiq")
            {
                qimmiqComponent.Shoot(firePoint, weaponLevelCountLocal["Qimmiq"]);
            } 
            else if (item.itemName == "Body_Armor")
            {
                bodyArmorComponent.Shoot(firePoint, weaponLevelCountLocal["Body_Armor"]);
            } 
            else if (item.itemName == "Exolegs")
            {
                exolegsComponent.Shoot(firePoint, weaponLevelCountLocal["Exolegs"]);
            }
            else if (item.itemName == "Fleshy")
            {
                fleshyComponent.Shoot(firePoint, weaponLevelCountLocal["Fleshy"]);
            }
            else if (item.itemName == "Haurio")
            {
                haurioComponent.Shoot(firePoint, weaponLevelCountLocal["Haurio"]);
            }
            else if (item.itemName == "Helmet")
            {
                helmetComponent.Shoot(firePoint, weaponLevelCountLocal["Helmet"]);
            } 
            else if (item.itemName == "Targeting_Computer")
            {
                targetingComputerComponent.Shoot(firePoint, weaponLevelCountLocal["Targeting_Computer"]);
            } 
            else if (item.itemName == "SwoleSamoyed")
            {
                swoleSamoyedComponent.Shoot(firePoint, weaponLevelCountLocal["SwoleSamoyed"]);
            }
            else if (item.itemName == "FlameShield")
            {
                flameShieldComponent.Shoot(firePoint, weaponLevelCountLocal["FlameShield"]);
            } 
            else if (item.itemName == "MiniNuke")
            {
                miniNukeComponent.Shoot(firePoint, weaponLevelCountLocal["MiniNuke"]);
            } else if (item.itemName == "Haurifulminator")
            {
                haurifulminatorComponent.Shoot(firePoint, weaponLevelCountLocal["Haurifulminator"]);
            } else if (item.itemName == "Bosco")
            {
                boscoComponent.Shoot(firePoint, weaponLevelCountLocal["Bosco"]);
            } else if (item.itemName == "DirectionalVectorDisruptor")
            {
                directionalVectorDisruptorComponent.Shoot(firePoint, weaponLevelCountLocal["DirectionalVectorDisruptor"]);
            } else if (item.itemName == "BladeStorm")
            {
                bladeStormComponent.Shoot(bladeStormFirepoint, weaponLevelCountLocal["BladeStorm"]);
            }
        }
        

        movement.x = joystick.Horizontal;
        movement.y = joystick.Vertical;
        
    }
    
    private void FixedUpdate()
    {
        var position = player.transform.position;
        firePoint.transform.position = position;
        autoFirePoint.transform.position = position;
        flamethrowerFirepoint.transform.position = position;
        bladeStormFirepoint.transform.position = position;
        
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
