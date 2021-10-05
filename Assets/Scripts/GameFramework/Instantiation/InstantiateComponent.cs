using System;
using Addons.EditorButtons.Runtime;
using CoreInterfaces;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace GameFramework {
    public class InstantiateComponent : Entity, IActionMonoBehaviour {
        public bool onStart;
        public Instantiate instantiate = new Instantiate();

        private void Reset() {
            instantiate.instantiatePoint = transform;
        }

        private void Start()
        {
            if(!instantiate.instantiatePoint)
                instantiate.instantiatePoint = transform;
            if (onStart)
                InvokeAction();
        }

        [Button("Instantiate", ButtonMode.EnabledInPlayMode)]
        public void InvokeAction() {
            instantiate.Invoke(this);
        }
    }

    [Serializable]
    public class Instantiate
    {
        public string description;
        [FormerlySerializedAs("obj")] public GameObject prefab;
        public Transform instantiatePoint;
        public Transform parentTransform;
        public Vector3 offset;

        private ISpawn _instantiateContract;
        private Entity _entity;
        private bool _initialized;

        private void Init()
        {
            _initialized = true;
            if(prefab){
                if(_instantiateContract == null)
                    _instantiateContract = prefab.GetComponent<ISpawn>();
                if (_entity == null)
                    _entity = prefab.GetComponent<Entity>();
            }
        }

        public GameObject Invoke(Entity ownerEntity)
        {
            if(instantiatePoint == null)
                return Invoke(ownerEntity,Vector3.zero, Quaternion.identity);
            return Invoke(ownerEntity,instantiatePoint.position, instantiatePoint.rotation);
        }
        
        public GameObject Invoke(Entity ownerEntity, Transform target, bool parent = false)
        {
            instantiatePoint = target;
            if (parent)
                parentTransform = instantiatePoint;
            return Invoke(ownerEntity);
        }

        public GameObject Invoke(Entity ownerEntity,Vector3 pos, Quaternion rot)
        {
            if (!_initialized) Init();
            GameObject go;
            if (_instantiateContract != null)
            {
                go = _instantiateContract.Spawn(ownerEntity,pos + offset, rot,parentTransform);
            }else if (_entity) {
                Entity entity = Object.Instantiate(_entity, pos + offset, rot,parentTransform);
                entity.OwnerLevel = ownerEntity.OwnerLevel;
                go = entity.gameObject;
            }
            else
            {
                go = Object.Instantiate(prefab, pos + offset, rot,parentTransform);
            }

            return go;
        }

        public GameObject Invoke(Entity ownerEntity, GameObject prefab, Vector3 pos, Quaternion rot)
        {
            this.prefab = prefab;
            return Invoke(ownerEntity, pos, rot);
        }
    }
}