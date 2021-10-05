using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public LayerMask groundLayer;
    public Rigidbody2D rb;
    public bool platformMover;
    public int direction = 1;
    public float speed;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        GroundedMovement();
    }

    public virtual void GroundedMovement()
    {
        if (IsGrounded())
        {
            transform.position = new Vector2(transform.position.x + speed * direction * Time.deltaTime, transform.position.y);
        }
        else
        {
            direction *= -1;
            transform.position = new Vector2(transform.position.x + speed * direction * Time.deltaTime, transform.position.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.transform.GetComponent<Enemy>();
        if (enemy)
            direction *= -1;
    }

    public bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }



}
