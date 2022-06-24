using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameFramework
{
    public delegate void AddAvaliable(IObjectPoolBehaviour obj);

    public interface IObjectPoolBehaviour
    {
        int PoolId{ get; set;}
    
        MonoBehaviour Self { get;}
    
        event AddAvaliable AddAvailable;
        void ResetObject();
    }

    public class ObjectPool : MonoBehaviour
    {
        [Serializable]
        public class Pool{
            public Transform parentDefault;
            
            public Stack<MonoBehaviour> avaliable=new Stack<MonoBehaviour>();
            public int num;

            public Pool(Transform parent){
                parentDefault = parent;
            }
        }
    
        private Dictionary<int,Pool> pools = new Dictionary<int, Pool>();

        public T PoolInstantiate<T>(T obj, Vector3 pos, Quaternion rot, Transform parent = null, bool forceAllocate = false) where T : MonoBehaviour, IObjectPoolBehaviour{
            return Instantiate(obj, pos, rot, forceAllocate, parent );
        }

        private T Instantiate<T>(T obj, Vector3 pos, Quaternion rot, bool instantiatePool=false, Transform parent = null) where T : MonoBehaviour, IObjectPoolBehaviour
        {
            int id = obj.gameObject.GetInstanceID();
            if (!pools.ContainsKey(id))
            {
                GameObject g = new GameObject("ObjectPool " + obj.name);
                g.transform.SetParent(transform,false);
                pools.Add(id,new Pool(g.transform));
            }

            Pool pool = pools[id];
            
            if (pool.avaliable.Count <= 0 || instantiatePool)
            {
                Transform poolParent = parent? parent : pool.parentDefault;

                T instObjPool = Object.Instantiate(obj, pos, rot, poolParent);
                instObjPool.PoolId = id;
                instObjPool.AddAvailable += AddAvailable;
                pools[id].num++;
                return instObjPool;
            }else{
                Transform poolParent = parent ? parent : pool.parentDefault;
                var inst = pool.avaliable.Pop();
                var instTransform = inst.transform;
                instTransform.parent = poolParent;
                instTransform.position = pos;
                instTransform.rotation = rot;
                var inter = inst as IObjectPoolBehaviour;
                inter.ResetObject();
                return inst as T;
            }
        }

        private void AddAvailable(IObjectPoolBehaviour obj)
        {
            if(obj.Self.transform.parent != pools[obj.PoolId].parentDefault)
                obj.Self.transform.parent = pools[obj.PoolId].parentDefault;
            pools[obj.PoolId].avaliable.Push(obj.Self);
        }
    }
}