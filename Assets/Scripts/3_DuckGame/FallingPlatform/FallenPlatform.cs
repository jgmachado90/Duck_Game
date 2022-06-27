using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CollisionSystem;
using System;

public class FallenPlatform : MonoBehaviour
{
    [SerializeField] private TriggerEnter2D triggerEnter2D;
    [SerializeField] private Rigidbody2D rb;

    private void OnEnable()
    {
        triggerEnter2D.OnCollisionEnter += CollideWithPlatform;
    }

    private void OnDisable()
    {
        triggerEnter2D.OnCollisionEnter -= CollideWithPlatform;
    }

    private void CollideWithPlatform(GameObject obj)
    {
        rb.isKinematic = false;
    }
}
