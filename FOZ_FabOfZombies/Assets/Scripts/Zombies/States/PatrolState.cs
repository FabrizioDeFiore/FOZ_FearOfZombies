using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : StateMachineBehaviour
{
    float timer;
    public float patrolingTime = 10f;
    Transform player;
    UnityEngine.AI.NavMeshAgent agent;
    public float detectionArea = 10f;
    public float patrolSpeed = 2f;
    List<Transform> waypointList = new List<Transform>();
    ZombieManager zombie;
    AudioSource audioSource;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>(); //UnityEngine.AI.NavMeshAgent
        zombie = animator.GetComponent<ZombieManager>();
        audioSource = animator.GetComponent<AudioSource>();
        agent.speed = patrolSpeed;
        timer = 0f;
        // Get all waypoints in the map to initialize the waypoint patrol system
        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
        // Add all waypoints to the list 
        foreach (Transform waypoint in waypointCluster.transform){ 
            waypointList.Add(waypoint);
        }
        // Move to the first waypoint (a random one from the list)
        Vector3 nextPosition = waypointList[Random.Range(0, waypointList.Count)].position;
        agent.SetDestination(nextPosition);
        // Get 10% chance to play the sound (to not have to many noises during the gameplay)
        if (Random.value <= 0.1f) audioSource.PlayOneShot(zombie.zombieWalkSound);      
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        // Check if reach a waypoint 
        if (agent.remainingDistance < agent.stoppingDistance){
            // Move to the next waypoint
            Vector3 nextPosition = waypointList[Random.Range(0, waypointList.Count)].position;
            agent.SetDestination(nextPosition);
        }
        // When patrolling for too long without success, transition to idle state
        timer += Time.deltaTime;
        if (timer >= patrolingTime){
            animator.SetBool("Patrolling", false);
        }
        // When detecting player, transition to chase state
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionArea){
            animator.SetBool("Chasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        // Stop the agent 
        agent.SetDestination(agent.transform.position);
    }
}