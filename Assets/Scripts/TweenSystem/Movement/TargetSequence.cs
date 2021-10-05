using System;
using CoreInterfaces;
using UnityEngine;

namespace TweenSystem
{
    public class TargetSequence : MonoBehaviour
    {
        public TargetInfo[] targets;

        private int _currentTargetId;
    
        [Serializable]
        public class TargetInfo
        {
            public Transform target;
            public Cooldown cooldown;
        }

        private void Start()
        {
            TargetInfo currentTarget = targets[_currentTargetId];
            currentTarget.cooldown.Init();
            transform.position = currentTarget.target.position;
        }

        private void Update()
        {

            if (targets[_currentTargetId].cooldown.Finished())
            {
                _currentTargetId++;
                if (_currentTargetId >= targets.Length) _currentTargetId = 0;
            
                TargetInfo currentTarget = targets[_currentTargetId];
                currentTarget.cooldown.Init();
                transform.position = currentTarget.target.position;
            }
        }
    }
}
