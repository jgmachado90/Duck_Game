using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using Health;

public class Player : Entity
{
    [Header("Components")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {

    }

    public void ApplyBounce(float force)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

}