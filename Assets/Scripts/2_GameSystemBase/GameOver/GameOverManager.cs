using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CheckPointSystem;
using GameFramework;
public class GameOverManager : Entity, ISubsystem<GameMode>
{
    public GameMode OwnerManager { get; set; }

    private CheckPointManager checkPointManager;
    private RespawnablesManager respawnablesManager;

    private void Start()
    {
        checkPointManager = this.GetGameModeSubsystem<CheckPointManager>();
        respawnablesManager = this.GetGameModeSubsystem<RespawnablesManager>();
    }

    public void GameOver()
    {
        checkPointManager.ResetPlayer();
        respawnablesManager.RespawnEntities();
    }
}
