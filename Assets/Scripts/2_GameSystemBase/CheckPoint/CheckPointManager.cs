using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

namespace CheckPointSystem
{
    public class CheckPointManager : Entity, ISubsystem<GameMode>
    {
        public GameMode OwnerManager { get; set; }

        private Checkpoint currentCheckPoint;

        public void SetCurrentCheckPoint(Checkpoint cP)
        {
            currentCheckPoint = cP;
        }

        public void ResetPlayer()
        {
            OwnerManager.PlayerCharacter.position = currentCheckPoint.spawnPosition.position;
            OwnerManager.PlayerCharacter.gameObject.SetActive(true);
        }

    }
}