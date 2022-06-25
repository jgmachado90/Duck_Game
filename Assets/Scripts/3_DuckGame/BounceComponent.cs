using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Health;
using CollisionSystem;

public class BounceComponent : MonoBehaviour
{
    public bool damageWhenBounce;
    public float bounceForce;
    public HealthComponent health;
    public TriggerEnter2D triggerEnter2D;

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
        Player player = obj.GetComponentInParent<Player>();
        if(player != null)
        {
            player.ApplyBounce(bounceForce);
            if (damageWhenBounce)
                ApplyDamage();
        }
    }

    public void ApplyDamage()
    {
        health.ForceDamage();
    }
}
