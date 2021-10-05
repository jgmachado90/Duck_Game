using System.Collections.Generic;
using TweenSystem;
using UnityEngine;

namespace TweenSystem
{
    [ExecuteAlways]
    public class SnakeMovement : MonoBehaviour
    {
        public bool rotation;
        public List<Transform> segments = new List<Transform>();
        public float dist=1;
        public Transform parentFollow;
        public Transform parent;
        public bool parentFollowXRRig;

        private void Start()
        {
            if (parentFollowXRRig && Application.isPlaying)
                parentFollow = TransformReference.GetTransform(TransformReferenceType.XrRig);
            if(parent && Application.isPlaying)
                parent.parent = null;
        }

        private void OnEnable() {
            if(parent)
                parent.gameObject.SetActive(true);
        }

        private void OnDisable() {
            if(parent)
                parent.gameObject.SetActive(false);
        }

        void Update()
        {
            if (parent && parentFollow)
                parent.transform.position = parentFollow.position;
        
            for (int i = 0; i < segments.Count; i++)
            {
                var segment = segments[i];
                Vector3 positionS = segments[i].transform.position;
                Vector3 targetS = i == 0 ? transform.position : segments[i - 1].transform.position;
                if(rotation)
                    segments[i].transform.rotation = Quaternion.LookRotation(Vector3.forward, (targetS - positionS).normalized);
                Vector3 diff = positionS - targetS; 
                diff.Normalize();
                segment.transform.position = targetS + dist * diff;
            }
        }

        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < segments.Count; i++)
            {
                Gizmos.DrawWireSphere(segments[i].position,dist/2f);
            }
        }
    }
}
