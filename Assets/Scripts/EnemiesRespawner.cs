using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class EnemiesRespawner : Entity
{
    [SerializeField] private List<Enemy> enemyList = new List<Enemy>();

    private void Start()
    {
        foreach (Transform child in transform)
        {
            Enemy e = child.GetComponent<Enemy>();
            enemyList.Add(e);
        }
    }


    public void RespawnEnemies()
    {
        foreach(Enemy e in enemyList)
        {
            e.Respawn();
        }
    }

}
