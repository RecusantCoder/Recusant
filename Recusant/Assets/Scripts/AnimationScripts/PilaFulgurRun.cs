using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilaFulgurRun : StateMachineBehaviour
{
    private Transform player;
    private Rigidbody2D rb;
    private float speed = 2.5f;
    
    public float lookRadius = 5f;
    public float attackRadius = 3f;
    
    
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector2.Distance(rb.position, player.position);
        //Debug.Log(rb.position.x + " " + rb.position.y);
        Debug.Log(distance);
        
        Vector2 target = new Vector2(player.position.x, player.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        
        
        if (distance <= attackRadius)
        {
            animator.SetTrigger("Attack");
        }
        
       


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
    
    // Rotate to face the target
    /*void FaceTarget ()
    {
        Vector3 direction = (player.position - rb.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }*/
    
}
