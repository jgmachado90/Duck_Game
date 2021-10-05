using UnityEngine;

namespace GameFramework
{
    public interface IDamager {
    
        GameplayFlags Flags{ get; }

        float Damage { get; }

        Transform Transform { get; }
        void OnDamaged();
    }
}