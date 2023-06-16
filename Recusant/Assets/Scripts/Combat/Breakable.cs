using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private Animator animator;
    public List<string> equipmentPrefabsPaths;
    private bool wasDamaged;
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        equipmentPrefabsPaths = new List<string>();
        equipmentPrefabsPaths.Add("PreFabs/Pickups/Helmet");
        equipmentPrefabsPaths.Add("PreFabs/Pickups/Exolegs");
        equipmentPrefabsPaths.Add("PreFabs/Pickups/Body_Armor");
        equipmentPrefabsPaths.Add("PreFabs/Pickups/Targeting_Computer");
        equipmentPrefabsPaths.Add("PreFabs/Pickups/Haurio");

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
        int index = UnityEngine.Random.Range(0, equipmentPrefabsPaths.Count);
        GameObject equipment = Instantiate(Resources.Load<GameObject>(equipmentPrefabsPaths[index]), transform.position, transform.rotation);
    }
}
