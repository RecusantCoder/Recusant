using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private Animator animator;
    public List<GameObject> powerUpPrefabs;
    public List<GameObject> coinPrefabs;
    public List<GameObject> foodPrefabs;
    private bool wasDamaged;
    private int coinChance = 61;
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void Damaged()
    {
        if (wasDamaged == false)
        {
            wasDamaged = true;
            animator.SetBool("wasHit", true);
            StartCoroutine(AnimateThenDie());
        }
    }
    
    private IEnumerator AnimateThenDie()
    {
        yield return new WaitForSeconds(0.25f); // Adjust this delay based on your preference
        animator.SetBool("wasHit", false);

        SpawnEquipment();
        Destroy(gameObject);
    }

    //if dice roll is below coin chance, spawn a coin
    //else roll for a random equipment to spawn
    //equipmentPrefabs[0] is coin
    private void SpawnEquipment()
    {
        int roll = UnityEngine.Random.Range(0, 100);
        if (roll <= coinChance)
        {
            if (roll >= coinChance - 10)
            {
                //spawn 10 coin
                GameObject coin = Instantiate((coinPrefabs[1]), transform.position, transform.rotation);
            }
            else if (roll >= coinChance - 1)
            {
                //spawn bar
                GameObject coin = Instantiate((coinPrefabs[2]), transform.position, transform.rotation);
            }
            else
            {
                GameObject coin = Instantiate((coinPrefabs[0]), transform.position, transform.rotation);
            }
        }
        else
        {
            if (roll >= 99)
            {
                int index = UnityEngine.Random.Range(0, powerUpPrefabs.Count);
                GameObject powerUp = Instantiate((powerUpPrefabs[index]), transform.position, transform.rotation);
            }
        }
    }
}
