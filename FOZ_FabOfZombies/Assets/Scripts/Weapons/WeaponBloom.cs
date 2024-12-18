using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBloom : MonoBehaviour
{
    [SerializeField] float defaultBloomAngle = 3f;
    [SerializeField] float walkBloomMultiplier = 1.5f;
    [SerializeField] float crouchBloomMultiplier = 0.5f;
    [SerializeField] float runBloomMultiplier = 2f;
    [SerializeField] float aimBloomMultiplier = 0.5f;

    MovementStateManager movement;
    AimStateManager aiming;
    
    float currentBloom;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponentInParent<MovementStateManager>();
        aiming = GetComponentInParent<AimStateManager>();
    }

    public Vector3 BloomAngle (Transform barrelPos){
        // Calculate the current bloom based on the movement state
        if (movement.currentState == movement.Idle) currentBloom = defaultBloomAngle;
        else if (movement.currentState == movement.Walk) currentBloom = defaultBloomAngle * walkBloomMultiplier;
        else if (movement.currentState == movement.Run) currentBloom = defaultBloomAngle * runBloomMultiplier;
        else if (movement.currentState == movement.Crouch) {
            if (movement.dir.magnitude == 0) currentBloom = defaultBloomAngle * crouchBloomMultiplier;
            else currentBloom = defaultBloomAngle * crouchBloomMultiplier * walkBloomMultiplier;
        } 
        if (aiming.currentState == aiming.Aim) currentBloom *= aimBloomMultiplier;
        // Calculate the random rotation for the bloom
        float randX = Random.Range(-currentBloom, currentBloom);
        float randY = Random.Range(-currentBloom, currentBloom);
        float randZ = Random.Range(-currentBloom, currentBloom);
        Vector3 randomRotation = new Vector3 (randX, randY, randZ);
        // Get the randomized angle based on the barrel position
        return barrelPos.localEulerAngles + randomRotation;
    }

}
