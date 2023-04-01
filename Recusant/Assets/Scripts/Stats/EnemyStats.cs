using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public GameObject orbPrefab;
    

    public override void Die()
    {
        base.Die();
        
        KillCounter.instance.EnemyKilled();

        GameObject orb = Instantiate(orbPrefab, transform.position, transform.rotation);
        
        Destroy(gameObject);
        
        // add loot spawn
    }
    
    
    private void Update()
    {
        //Debug.Log(gameObject.name + " health is " + currentHealth);
    }
}
