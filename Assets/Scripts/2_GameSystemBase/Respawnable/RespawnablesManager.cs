using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class RespawnablesManager : Entity, ISubsystem<GameMode>
{
    public GameMode OwnerManager { get; set; }

    public void RespawnEntities()
    {
        IRespawnable[] respawnables = GetComponentsInChildren<IRespawnable>();
        foreach(IRespawnable respawnable in respawnables)
        {
            respawnable.Respawn();
        }
    }
}
