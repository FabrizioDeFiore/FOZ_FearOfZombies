using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    private Animator anim;
    private UnityEngine.AI.NavMeshAgent agent;
    public float damage = 20;
    AudioSource audioSource;
    public AudioClip zombieWalkSound;
    public AudioClip zombieChaseSound;
    public AudioClip zombieAttackSound;
    public AudioClip zombieDeathSound;
    public AudioClip zombieHitSound;
    // Start is called before the first frame update
    void Start()
    {
        // anim = GetComponent<Animator>();
        // agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //zombieHand = GetComponentInChildren<ZombieHand>();
        //if (zombieHand == null) Debug.Log("ZombieHand not found");
        //zombieDamage = zombieHand.damage;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrawGizmos(){
        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, .75f);
        // Detection range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 10);
        // Stop chasing range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 15);
    }
}
