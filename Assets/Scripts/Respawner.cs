using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private float respawnTime;

    public CheckPoint lastCheckPoint;

    private void Start()
    {
        if (player == null)
            player = GetComponent<Player>();
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnTime);
        transform.position = lastCheckPoint.transform.position;
        player.Ressurrect();
        player.OwnerLevel.enemiesRespawner.RespawnEnemies();
    }
}
