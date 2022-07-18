using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using Health;
using CheckPointSystem;
public class Player : Entity, IBouncable
{
    [Header("Components")]
    [SerializeField] private int health = 1;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public SpriteRenderer MySpriteRenderer
    {
        get { return spriteRenderer; }
    }


    public PlayerMovement PlayerMovement
    {
        get { return playerMovement; }
    }

    public Transform content;
    private GameOverManager gameOverManager;

    private void OnEnable()
    {
        healthComponent.OnDeath += Death;
    }
    private void OnDisable()
    {
        healthComponent.OnDeath -= Death;
    }

    private void Start()
    {
        gameOverManager = this.GetGameModeSubsystem<GameOverManager>();
    }

    private void Death()
    {
        gameOverManager.GameOver();
        healthComponent.AddHealth(health);
    }

    public void Bounce(float force)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }
}