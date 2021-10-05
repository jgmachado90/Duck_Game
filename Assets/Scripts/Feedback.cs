﻿using System;
using System.Collections.Generic;
using CoreInterfaces;
using Pixel;
using UnityEngine;

namespace GameFramework
{
    [Serializable]
    public class Feedback
    {
        [Header("Invoke Params")]
        public Instantiate[] spawn;
        public MonoBehaviour[] actions;

        private List<IActionMonoBehaviour> _actionMonoBehaviour;

        public void Invoke(Entity ownerEntity) {
            for (int i = 0; i < spawn.Length; i++)
            {
                spawn[i].Invoke(ownerEntity);
            }

            if(_actionMonoBehaviour == null)
                _actionMonoBehaviour = actions.GetActionsOfType<IActionMonoBehaviour>();
            foreach (var t in _actionMonoBehaviour) {
                t.InvokeAction();
            }
        }
    
        public void Invoke(Transform transform, Entity ownerEntity, bool overrideInstantiatePoint = false)
        {
            for (int i = 0; i < spawn.Length; i++)
            {
                if(!spawn[i].instantiatePoint || overrideInstantiatePoint)
                    spawn[i].instantiatePoint = transform;
            }
            Invoke(ownerEntity);
        }
    
        public void Stop(Entity ownerEntity)
        {
            for (int i = 0; i < spawn.Length; i++)
            {
                if (spawn[i].prefab.TryGetComponent(out IStopEntityActionMonoBehaviour StopEntityAction))
                {
                    StopEntityAction.StopAction(ownerEntity);
                }
                else if(spawn[i].prefab.TryGetComponent(out IStopActionMonoBehaviour StopAction)){
                    StopAction.StopAction();
                }
            }
        }
    }
}
