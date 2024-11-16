using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    #region Movement 
    [Header("Movement")]
    public float currentMoveSpeed;
    public float walkSpeed = 5, walkBackSpeed = 4;
    public float runSpeed = 8, runBackSpeed = 7;
    public float crouchSpeed = 3, crouchBackSpeed = 2;
    public float airSpeed = 3;
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public float hzInput, vInput;
    CharacterController controller;
    #endregion

    #region Ground Check 
    [Header("Ground Check")]
    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundMask;
    Vector3 spherePos;
    #endregion

    #region Gravity
    [Header("Gravity")]
    [SerializeField] float gravity = 9.81f;
    Vector3 velocity;
    [SerializeField] float jumpForce = 5;
    [HideInInspector] public bool jumped;
    #endregion

    #region States	
    [Header("States")]
    public MovementBaseState previousState;
    public MovementBaseState currentState;
    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public RunState Run = new RunState();
    public CrouchState Crouch = new CrouchState();
    public JumpState Jump = new JumpState();
    [HideInInspector] public Animator anim;
    #endregion

    void Start(){
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        SwitchState(Idle);
    }

    void Update(){
        GetDirectionAndMove();
        Gravity();
        Falling();
        // Update animator values and animation state
        anim.SetFloat("hzInput", hzInput);
        anim.SetFloat("vInput", vInput);
        currentState.UpdateState(this);
    }

    public void SwitchState(MovementBaseState state){
        // Update the current state
        currentState = state;
        // Enter the new state
        currentState.EnterState(this);
    }

    void GetDirectionAndMove(){
        // Get the input from the player
        hzInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
        Vector3 airDir = Vector3.zero;
        // If the player is not grounded, get the direction of the player in the air
        if (!IsGrounded()) airDir = transform.forward * vInput + transform.right * hzInput;
        // Else if the player is grounded, get the direction of the player on the ground
        else dir = transform.forward * vInput + transform.right * hzInput;
        // Move the player
        controller.Move((dir.normalized * currentMoveSpeed + airDir.normalized * airSpeed) * Time.deltaTime);
    }

    public bool IsGrounded(){
        // Get the position of the sphere
        spherePos = new Vector3(transform.position.x, transform.position.y , transform.position.z);// transform.position.y - groundYOffset
        // Check if the sphere is touching the ground
        if(Physics.CheckSphere(spherePos, controller.radius, groundMask))return true; 
        return false; 
    }

    void Gravity(){
        // If the player is not grounded, apply gravity
        if(!IsGrounded()) velocity.y -= gravity * Time.deltaTime; 
        // Give extra force when touching the ground to make the touching smoother 
        else if (velocity.y < 0) velocity.y = -2;
        // Move the player
        controller.Move(velocity * Time.deltaTime); 
    }

    // private void OnDrawGizmos(){
        //Just testing and debugging 
        // Gizmos.color = Color.red;
        // Gizmos.DrawSphere(spherePos, controller.radius - 0.05f);
    // }

    public void JumpForce() => velocity.y += jumpForce; //8

    public void Jumped() => jumped = true;

    void Falling() => anim.SetBool("Falling", !IsGrounded());


}
