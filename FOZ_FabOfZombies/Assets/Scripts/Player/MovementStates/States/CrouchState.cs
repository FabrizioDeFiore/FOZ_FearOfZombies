using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement){
        movement.anim.SetBool("Crouching", true);
    }
    public override void UpdateState(MovementStateManager movement){
        // Check for movement input and key press and transition to other states
        if( Input.GetKey(KeyCode.LeftShift) ) ExitState(movement, movement.Run);
        if( Input.GetKey(KeyCode.C) ) {
            if(movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);
            else ExitState(movement, movement.Walk);
        }
        // Update character speed depending on direction 
        if (movement.vInput < 0) movement.currentMoveSpeed = movement.crouchBackSpeed;
        else movement.currentMoveSpeed = movement.crouchSpeed;
    }
    void ExitState(MovementStateManager movement, MovementBaseState state){
        movement.anim.SetBool("Crouching", false);
        movement.SwitchState(state);
    }
}
