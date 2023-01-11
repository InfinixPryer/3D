using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    InputActions inputActions;
    Rigidbody rb;
    Animator animator;

    //MOVEMENT VARIABLES
    Vector2 movementInput;
    bool movePressed;
    [SerializeField] float moveSpeed;
    Vector3 moveDir;
    Vector3 runMoveDir;
    bool runPressed;
    [SerializeField] float runMultiplier;

    //ROTATION VARIABLES
    [SerializeField] float timeToRotate;

    //JUMP VARIABLES
    bool jumpPressed; 
    bool isJumping;
    [SerializeField] float initialJumpForce = 20f;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpTime;
    [SerializeField] float jumpTimer = 0;

    //GROUNDING VARIABLES
    bool isGrounded;
    RaycastHit groundHit;
    bool groundCheck;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundLayerMask;

    //GRAVITY VARIABLES
    [SerializeField] float gravity;
    [SerializeField] float groundedGravity;

    void Awake(){
        inputActions = new InputActions(); 
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        inputActions.CharacterControls.Move.started += OnMovementPressed;
        inputActions.CharacterControls.Move.performed += OnMovementPressed;
        inputActions.CharacterControls.Move.canceled += OnMovementPressed;
        inputActions.CharacterControls.Run.started += OnRunPressed;
        inputActions.CharacterControls.Run.canceled += OnRunPressed;
        inputActions.CharacterControls.Jump.started += OnJumpPressed;
        inputActions.CharacterControls.Jump.canceled += OnJumpPressed;
        
    }

    void OnJumpPressed(InputAction.CallbackContext ctx){
        jumpPressed = ctx.ReadValueAsButton();
    }

    void OnRunPressed(InputAction.CallbackContext ctx){
        runPressed = ctx.ReadValueAsButton();
    }

    void OnMovementPressed(InputAction.CallbackContext ctx){
        movementInput = ctx.ReadValue<Vector2>();
        
        moveDir = new Vector3 (movementInput.x, 0, movementInput.y) * rb.mass;
        runMoveDir = moveDir * runMultiplier;

        movePressed = movementInput.x != 0 || movementInput.y != 0;
    }

    void Update(){
        HandleRotation();
        
    }

    void HandleGravity(){
        if ( !isGrounded && !jumpPressed){
            rb.AddForce(Vector3.down * gravity, ForceMode.Force);
        }
        
    }

    void FixedUpdate(){
        GroundCheck();
        HandleMovement();
        HandleJump();
        HandleGravity();
    }

    void HandleJump(){
        if ( jumpPressed && !isJumping) {
            rb.velocity = new Vector2 ( rb.velocity.x, 0);
            rb.AddForce( Vector3.up * rb.mass * initialJumpForce , ForceMode.Impulse);
            isJumping = true;
        }

        if ( isJumping && jumpPressed && jumpTimer <= jumpTime){
            Debug.Log("WOOOOOOW");
            rb.AddForce( Vector3.up * rb.mass * jumpForce, ForceMode.Force);
            jumpTimer += Time.fixedDeltaTime;
        }
        if ( isGrounded ){
            jumpTimer = 0;
        }
    }

    void GroundCheck(){
        bool groundCheck = Physics.Raycast(transform.position, Vector3.down, out groundHit, groundCheckDistance, groundLayerMask);
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.green );
        isJumping = false;
        isGrounded = groundCheck;
    }

    void HandleRotation(){
        Vector3 positionToLookAt;

        positionToLookAt = new Vector3 (movementInput.x, 0f, movementInput.y);

        Quaternion currentRotation = transform.rotation;

        if(movePressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, timeToRotate * Time.deltaTime);
        }
    }

    void HandleMovement(){
        if(runPressed)
        {
            rb.AddForce(runMoveDir * moveSpeed, ForceMode.Force);
        }
        else{
            rb.AddForce(moveDir * moveSpeed, ForceMode.Force);
        }
    }

    void OnEnable(){
        inputActions.CharacterControls.Enable();
    }

    void OnDisable(){
        inputActions.CharacterControls.Disable();
    }
}
