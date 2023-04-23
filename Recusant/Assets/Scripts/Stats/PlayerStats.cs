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

    // Start is called before the first frame update
    void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        //healthBarUI = GameObject.Find("PlayerHealth").GetComponent<TextMeshProUGUI>();
        healthbar.SetMaxHealth(currentHealth);
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
                Debug.Log("Regenerated Health!");
                RegenHealth();
                
                timePassed = 0f;
            }
        }
        
        
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Healing");
            RegenHealth();
        }
    }


    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        
        FindObjectOfType<AudioManager>().Play("lowsound");
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            armor.AddModifier(newItem.armorModifier);
            damage.AddModifier(newItem.damageModifier);
            healthRegen.AddModifier(newItem.healthRegenModifier);
        }
        
        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifier);
            healthRegen.RemoveModifier(oldItem.healthRegenModifier);
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
