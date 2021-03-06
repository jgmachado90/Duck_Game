using System;
using CollisionSystem;
using GameFramework;
using UnityEditor;
using UnityEngine;
using UtilsEditor;
using MoreMountains.Feedbacks;
#if UNITY_EDITOR

#endif

namespace Health
{
    public class Damager : Entity, IDamager {
        public bool setDamagerChilds;
        public float damage=1;
        public bool destroyOnDamage;
        public GameplayFlags flags;
        public MMFeedbacks feedbackOnDamaged;  

        private bool damaged;

        public GameplayFlags Flags { get { return flags; } set { flags = value; } }
        public float Damage => damage;
        public Transform Transform => transform;

        public event Action OnDamagedEvent;

        private void Start() {
            if(setDamagerChilds)
                SetDamagerChild();
        }

        public void OnDamaged(GameObject damagedObj) {
            if (destroyOnDamage && damaged) return;
            if (destroyOnDamage) damaged = true;
            
            OnDamagedEvent?.Invoke();
            if(feedbackOnDamaged != null)
                feedbackOnDamaged.PlayFeedbacks();
            
            if (destroyOnDamage){
                Destroy(gameObject);
            }
        }
        

        [Button(ButtonMode.DisabledInPlayMode)]
        private void SetDamagerChild() {
            var damagers = GetComponentsInChildren<DamagerChild>(true);
            foreach (var damager in damagers) {
                damager.SetParentDamager(this);
#if UNITY_EDITOR
                EditorUtility.SetDirty(damager);
#endif
            }
        }
    }
}


