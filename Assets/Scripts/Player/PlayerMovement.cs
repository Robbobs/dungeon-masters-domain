using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class PlayerMovement : MonoBehaviour{
    private CharacterController controller;
    public GameObject body;

    public float baseSpeed = 5.0f;
    public float runSpeedModifier = 8f;
    public float crouchSpeedModifier = 0.5f;
    public float verticalSpeed = 0f;
    
    public float baseJump = 5.0f;
    public float jumpMultiplier = 1.2f;
    public float doubleJumpMultiplier = 0.8f;

    public float distanceFromGround = 0f;
    
    public bool isJumping = false;
    public bool isDoubleJumping = false;
    public bool isCrouching = false;
    
    private Vector3 movementDirection;
    private Animator animator;

    bool jumpPressed = false;

    Vector3 HandleHorizontalMove(){
        float currentHorizontalSpeed = baseSpeed;
        if(Input.GetButton(Control.Run)){
            currentHorizontalSpeed *= runSpeedModifier;
            this.animator.SetBool(HumanoidAnimation.Sprint, true);
        }else{
            this.animator.SetBool(HumanoidAnimation.Sprint, false);
        }
        if(this.isCrouching){
            currentHorizontalSpeed *= crouchSpeedModifier;
        }

        float movementNorm = currentHorizontalSpeed * Time.deltaTime;


        Vector3 movementDirection = new Vector3(
            Input.GetAxis(Control.HorizontalAxis), 0, Input.GetAxis(Control.VerticalAxis)
        );

        if(movementDirection == Vector3.zero){
            this.animator.SetBool(HumanoidAnimation.Walk, false);
            this.animator.SetBool(HumanoidAnimation.Sprint, false);
        }else{
            this.animator.SetBool(HumanoidAnimation.Walk, true);
            this.body.transform.forward = movementDirection;
        }

        return movementNorm * movementDirection;
    }
    
    float HandleFall(){
        return Constant.g * Time.deltaTime;
    }

    (float, bool) HandleJump(){
        var ySpeed = 0f;
        var resetSpeed = false;

        if(this.controller.isGrounded){
            this.isJumping = false;
            this.isDoubleJumping = false;
        }

        if(this.jumpPressed){
            this.jumpPressed = false;  

            if(!this.isJumping){
                this.isJumping = true;
                ySpeed = this.baseJump * this.jumpMultiplier;
                this.animator.SetTrigger(HumanoidAnimation.Jump);
            }else if (!this.isDoubleJumping && this.distanceFromGround >= 0.5f){
                ySpeed = this.baseJump * this.doubleJumpMultiplier;
                resetSpeed = true;
                this.isDoubleJumping = true;
                this.animator.SetTrigger(HumanoidAnimation.DoubleJump);
            }
        }

        return (ySpeed, resetSpeed);
    }

    Vector3 HandleVerticalMove(){
        if(this.controller.isGrounded){
            verticalSpeed = 0;
        }
        verticalSpeed += this.HandleFall();

        var (jumpSpeed, resetSpeed) = this.HandleJump();
        if(resetSpeed){
            verticalSpeed = 0;
        }
        verticalSpeed += jumpSpeed;

        return verticalSpeed * Time.deltaTime * transform.up;
    }

    void HandleMove(){
        var horizontalMove = this.HandleHorizontalMove();
        var VerticalMove = this.HandleVerticalMove();

        var deslocation = horizontalMove + VerticalMove;
        controller.Move(deslocation);

        var isGrounded = this.controller.isGrounded;
        var ungroundedTime = 0f;
        if(!isGrounded)
            ungroundedTime = this.animator.GetFloat(HumanoidAnimation.UngroundedTime) + Time.deltaTime;    
        this.animator.SetFloat(HumanoidAnimation.UngroundedTime, ungroundedTime);
        this.animator.SetBool(HumanoidAnimation.IsGrounded, this.controller.isGrounded);
    }

    void HandleCrouch(){
        this.isCrouching = Input.GetButton(Control.Crouch);
        this.animator.SetBool(HumanoidAnimation.IsCrouched, this.isCrouching);
    }

    float GroundDistance(){
        var playerFeetYPosition = -this.controller.height/2f;
        var playerFeetVector = new Vector3(
            this.controller.center.x,
            this.controller.center.y + playerFeetYPosition,
            this.controller.center.z
        ) + this.transform.position;

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(playerFeetVector, -this.transform.up, out hit)){
            return hit.distance;
        }
        return 0f;
    }

    void Start(){
        this.controller = this.GetComponent<CharacterController>();
        this.animator = this.GetComponent<Animator>();
    }

    void Update(){
        if(Input.GetButtonDown(Control.Jump)){
            this.jumpPressed = true;
        }
    
        if(Input.GetButtonDown(Control.Crouch)){
            this.animator.SetTrigger(HumanoidAnimation.Crouching);
        }
    }

    void FixedUpdate(){
        this.HandleCrouch();
        this.HandleMove();
        
        this.distanceFromGround = this.GroundDistance();
        this.animator.SetFloat(HumanoidAnimation.DistanceFromGround, this.distanceFromGround);
    }
}
