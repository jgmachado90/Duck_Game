using UnityEngine;

namespace GameFramework
{
    public interface ISpawn
    {
        GameObject Spawn(Entity owner, Vector3 pos,Quaternion rot, Transform parent=null);
    }
}