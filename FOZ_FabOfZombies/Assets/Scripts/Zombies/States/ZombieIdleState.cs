using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState : StateMachineBehaviour
{
    float timer;
    public float idleTime = 0f;
    Transform player;
    public float detectionAreaRadius = 10f;
    ZombieManager zombie;
    AudioSource audioSource;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        timer = 0f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        zombie = animator.GetComponent<ZombieManager>();
        audioSource = animator.GetComponent<AudioSource>();
        // Get 10% chance to play the sound (to not have to many noises during the gameplay)
        if (Random.value <= 0.1f) audioSource.PlayOneShot(zombie.zombieWalkSound);      
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        // When idle for enough time, transition to patrol state
        timer += Time.deltaTime;
        if (timer >= idleTime){
            animator.SetBool("Patrolling", true);
        }
        // When detecting player, transition to chase state
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionAreaRadius){
            animator.SetBool("Chasing", true);
        }
    }

}
