using System;
using Common;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace GameFramework {
    public class InstantiateComponent : Entity, IActionMonoBehaviour {
        public bool onStart;
        [FormerlySerializedAs("instantiate")] public Instantiator instantiator = new Instantiator();

        private void Reset() {
            instantiator.instantiatePoint = transform;
        }

        private void Start()
        {
            if(!instantiator.instantiatePoint)
                instantiator.instantiatePoint = transform;
            if (onStart)
                InvokeAction();
        }

        public void InvokeAction() {
            instantiator.Invoke(this);
        }
    }

    [Serializable]
    public class Instantiator
    {
        public string description;
        public GameObject prefab;
        public Transform instantiatePoint;
        public Transform parentTransform;
        public Vector3 offset;
        
        private bool _initialized;
        private SpawnableInfo _spawnableInfo;

        public Instantiator(GameObject prefab){
            this.prefab = prefab;
        }
        
        public Instantiator(){
        }

        private void Init(Entity ownerEntity)
        {
            if (_initialized) return;
            _initialized = true;
            if(prefab){
                _spawnableInfo = new SpawnableInfo(ownerEntity,prefab);
            }
        }

        public GameObject Invoke(Entity ownerEntity){
            if(instantiatePoint == null)
                return Invoke(ownerEntity,Vector3.zero, Quaternion.identity);
            return Invoke(ownerEntity,instantiatePoint.position, instantiatePoint.rotation);
        }
        
        public GameObject Invoke(Entity ownerEntity, Transform target, bool parent = false){
            instantiatePoint = target;
            if (parent)
                parentTransform = instantiatePoint;
            return Invoke(ownerEntity);
        }

        public GameObject Invoke(Entity ownerEntity,Vector3 pos, Quaternion rot){
            
            if (!_initialized) Init(ownerEntity);
            return _spawnableInfo.Spawn(pos + offset, rot, parentTransform);
        }

        public GameObject Invoke(Entity ownerEntity, GameObject prefab, Vector3 pos, Quaternion rot)
        {
            this.prefab = prefab;
            return Invoke(ownerEntity, pos, rot);
        }
    }
}