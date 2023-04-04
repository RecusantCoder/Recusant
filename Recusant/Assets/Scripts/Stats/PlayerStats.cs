using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    //private TextMeshProUGUI healthBarUI;
    public HealthBar healthbar;

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
        }
        
        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifier);
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
