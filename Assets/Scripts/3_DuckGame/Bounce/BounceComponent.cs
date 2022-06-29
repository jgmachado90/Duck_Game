using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Health;
using CollisionSystem;
using MoreMountains.Feedbacks;

public class BounceComponent : MonoBehaviour
{
    public bool damageWhenBounce;
    public float bounceForce;
    public HealthComponent health;
    public TriggerEnter2D triggerEnter2D;
    public MMFeedbacks bounceFeedback;

    private void OnEnable()
    {
        if (triggerEnter2D != null)
        {
            triggerEnter2D.OnCollisionEnter += OnCollideWith;
        }
    }
    private void OnDisable()
    {
        if (triggerEnter2D != null)
        {
            triggerEnter2D.OnCollisionEnter -= OnCollideWith;
        }
    }
    public void OnCollideWith(GameObject obj)
    {
        IBouncable bouncable = obj.GetComponentInParent<IBouncable>();
        if(bouncable != null)
        {
            if(bounceFeedback != null)
                bounceFeedback.PlayFeedbacks();
            bouncable.Bounce(bounceForce);
            if (damageWhenBounce)
                ApplyDamage();
        }
    }

    public void ApplyDamage()
    {
        health.ForceDamage();
    }
}
