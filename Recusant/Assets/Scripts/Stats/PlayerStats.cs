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

    // Start is called before the first frame update
    void Start()
    {
        //healthBarUI = GameObject.Find("PlayerHealth").GetComponent<TextMeshProUGUI>();
        healthbar.SetMaxHealth(currentHealth);
        ApplyUpgrades();
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


    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        
        AudioManager.instance.Play("lowsound");
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

    public void ApplyUpgrades()
    {
        try
        {
            Debug.Log("Running ApplyUpgrades");
            DataManager dataManager = DataManager.Instance;
            List<Upgrade> loadedData = dataManager.LoadData<Upgrade>(DataManager.DataType.Upgrade);

            foreach (var upgrade in loadedData)
            {
                if (upgrade.rank > 1)
                {
                    Debug.Log("Applying Upgrades to " + upgrade.name);
                    if (upgrade.name.Equals("Armor"))
                    {
                        armor.AddModifier((upgrade.rank - 1)* 2);
                    } else if (upgrade.name.Equals("Value"))
                    {
                        value.AddModifier((upgrade.rank - 1));
                    } else if (upgrade.name.Equals("Damage"))
                    {
                        damage.AddModifier((upgrade.rank - 1)* 5);
                    } else if (upgrade.name.Equals("Speed"))
                    {
                        speed.AddModifier((upgrade.rank - 1));
                    } else if (upgrade.name.Equals("Attraction"))
                    {
                        pickupRadius.AddModifier((upgrade.rank - 1)* 2);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Catch: " + e);
        }
    }
}
