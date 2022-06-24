using UnityEngine;

namespace GameFramework{
    public class SpawnableInfo{
        public ISpawnable Spawnable{ get; }
        public Entity SpawnableEntity{ get; }
        public GameObject Prefab{ get; }
        public Entity Owner{ get; }
        public IEnvInfoContainer EnvironmentInfoContainerContainer{ get; }
        
        public bool CanReceiveEnvInfo { get; }

        public SpawnableInfo(Entity owner, GameObject prefab){
            Prefab = prefab;
            Owner = owner;
            Spawnable = prefab.GetComponent<ISpawnable>();
            SpawnableEntity = prefab.GetComponent<Entity>();
            if (SpawnableEntity){
                EnvironmentInfoContainerContainer = owner as IEnvInfoContainer;
                CanReceiveEnvInfo = EnvironmentInfoContainerContainer != null && prefab.GetComponent<IEnvInfoReceiver>() != null;
            }
        }

        public GameObject Spawn(Vector3 pos, Quaternion rot, Transform parent=null){
            return Owner.GetOwnerLevel().Spawn(this, pos,rot, parent);
        }
    }
}