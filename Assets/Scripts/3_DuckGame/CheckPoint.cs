using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*Respawner playerRespawner = collision.GetComponentInParent<Respawner>();
        if (playerRespawner != null)
        {
            playerRespawner.lastCheckPoint = this;
        }*/
    }
}
