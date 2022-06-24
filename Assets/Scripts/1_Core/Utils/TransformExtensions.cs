using UnityEngine;
using UnityEngine.AI;

namespace Utils{
    public static class TransformExtensions{
        
        public static void TeleportTo(this Transform transform, Transform target, bool applyRotation=false){
            var navMeshAgent = transform.GetComponentInChildren<NavMeshAgent>();
            var position = target.position;
            if (navMeshAgent){
                
                var offset = transform.InverseTransformPoint(navMeshAgent.transform.position);
                transform.position = position;
                navMeshAgent.Warp(position + offset);
            }
            else{
                transform.position = position;
            }
            if (applyRotation){
                transform.rotation = target.rotation;
            }
        }
    }
}