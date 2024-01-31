using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 2D Player Controller

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float airSpeed = 3f;
    TouchingDirections touchingDirections;
    public float jumpImpulse = 10f;

    public float CurrentSpeed { get 
        {
            if (IsMoving && !touchingDirections.IsOnWall)
            {
                if (touchingDirections.IsGrounded)
                {
                    if (IsRunning)
                    {
                        return runSpeed;
                    }
                    else
                    {
                        return walkSpeed;
                    }
                } else
                {
                    if (IsRunning)
                    {
                        return airSpeed + 3;
                    }
                    else
                    {
                        return airSpeed;
                    }
                }
            }else
            {
                // Not moving
                return 0;
            }
        } 
    }

    Rigidbody2D rb;
    Animator animator;
    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving { 
        get 
        {
            return _isMoving;
        } 
        private set 
        { 
            _isMoving = value;
            if (value)
            {
                animator.SetBool(AnimationStrings.isMoving, true);
            }
            else
            {
                animator.SetBool(AnimationStrings.isMoving, false);
            }
        }
    }
    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning { 
        get 
        {
            return _isRunning;
        } 
        private set 
        { 
            _isRunning = value;
            if (value)
            {
                animator.SetBool(AnimationStrings.isRunning, true);
            }
            else
            {
                animator.SetBool(AnimationStrings.isRunning, false);
            }
        } 
    }

    public bool _isFacingRight = true;
    

    public bool IsFacingRight { get { return _isFacingRight; } private set {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1); 
            }
            _isFacingRight = value;
        } 
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if(context.canceled)
        {
            IsRunning = false;
        } 
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //TODO check if alive as well
        if (context.started && touchingDirections.IsGrounded)
        {
            animator.SetTrigger(AnimationStrings.jump);
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
    }
}
