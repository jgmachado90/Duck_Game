using UnityEngine;

namespace GameFramework
{
    public interface ISpawnable
    {
        GameObject Spawn(Entity owner, Vector3 pos,Quaternion rot, Transform parent=null);
    }
}