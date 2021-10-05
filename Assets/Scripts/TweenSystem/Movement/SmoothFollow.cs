using TweenSystem;
using UnityEngine;

namespace TweenSystem
{
    public class SmoothFollow : FollowTargetMonobehaviour
    {
        public Transform target;
        public float smoothTime=1;
        public Vector3 currentVel;

        [Header("Transform Reference")]
        public bool useTransformReference;
        public TransformReference transformReference;

        private void Start()
        {
            if (useTransformReference)
                target = transformReference.GetTransform();
        }

        private void Update()
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref currentVel, smoothTime);
        }

        public override Transform GetTarget() => target;

        public override void SetTarget(Transform t)
        {
            currentVel = Vector3.zero;
            target = t;
        }
    }
}
