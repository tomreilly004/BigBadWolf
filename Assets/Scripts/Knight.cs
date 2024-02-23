using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Knight : MonoBehaviour
{
    public float walkSpeed = 3f;
    Rigidbody2D rb;
    TouchingDirections touchingDirections;

    public enum WalkingDirection
    {
        Right,
        Left
    }

    private WalkingDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkingDirection WalkDirection
    {
        get
        {
            return _walkDirection;
        }
        set
        {
            if(_walkDirection != value)
            {
                //Direction flipped
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if(value == WalkingDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                } else if(value == WalkingDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void FixedUpdate()
    {
        if(touchingDirections.IsOnWall && touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
        rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
    }

    private void FlipDirection()
    {
        if(WalkDirection == WalkingDirection.Right)
        {
            WalkDirection = WalkingDirection.Left;
        } else if(WalkDirection == WalkingDirection.Left)
        {
            WalkDirection = WalkingDirection.Right;
        } else
        {
            Debug.LogError("Invalid walk direction. Not set to right or left");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
