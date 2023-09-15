using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private Animator animator;
    public List<GameObject> equipmentPrefabs;
    private bool wasDamaged;
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

    private void SpawnEquipment()
    {
        int index = UnityEngine.Random.Range(0, equipmentPrefabs.Count);
        GameObject equipment = Instantiate((equipmentPrefabs[index]), transform.position, transform.rotation);
    }
}
