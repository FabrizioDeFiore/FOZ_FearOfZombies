using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : StateMachineBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    Transform player;
    public float chaseSpeed = 5f;
    public float stopChaseDistance = 15;
    public float attackDistance = 0.25f;
    ZombieManager zombie;
    AudioSource audioSource;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        zombie = animator.GetComponent<ZombieManager>();
        audioSource = animator.GetComponent<AudioSource>();
        agent.speed = chaseSpeed;
        // Get 10% chance to play the sound (to not have to many noises during the gameplay)
        if (Random.value <= 0.1f) audioSource.PlayOneShot(zombie.zombieChaseSound);      
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        // Chase and face the player direction
        agent.SetDestination(player.position);
        animator.transform.LookAt(player);
        // Get the distance from the player
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        // If the player is too far, stop chasing
        if (distanceFromPlayer > stopChaseDistance){
            animator.SetBool("Chasing", false);
        }
        // If the player is close enough, attack
        if (distanceFromPlayer < attackDistance){
            animator.SetBool("Attacking", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        // Stop the agent 
        agent.SetDestination(animator.transform.position);
    }
}
