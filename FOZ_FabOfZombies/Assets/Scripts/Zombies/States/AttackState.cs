using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    Transform player;
    UnityEngine.AI.NavMeshAgent agent;
    public float stopAttackDistance = 0.25f;
    ZombieManager zombie;
    AudioSource audioSource;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        zombie = animator.GetComponent<ZombieManager>();
        audioSource = animator.GetComponent<AudioSource>();
        // Get 10% chance to play the attack sound (to not have to many noises during the gameplay)
        if (Random.value <= 0.1f) audioSource.PlayOneShot(zombie.zombieAttackSound);      
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        LookAtPlayer();
        // Get the distance from the player
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        // If the player is too far, stop attacking
        if (distanceFromPlayer > stopAttackDistance){
            animator.SetBool("Attacking", false);
        }
    }

    private void LookAtPlayer(){
        // Rotate the agent to face the player
        Vector3 direction = (player.position - agent.transform.position); //agent.transform.position).normalized
        agent.transform.rotation = Quaternion.LookRotation(direction);
        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        // Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        // agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
