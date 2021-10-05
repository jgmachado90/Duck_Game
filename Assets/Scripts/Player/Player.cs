using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : Actor, IKillable
{
    [Header("ExternComponents")]
    [SerializeField] private CinemachineVirtualCamera cameraFollower;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ParticleSystem dieParticle;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private Respawner respawner;

    [Header("Killable Variables")]
    [SerializeField] private int health;
    [SerializeField] private bool dead;
    public int Health { get { return health; } set { health = value; } }
    public bool Dead { get { return dead; } set { dead = value; } }

    private void Start()
    {
        if (sprite == null)
            sprite = GetComponentInChildren<SpriteRenderer>();
        if (playerCollider == null)
            playerCollider = GetComponentInChildren<BoxCollider2D>();
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (respawner == null)
            respawner = GetComponent<Respawner>();
    }
    public void Death()
    {
        Dead = true;
        sprite.enabled = false;
        playerCollider.enabled = false;
        rb.isKinematic = true;
        dieParticle.Play();
        if (cameraFollower)
            cameraFollower.Follow = null;
        respawner.Respawn();
    }
    public void Hit()
    {
        Health--;
        if (Health < 1)
        {
            Death();
        }
    }

    public void Ressurrect() 
    {
        Dead = false;
        sprite.enabled = true;
        playerCollider.enabled = true;
        rb.isKinematic = false;
        rb.velocity = Vector2.zero;
        
        if (cameraFollower)
            cameraFollower.Follow = transform;
        
    }
}
