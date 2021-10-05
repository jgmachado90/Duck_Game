using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceSkill : Skill
{
    [SerializeField] private PlayerMovement playerMovement;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Bounce")
        {
            Enemy enemy = collision.GetComponentInParent<Enemy>();
            if (!enemy.Dead)
            {
                playerMovement.Jump();
                enemy.Hit();
            }
        }
    }
}
