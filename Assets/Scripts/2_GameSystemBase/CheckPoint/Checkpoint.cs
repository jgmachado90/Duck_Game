using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using CollisionSystem;
using System;

namespace CheckPointSystem
{
    public class Checkpoint : Entity
    {
        [SerializeField] TriggerEnter2D triggerEnter;
        public Transform spawnPosition;
        CheckPointManager checkPointManager;


        private void OnEnable()
        {
            triggerEnter.OnCollisionEnter += SetCurrentCheckpoint;
        }

        private void OnDisable()
        {
            triggerEnter.OnCollisionEnter -= SetCurrentCheckpoint;
        }

        public void Start()
        {
            checkPointManager = this.GetGameModeSubsystem<CheckPointManager>();
        }
        private void SetCurrentCheckpoint(GameObject obj)
        {
            checkPointManager.SetCurrentCheckPoint(this);
        }
    }
}