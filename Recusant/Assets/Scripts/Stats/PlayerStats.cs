using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    //private TextMeshProUGUI healthBarUI;
    public HealthBar healthbar;
    
    //Timing
    private float elapsed = 0f;
    public float creationTime = 1f;
    private float timePassed = 0f;
    
    private Dictionary<string, EquipmentLogic> logicMap = new Dictionary<string, EquipmentLogic>();
    EquipmentLogic logic = null;

    // Start is called before the first frame update
    void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        //healthBarUI = GameObject.Find("PlayerHealth").GetComponent<TextMeshProUGUI>();
        healthbar.SetMaxHealth(currentHealth);
        
        // Populate the logic map
        logicMap.Add("Targeting_Computer", new TargetingComputerLogic());
        logicMap.Add("Exolegs", new ExolegsLogic());
        logicMap.Add("Helmet", new HelmetLogic());
    }
    
    private void Update()
    {
        UpdatePlayerHealth(currentHealth);
        
        
        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            elapsed = elapsed % 1f;
            timePassed++;
            if (timePassed >= creationTime)
            {
                RegenHealth();
                
                timePassed = 0f;
            }
        }
        
    }

    private void OnDestroy()
    {
        EquipmentManager.instance.onEquipmentChanged -= OnEquipmentChanged;
    }


    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        
        FindObjectOfType<AudioManager>().Play("lowsound");
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem, int lvl)
    {
        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifier);
            healthRegen.RemoveModifier(oldItem.healthRegenModifier);
            speed.RemoveModifier(oldItem.speedModifier);
            pickupRadius.RemoveModifier(oldItem.pickupRadiusModifier);
        }
        
        //adding new level updates to the item before equipping
        if (logicMap.ContainsKey(newItem.itemName))
        {
            logic = logicMap[newItem.itemName];
            logic.ApplyLogic(newItem, oldItem == null ? 1 : lvl);
        }

        if (newItem != null)
        {
            armor.AddModifier(newItem.armorModifier);
            damage.AddModifier(newItem.damageModifier);
            healthRegen.AddModifier(newItem.healthRegenModifier);
            speed.AddModifier(newItem.speedModifier);
            pickupRadius.AddModifier(newItem.pickupRadiusModifier);
        }
    }


    public override void Die()
    {
        base.Die();
        //Kill the player
        PlayerManager.instance.KillPlayer();
    }

    void UpdatePlayerHealth(float healthChange)
    {
        //healthBarUI.text = healthChange.ToString() + "%";
        healthbar.SetHealth((int)healthChange);
    }
    
}
