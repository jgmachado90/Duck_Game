using CollisionSystem;
using UnityEngine;

namespace Health
{
    public interface IDamager {
    
        GameplayFlags Flags{ get; set; }

        float Damage { get; }

        Transform Transform { get; }
        void OnDamaged(GameObject damagedObj);
    }

    public interface IDamageCondition{
        public bool CanTakeDamage(float damage, IDamager damager);
    }
}