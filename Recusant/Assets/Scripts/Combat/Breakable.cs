using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        
    }

    public void Damaged()
    {
        animator.SetBool("wasHit", true);
        StartCoroutine(AnimateThenDie());
    }
    
    private IEnumerator AnimateThenDie()
    {
        yield return new WaitForSeconds(0.25f); // Adjust this delay based on your preference
        animator.SetBool("wasHit", false);
        Destroy(gameObject);
    }
}
