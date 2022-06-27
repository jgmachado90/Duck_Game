using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Entity
{
    [Header("Components")]
    private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private InputController inputController;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayer = new LayerMask();

    [Header("Movement Variables")]
    [SerializeField] private float movementAcceleration;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float groundLinearDrag;

    private float horizontalDirection;
    private bool changingDireciton => (rb.velocity.x > 0f && horizontalDirection < 0f) || (rb.velocity.x < 0f && horizontalDirection > 0f);

    [Header("JumpVariables")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float airLinearDrag = 2.5f;

    [Header("Ground Collision Variables")]
    [SerializeField] private float groundRaycastLength;
    private bool onGround;


    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (sprite == null)
            sprite = GetComponentInChildren<SpriteRenderer>();
        if (inputController == null)
            inputController = this.GetLevelManagerSubsystem<InputController>();
    }

    private void Update()
    {
        if(inputController != null)
        {
            horizontalDirection = inputController.GetMovementInput().x;
        }
    }

    private void FixedUpdate()
    {
        CheckCollisions();
        MovePlayer();
        FlipPlayer();
        if (onGround)
        {
            ApplyGroundLinearDrag();
        }
        else
        {
            ApplyAirLinearDrag();
        }
    }

    private void FlipPlayer()
    {
        if (horizontalDirection > 0) sprite.flipX = false;
        if (horizontalDirection < 0) sprite.flipX = true;
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MovePlayer()
    {
        rb.AddForce(new Vector2(horizontalDirection, 0f) * movementAcceleration);

        if (Mathf.Abs(rb.velocity.x) > maxMoveSpeed)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxMoveSpeed, rb.velocity.y);
    }

    private void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(horizontalDirection) < 0.4f || changingDireciton)
        {
            rb.drag = groundLinearDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }
    private void ApplyAirLinearDrag()
    {
        rb.drag = airLinearDrag;
    }

    public void Jump(float force = -1)
    {
        if (force < 0)
            force = jumpForce;

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }



    private void CheckCollisions()
    {
        Physics2D.queriesHitTriggers = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up *groundRaycastLength, groundRaycastLength, groundLayer);
        Physics2D.queriesHitTriggers = true;
        onGround = hit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * groundRaycastLength);
    }

}