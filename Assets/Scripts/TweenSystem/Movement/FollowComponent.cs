using System;
using System.Collections;
using System.Collections.Generic;
using TweenSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace TweenSystem
{
    public enum UpdateType
    {
        Update,
        FixedUpdate, 
        LateUpdate,
    }

    public enum MovementType
    {
        Transform,
        MoveRigdbody,
        AddForce,
        Velocity,
    }

    [Serializable]
    public class Follow
    {
        public bool unscaleDeltaTime;
        [FormerlySerializedAs("MovementType")] public MovementType movementType;

        [Header("Position")]
        public float speed = 20;
        public Vector3 axisMask = Vector3.one;
        public Vector3 offset;
        public Vector3 directionOffset;
    
        [Header("Rotation")]
        public float rotationSpeed = 0;
        public float rotToward = 0;
        public Vector3 offsetRot;
        public Vector3 rotTowardOffset;
    
        [Header("Rigdbody")]
        public float force;

        public Transform Target {
            get;
            set; 
        }
    
        private Quaternion _offsetQuat;
        private Vector3 _lastPos;
        private Vector3 _actualDir;
        private Transform _transform;
        private Rigidbody _rigidb;
    
        private static float GetScaledDeltaTime() => Time.deltaTime;
        private static float GetSUnscaledDeltaTime() => Time.unscaledTime;
        
        public Vector3 TargetPos => _transform.position + Vector3.Scale(Target.position - _transform.position, axisMask);

        private Dictionary<bool, Func<float>> GetDeltaTime = new Dictionary<bool, Func<float>>()
        {
            {false, GetScaledDeltaTime},
            {true, GetSUnscaledDeltaTime},
        };

        private Dictionary<MovementType, Action> move;

        public bool Init(Transform transform,Transform target)
        {
            _transform = transform;
            Target = target;
        
            _offsetQuat = Quaternion.Euler(offsetRot);
            _lastPos = transform.position;

            if (movementType != MovementType.Transform)
            {
                _rigidb = transform.GetComponent<Rigidbody>();
            }

            move = new Dictionary<MovementType, Action>()
            {
                {MovementType.Transform, MoveTo},
                {MovementType.MoveRigdbody, MoveRigidbodyPosition},
                {MovementType.AddForce, MoveRigdbodyAddForce},
                {MovementType.Velocity, MoveRigdbodyVelocity},
            };
        
            return _transform && Target; 
        }

        public void Update()
        {
            move[movementType].Invoke();
        }
    
        private Vector3 GetPos() 
        {
            Vector3 dirOffset = _transform.TransformDirection(directionOffset);
            return Vector3.MoveTowards(_transform.position, TargetPos + offset + dirOffset, speed * GetDeltaTime[unscaleDeltaTime].Invoke());
        }

        private Quaternion GetRot() 
        {
            float deltatime = GetDeltaTime[unscaleDeltaTime].Invoke();
            if(rotToward > 0)
            {
                Vector3 dir = _transform.position - _lastPos + rotTowardOffset * deltatime;
                float len = dir.magnitude;
                _lastPos = Vector3.Lerp(_lastPos, _transform.position, rotToward * len * deltatime);
                _actualDir = Vector3.Lerp(_actualDir, dir, rotToward * deltatime);
                return Quaternion.LookRotation(_actualDir/len);
            }
            return Quaternion.RotateTowards(_transform.rotation, Target.rotation * _offsetQuat, rotationSpeed * deltatime);
        }
    
        private void MoveTo()
        {
            _transform.position = GetPos();
            _transform.rotation = GetRot();
        }
    
        private void MoveRigidbodyPosition()
        {
            _rigidb.MovePosition(GetPos());
            _rigidb.MoveRotation(GetRot());
        }

        private void MoveRigdbodyVelocity()
        {
            Vector3 dir2 = (GetPos() - _rigidb.position).normalized;
            _rigidb.AddForce(dir2 * force, ForceMode.VelocityChange);
        }

        private void MoveRigdbodyAddForce()
        {
            Vector3 dir = (GetPos() - _rigidb.position).normalized;
            _rigidb.AddForce(dir * force);
        }
    }

    public class FollowComponent : MonoBehaviour
    {
        [Header("Target Settings")]
        public TransformReference transformReference;

        [Header("Update Settings")]
        public UpdateType updateType;
        [Header("Follow Settings")]
        public Follow followSettings;

        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            bool init = followSettings.Init(transform, transformReference.GetTransform());
            if (init)
            {
                switch (updateType)
                {
                    case UpdateType.Update:
                        StartCoroutine(UpdateCoroutine());
                        break;
                    case UpdateType.FixedUpdate:
                        StartCoroutine(FixedUpdateCoroutine());
                        break;
                    case UpdateType.LateUpdate:
                        StartCoroutine(LateUpdateCoroutine());
                        break;
                    default:
                        StartCoroutine(UpdateCoroutine());
                        break;
                }
            }
        }

        IEnumerator UpdateCoroutine() 
        {
            while(enabled){
                yield return null;
                followSettings.Update();
            }
        }

        IEnumerator FixedUpdateCoroutine() 
        {
            while(enabled){
                yield return new WaitForFixedUpdate();
                followSettings.Update();
            }
        }

        IEnumerator LateUpdateCoroutine() 
        {
            while(enabled){
                yield return new WaitForEndOfFrame();
                followSettings.Update();
            }
        }

        public void SetRotTowardOffset(Vector3 rot)
        {
            followSettings.rotTowardOffset = rot;
        }

        public void SetRotToward(float  rot)
        {
            followSettings.rotToward = rot;
        }

        public void SetRotationSpeed(float  speed)
        {
            followSettings.rotationSpeed = speed;
        }

        public void SetTarget(GameObject go)
        {
            followSettings.Target = go.transform;
        }

        public void SetTarget(Transform t)
        {
            followSettings.Target = t;
        }
    }
}