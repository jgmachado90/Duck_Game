using TweenSystem;
using UnityEngine;

namespace TweenSystem
{
    public class SphericalFollow : FollowTargetMonobehaviour
    {
        public Transform target;
        public float speed=1;
        public float rotationSpeed=1;
    
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
            transform.position = Vector3.SlerpUnclamped(transform.position, target.position,speed * Time.deltaTime);
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, target.rotation,rotationSpeed * Time.deltaTime);
        }

        public override Transform GetTarget() => target;

        public override void SetTarget(Transform t)
        {
            target = t;
        }
    }
}
