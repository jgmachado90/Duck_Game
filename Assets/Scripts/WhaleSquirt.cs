using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleSquirt : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private float squirtDelay;
    [SerializeField] private ParticleSystem squirtParticle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement playerMovement = collision.GetComponentInParent<PlayerMovement>();
        if (playerMovement != null)
        {
            StartCoroutine(WhaleSquirtCoroutine(playerMovement)); 
        }
    }

    public IEnumerator WhaleSquirtCoroutine(PlayerMovement playerMovement)
    {
        squirtParticle.Play();
        yield return new WaitForSeconds(squirtDelay);
        playerMovement.Jump(force);
    }
}
