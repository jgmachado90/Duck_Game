using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityComponent : MonoBehaviour
{
    public float gravity = 10;
    public bool limitVelocityDown = false;
    public float limitVelocityDownValue = 10;

    private PhysicsBody _physics;

    private void Awake()
    {
        _physics = new PhysicsBody(gameObject);
    }

    void FixedUpdate()
    {
        var newVelocity = _physics.Velocity;

        if (gravity > 0)
        {
            newVelocity += Vector3.down * (gravity * Time.fixedDeltaTime);
        }

        if (limitVelocityDown && newVelocity.y < -limitVelocityDownValue)
        {
            newVelocity.y = -limitVelocityDownValue;
        }

        _physics.Velocity = newVelocity;
    }
}
