using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFishHead : Enemy
{
    [SerializeField] private ParticleSystem dieParticle;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private BoxCollider2D enemyCollider;

    private void Start()
    {
        if (sprite == null)
            sprite = GetComponentInChildren<SpriteRenderer>();
        if (enemyCollider == null)
            enemyCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.transform.GetComponentInParent<Player>();
        if(player != null)
        {
            ApplyDamage(player);
        }
    }

    public override void Death()
    {
        base.Death();
        sprite.enabled = false;
        enemyCollider.enabled = false;
        dieParticle.Play();
        Invoke("DisableGameObject", 1f);
    }

    public override void Respawn()
    {
        base.Respawn();
        sprite.enabled = true;
        enemyCollider.enabled = true;
    }

    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
