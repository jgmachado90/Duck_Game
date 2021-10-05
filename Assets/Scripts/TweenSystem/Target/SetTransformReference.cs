using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TweenSystem
{
    public enum TransformReferenceType
    {
        Target,
        Custom,
        PlayerActor,
        Head,
        XrRig,
        PlayerActorDamageTarget,
    }

    [Serializable]
    [DefaultExecutionOrder(0)]
    public class TransformReference {
        public Transform target;
        public TransformReferenceType transformReference = TransformReferenceType.Target;
        public string custom;

        public static Dictionary<string,Transform> Transforms = new Dictionary<string, Transform>();
    
        public void AddTransform(Transform transform)
        {
            if (transformReference == TransformReferenceType.Custom)
            {
                AddTransform(custom,transform);
            }
            else
            {
                AddTransform(transformReference.ToString(),transform);
            }
        }
    
        public void RemoveTransform()
        {
            if (transformReference == TransformReferenceType.Custom)
            {
                RemoveTransform(custom);
            }
            else
            {
                RemoveTransform(transformReference.ToString());
            }
        }
    
        public Transform GetTransform()
        {
            if (transformReference == TransformReferenceType.Target) {
                return target;
            }
            else if (transformReference == TransformReferenceType.Custom) {
                return GetTransform(custom);
            }
            else {
                return GetTransform(transformReference.ToString());
            }
        }

        public static Transform GetTransform(TransformReferenceType type)
        {
            return GetTransform(type.ToString());
        }

        public static Transform GetTransform(string key)
        {
            if (Transforms.ContainsKey(key))
            {
                if (Transforms[key] == null)
                {
                    Transforms.Remove(key);
                    return null;
                }
                return Transforms[key];
            }
        
            return null;
        }

        public static void AddTransform(string key,Transform transform)
        {
            if (Transforms.ContainsKey(key))
                Transforms[key] = transform;
            else
                Transforms.Add(key,transform);
        }
    
        public static void RemoveTransform(string key)
        {
            if (Transforms.ContainsKey(key))
                Transforms.Remove(key);
        }
    }

    [DefaultExecutionOrder(0)]
    public class SetTransformReference : MonoBehaviour
    {
        public TransformReference transformReference;
        
        private void OnEnable()
        {
            Set(transform);
        }

        public void Set(Transform t)
        {
            transformReference.AddTransform(t);
        }

        private void OnDisable()
        {
            transformReference.RemoveTransform();
        }
    }
}