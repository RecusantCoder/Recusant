using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; private set; }

    public Stat damage;
    public Stat armor;
    



    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        
        
        
    }

    public virtual void TakeDamage(int damage)
    {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);
        
        currentHealth -= damage;
        //Debug.Log(transform.name + " takes " + damage + " damage.");
        //Debug.Log(transform.name + " health is at " + currentHealth);
        
        //Show Damage Numbers
        GameObject damageNum = Instantiate(Resources.Load("PreFabs/DamageNumbers", typeof(GameObject))) as GameObject;
        damageNum.transform.position = transform.position;
        damageNum.GetComponent<DNController>().ShowDamage(damage);
        
        
        


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

}
