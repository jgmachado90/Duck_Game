using System;
using TweenSystem;
using UnityEngine;

namespace TweenSystem
{
    [Serializable]
    public class LookAt {
        public Transform transform;
        public Transform target;
        public Vector3 mask = Vector3.one;
        public float angularSpeed = -1;
        public Vector3 offset;
        public Vector3 directionOffset;

        public void Update() {
            if (angularSpeed >= 0) LookWithSpeed(angularSpeed);
            else LookAtTarget();
        }
        
        private void LookAtTarget()
        {
            transform.LookAt(GetDir());
        }

        private Vector3 GetDir() {
            Vector3 dirOffset = target.TransformDirection(directionOffset);
            return transform.position + (Vector3.Scale(target.position - transform.position, mask) + offset + dirOffset);
        }

        private void LookWithSpeed(float speed)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(GetDir()), speed * Time.deltaTime);
        }
    }

    public class LookAtComponent : MonoBehaviour
    {
        public TransformReference targetTransform;
        public LookAt lookAt = new LookAt();

        private void Reset() {
            lookAt.transform = transform;
        }

        private void Start() {
            if(!lookAt.transform)
                lookAt.transform = transform;
            if(!lookAt.target)
                lookAt.target = targetTransform.GetTransform();
            lookAt.Update();
        }

        private void Update()
        {
            lookAt.Update();
        }

        public void SetTarget(Transform target)
        {
            lookAt.target = target;
        }
    }
}
