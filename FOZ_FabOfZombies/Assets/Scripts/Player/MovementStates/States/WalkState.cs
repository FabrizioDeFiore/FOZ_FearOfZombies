using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement){
        movement.anim.SetBool("Walking", true);
    }
    public override void UpdateState(MovementStateManager movement){
        // Check for movement input and key press and transition to other states
        if(Input.GetKey(KeyCode.LeftShift)) ExitState(movement, movement.Run);
        else if(Input.GetKey(KeyCode.C)) ExitState(movement, movement.Crouch);
        else if(movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);
        // Update character speed depending on direction 
        if (movement.vInput < 0) movement.currentMoveSpeed = movement.walkBackSpeed;
        else movement.currentMoveSpeed = movement.walkSpeed;
        // Remember current state before transitioning to jump state
        if (Input.GetKeyDown(KeyCode.Space)) {
            movement.previousState = this;
            ExitState(movement, movement.Jump);
        } 
    }
    
    void ExitState(MovementStateManager movement, MovementBaseState state){
        movement.anim.SetBool("Walking", false);
        movement.SwitchState(state);
    }
}
