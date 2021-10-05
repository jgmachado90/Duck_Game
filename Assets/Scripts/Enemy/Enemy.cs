using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor, IKillable
{
    public Vector3 spawnPosition;

    public float bounceForce;

    [SerializeField] private int health;
    [SerializeField] private bool dead;
    public int Health { get { return health; } set { health = value; } }
    public bool Dead { get { return dead; } set { dead = value; } }

    private void Awake()
    {
        dead = false;
        spawnPosition = transform.position;
    }

    public virtual void Respawn()
    {
        dead = false;
        gameObject.SetActive(true);
        transform.position = spawnPosition;
    }

    public virtual void Death()
    {
        dead = true;
    }

    public virtual void Hit()
    {
        Health--;
        if (Health < 1)
        {
            Death();
        }
    }

    public virtual void ApplyDamage(Player player)
    {
        player.Hit();
    }

    public virtual void BounceEffect()
    {

    }



}
