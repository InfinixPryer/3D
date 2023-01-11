using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("State")]
    public string stateName;

    //MOVEMENT VARIABLES
    [Header("Move")]
    public float moveSpeed = 200f;
    public float timeToRotate = 15f;
    Vector2 movementInput;
    Vector3 movementDirection;
    public bool isMovePressed = false;

    //RUN VARIABLES
    [Header("Run")]
    public float runMultiplier = 2f;
    Vector3 runMovementDirection;
    bool isRunPressed = false;

    //JUMP VARIABLES
    [Header("Jump")]
    public float jumpForce = 30f;
    public float initialJumpForce = 30f;
    public float jumpTime = 2f;
    bool isJumpPressed = false;
    bool isJumping = false;
    float jumpTimer = 0;
    
    //GROUNDING VARIABLES
    [Header("Ground")]
    public LayerMask groundLayerMask;
    public float groundDetectLength;
    bool isGrounded = true;
    bool groundDetect = true;
    RaycastHit groundHit;
    Ray groundRay;

    //GRAVITY VARIABLES
    [Header("Gravity")]
    public float gravity = 200f;
    public float groundedGravity = 100f;

    //State Variables
    PlayerBaseState currentState;
    PlayerStateFactory states;
    
    #region GETTERS AND SETTERS
    public PlayerBaseState CurrentState { get { return currentState; } set { currentState = value; } }
    public Rigidbody Rb { get { return rb; } }
    public Animator Animator { get {return animator; } }
    
    public bool IsMovePressed { get { return isMovePressed;} set { isMovePressed = value; } }
    public bool IsJumpPressed { get { return isJumpPressed;} set { isJumpPressed = value; } }
    public bool IsRunPressed { get { return isRunPressed;} set { isRunPressed = value; } }
    public bool IsGrounded { get { return isGrounded; } set { isGrounded = value; } }
    public bool IsJumping { get { return isJumping; } set { isJumping = value; } }
    public RaycastHit GroundHit {  get { return groundHit; } set { groundHit = value; } }

    public Vector3 MovementDirection { get { return movementDirection; } set { movementDirection = value; } }
    public Vector3 RunMovementDirection { get { return runMovementDirection; } set { runMovementDirection = value; } }

    public float InitialJumpForce { get { return initialJumpForce; } set { initialJumpForce = value; } }
    public float JumpForce { get { return jumpForce; } set { jumpForce = value; } }
    public float JumpTime { get { return jumpTime; } set { jumpTime = value; } }
    public float JumpTimer { get { return jumpTimer; } set { jumpTimer = value; } }
    public float Gravity { get { return gravity; } set { gravity = value; } }
    public float GroundedGravity { get { return groundedGravity; } set { groundedGravity = value; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    


    #endregion

    InputActions inputActions;
    Rigidbody rb;
    Animator animator;

    void Awake(){
        states = new PlayerStateFactory(this);
        currentState = states.Ground();
        currentState.EnterState();

        inputActions = new InputActions();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        inputActions.CharacterControls.Move.started += OnMove;
        inputActions.CharacterControls.Move.performed += OnMove;
        inputActions.CharacterControls.Move.canceled += OnMove;

        inputActions.CharacterControls.Jump.started += OnJump;
        inputActions.CharacterControls.Jump.canceled += OnJump;

        inputActions.CharacterControls.Run.started += OnRun;
        inputActions.CharacterControls.Run.canceled += OnRun;
    }

    void OnMove(InputAction.CallbackContext context){
        movementInput = context.ReadValue<Vector2>();

        movementDirection.x = movementInput.x;
        movementDirection.y = 0;
        movementDirection.z = movementInput.y;

        runMovementDirection = movementDirection;
        runMovementDirection.x = movementDirection.x * runMultiplier;
        runMovementDirection.z = movementDirection.z * runMultiplier; 

        isMovePressed = movementDirection.x != 0 || movementDirection.z != 0;
    }

    void OnRun(InputAction.CallbackContext context){
        isRunPressed = context.ReadValueAsButton();
    }

    void OnJump(InputAction.CallbackContext context){
        isJumpPressed = context.ReadValueAsButton();
    }

    void Start(){

    }

    void Update(){
        HandleRotation();
        stateName = currentState.ToString();
        currentState.UpdateStates();
    }

    void FixedUpdate(){
        GroundCheck();
        currentState.FixedUpdateStates();
    }
    
    void HandleRotation() {
        Vector3 positionToLookAt = new Vector3 (movementDirection.x, 0, movementDirection.z);

        Quaternion currentRotation = transform.rotation;

        if(isMovePressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, timeToRotate * Time.deltaTime);
        }
    }

    void GroundCheck(){
        groundRay = new Ray(transform.position, Vector3.down);
        groundDetect = Physics.Raycast(groundRay, out groundHit, groundDetectLength, groundLayerMask);

        isGrounded = groundDetect;
    }

    void OnEnable() {
        inputActions.Enable();
    }

    void OnDisable() {
        inputActions.Disable();
    }
}
