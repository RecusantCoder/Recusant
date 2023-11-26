using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private Animator animator;
    public List<GameObject> equipmentPrefabs;
    private bool wasDamaged;
    private int coinChance = 50;
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
        if (UnityEngine.Random.Range(0, 100) <= coinChance)
        {
            GameObject coin = Instantiate((equipmentPrefabs[0]), transform.position, transform.rotation);
        }
        else
        {
            int index = UnityEngine.Random.Range(1, equipmentPrefabs.Count);
            GameObject equipment = Instantiate((equipmentPrefabs[index]), transform.position, transform.rotation);
        }
    }
}
