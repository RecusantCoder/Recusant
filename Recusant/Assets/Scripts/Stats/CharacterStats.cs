using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; set; }

    public Stat damage;
    public Stat armor;
    public Stat healthRegen;
    public Stat speed;
    public Stat pickupRadius;
    public Stat weight;
    public Stat spikeDamage;
    public Stat revives;
    public Stat value;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
       

    }

    // Damage updated to be reduced by a percentage which is 2x the armor value
    public virtual void TakeDamage(int damage)
    {
        Debug.Log("Damage before reduction: " + damage);
        //damage -= armor.GetValue();
        double damageAsDouble = damage * (100 - (armor.GetValue() * 2)) * 0.01;
        Debug.Log("DamageAsDouble: " + damageAsDouble);
        damage = Convert.ToInt32(Math.Round(damageAsDouble));
        damage = Mathf.Clamp(damage, 1, int.MaxValue);
        Debug.Log("Damage after reduction: " + damage);
        
        currentHealth -= damage;
        //Debug.Log(transform.name + " takes " + damage + " damage.");
        //Debug.Log(transform.name + " health is at " + currentHealth);
        
        //Show Damage Numbers
        if (damage > 0)
        {
            GameObject damageNum = Instantiate(Resources.Load("PreFabs/UI/DamageNumbers", typeof(GameObject))) as GameObject;
            damageNum.transform.position = transform.position;
            damageNum.GetComponent<DNController>().ShowDamage(damage);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // Die in some way, meant to be overwritten
        //Debug.Log(transform.name + " died.");
    }
    
    public void RegenHealth()
    {
        if (currentHealth < maxHealth)
        {
            if (currentHealth + healthRegen.GetValue() <= maxHealth)
            {
                currentHealth += healthRegen.GetValue();
            }
            else
            {
                currentHealth = maxHealth;
            }
        }
    }

}
