using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float timeToDestroy;
    [HideInInspector] public WeaponManager weapon;
    [HideInInspector] public Vector3 dir;

    // Start is called before the first frame update
    void Start(){
        // Destroy the bullet after a certain amount of time
        Destroy(this.gameObject, timeToDestroy);
    }


    private void OnCollisionEnter(Collision collision) {
        // Check if the bullet hit a zombie
        if (collision.gameObject.GetComponentInParent<ZombieHealth>()){
            // Get Health component from the zombie
            ZombieHealth zombieHealth = collision.gameObject.GetComponentInParent<ZombieHealth>();
            // Deal damage to the zombie
            zombieHealth.TakeDamage(weapon.damage);  
            if (zombieHealth.health <= 0 && zombieHealth.isDead == false){
                // Apply force to the zombie ragdoll
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(dir * weapon.zombieKickBackForce, ForceMode.Impulse);
                // Set the zombie as dead (so bullet force is not applied again)
                zombieHealth.isDead = true;
            // If the zombie is not dead create blood effect on the hit position
            } else if (zombieHealth.isDead == false){
                GameObject blood = Instantiate(weapon.bloodEffect, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));    
                // Destroy the effect after a certain amount of time
                Destroy(blood, timeToDestroy);
            }
        }
        // Destroy the bullet when it hits something
        Destroy(this.gameObject);
    }
}
