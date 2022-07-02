using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Health;

public class JellyFish : Enemy, IRespawnable
{
    [SerializeField] private int health;
    [SerializeField] Transform content;
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] Rigidbody2D rb;
    private Vector3 initPos;

    public Water water;
    public bool floating;
    private void Update()
    {
        if (floating)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            water.Float(transform.position.x, content);         
        }
    }

    private void OnEnable()
    {
        healthComponent.OnDeath += Death;
    }
    private void OnDisable()
    {
        healthComponent.OnDeath -= Death;
    }

    private void Death()
    {
        content.gameObject.SetActive(false);
    }

    private void Start()
    {
        initPos = content.position;
    }
    public void Respawn()
    {
        content.position = initPos;
        if(!healthComponent.IsAlive)
            healthComponent.AddHealth(health);
        content.gameObject.SetActive(true);
    }
}
