using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public float xpDrop;
    
    public override void Die()
    {
        base.Die();
        
        KillCounter.instance.EnemyKilled();
        LevelBar.instance.AddExperience(xpDrop);

        Destroy(gameObject);
        
        // add loot spawn
    }
    
    
    private void Update()
    {
        //Debug.Log(gameObject.name + " health is " + currentHealth);
    }
}
