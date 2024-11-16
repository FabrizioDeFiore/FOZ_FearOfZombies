using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement){

    }
    public override void UpdateState(MovementStateManager movement){
        // Check for movement input and key press and transition to other states
        if (movement.dir.magnitude > 0.1f){
            if (Input.GetKey(KeyCode.LeftShift)) movement.SwitchState(movement.Run);
            else movement.SwitchState(movement.Walk);
        }
        if (Input.GetKey(KeyCode.C)) movement.SwitchState(movement.Crouch);
        // Remember current state before transitioning to jump state
        if (Input.GetKeyDown(KeyCode.Space)) {
            movement.previousState = this;
            movement.SwitchState(movement.Jump);
        } 
    }
}
