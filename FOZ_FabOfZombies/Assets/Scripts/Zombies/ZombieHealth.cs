using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float health;
    RagdollManager ragdollManager;
    [HideInInspector] public bool isDead;
    Animator anim;
    ZombieManager zombie;
    AudioSource audioSource;
    PlayerManager player;

    private void Start(){
        anim = GetComponent<Animator>();
        ragdollManager = GetComponent<RagdollManager>();
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
        if (ragdollManager == null) Debug.Log("RagdollManager not found");
        zombie = GetComponent<ZombieManager>();
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage (float damage){
        if (health > 0){
            health -= damage;
            anim.SetTrigger("Hit");
            // Get 10% chance to play the sound (to not have to many noises during the gameplay)
            if (Random.value <= 0.1f) audioSource.PlayOneShot(zombie.zombieHitSound);      
            if (health <= 0) ZombieDeath();
            else player.money += 10;
        }
    }

    void ZombieDeath(){
        ragdollManager.TriggerRagdoll();
        // Get 10% chance to play the sound (to not have to many noises during the gameplay)
        if (Random.value <= 0.1f) audioSource.PlayOneShot(zombie.zombieDeathSound);      
        player.money += 50;
    }
}
