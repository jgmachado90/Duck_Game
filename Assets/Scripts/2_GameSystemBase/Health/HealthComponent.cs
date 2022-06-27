using System;
using System.Linq;
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
    public class HealthComponent : Entity, IContainsHealth {
        public Health health;
        public TakeDamage damageSettings;
        public TakeDamageComponent[] takeDamageComponents;
        public bool getChildTakeDamage;

        [Header("Feedbacks")]
        public MMFeedbacks feedbackDamage;
        public MMFeedbacks feedbackDeath;

        public Health HealthStatus => health;
        public bool IsAlive => health.IsAlive;

        public event Action OnDeath;

        private bool _init;

        private void Start(){
            Init();
        }

        private void Init(){
            if(_init) return;
            _init = true;
            if (getChildTakeDamage)
                GetTakeDamage();
            health.OnDeath += PostDeath;
            var takeDamages = new TakeDamage [1 + takeDamageComponents.Length];
            takeDamages[0] = damageSettings;
            for (int i = 1; i < takeDamages.Length; i++){
                takeDamages[i] = takeDamageComponents[i - 1].takeDamage;
            }
            health.Init(takeDamages: takeDamages);
            health.OnTakeDamage += OnTakeDamage;
            damageSettings.Init(gameObject);
        }

        private void OnTakeDamage(float obj){
            if (feedbackDamage == null) return;
            feedbackDamage.PlayFeedbacks();
        }

        [Button(ButtonMode.EnabledInPlayMode)]
        public void ForceDeath(){
            health.TakeDamage(Mathf.Infinity);
        }
        
        [Button(ButtonMode.EnabledInPlayMode)]
        public void ForceDamage(){
            health.TakeDamage(1);
        }
        
        [Button(ButtonMode.EnabledInPlayMode)]
        public void AddHealth(int value){
            health.AddHealth(value);
        }

        private void PostDeath(){
            if(feedbackDeath != null)
                feedbackDeath.PlayFeedbacks();
            OnDeath?.Invoke();
        }

        [Button(ButtonMode.DisabledInPlayMode)]
        private void GetTakeDamage() {
            var collisions = GetComponentsInChildren<CollisionEventBase>(true);
            damageSettings.collisionEventComponent = collisions;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
