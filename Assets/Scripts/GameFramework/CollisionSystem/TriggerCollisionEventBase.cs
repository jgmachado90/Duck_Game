using System.Collections.Generic;
using Addons.EditorButtons.Runtime;
using CoreInterfaces;
using UnityEngine;

namespace GameFramework
{
    public class TriggerCollisionEventBase : CollisionEventBase {
        
        public bool destroy=false;
        public bool destroyGameObject=false;
        public bool triggerSameCollider=true;

        private GameObject _lastCol;

        private bool _triggered;

        protected override bool Collision(GameObject col)
        {
            if((_triggered && destroy) ||
               (!triggerSameCollider && _lastCol == col)	
            ) return false;

            if (!base.Collision(col)) return false;

            _lastCol = col;
            _triggered = true;
            if(destroy){ 
                Destroy(this); 
            }else if(destroyGameObject){
                Destroy(gameObject);
            }
            return true;
        }

        [Button(ButtonMode.EnabledInPlayMode)]
        public void Invoke() {
            base.InvokeEvents(gameObject);
        }
    }
}
