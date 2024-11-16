using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    Rigidbody[] rbs;
    private Animator anim;
    //private CharacterController animController;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //animController = GetComponent<CharacterController>();
        // Get all the rigidbodies in the ragdoll
        rbs = GetComponentsInChildren<Rigidbody>();
        // Turn off the ragdoll physics
        foreach (Rigidbody rb in rbs) rb.isKinematic = true;
    }

    public void TriggerRagdoll(){
        // Disable the Animator
        anim.enabled = false;
        // Disable the Character Controller
        //animController.enabled = false;
        // Turn on the ragdoll physics
        foreach (Rigidbody rb in rbs) rb.isKinematic = false;
    }
}
