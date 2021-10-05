using UnityEngine;

namespace GameFramework
{
    public class DamagerChild : MonoBehaviour , IDamager
    {
        public MonoBehaviour damager;

        private IDamager _parentDamager;

        private void OnValidate()
        {
            if (damager && !damager.TryGetComponent(out IDamager damagerContract))
            {
                damager = null;
            }
        }

        private void OnEnable() {
            if (_parentDamager == null)
                SetParentDamager(damager);
        }

        public void SetParentDamager(MonoBehaviour monoBehaviour) {
            damager = monoBehaviour;
            _parentDamager = monoBehaviour.GetComponent<IDamager>();
        }

        public GameplayFlags Flags => _parentDamager.Flags;
        public float Damage => _parentDamager.Damage;
    
        public Transform Transform => _parentDamager.Transform;
    
        public void OnDamaged()
        {
            _parentDamager.OnDamaged();
        }
    }
}
