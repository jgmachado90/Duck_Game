using System;
using System.Linq;
using Addons.EditorButtons.Runtime;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

#endif

namespace GameFramework
{
    public class HealthComponent : Entity, IContainsHealth {
        [HideInInspector] public bool dead;
        public Health health;
        public TakeDamage damageSettings;
        public bool maxHealthDeath = false;
        public bool getChildTakeDamage;

        [Header("Feedbacks")]
        public Feedback feedbackDamage;
        public Feedback feedbackDeath;
        public Feedback feedbackRespawn;
        
        public event Action OnRespawn;

        public Health HealthStatus => health;

        private float _startHealth=0;
        
        public event Action OnDeath;

        public event Action OnNormalDeath;
        public event Action OnMaxHealthDeath;

        private void Awake() {
            _startHealth = health.CurrentHealth;
        }

        private void Start()
        {
            if(getChildTakeDamage)
                GetTakeDamage();
            health.OnDeath += PostDeath;
            health.OnHealthChanged += HealthChanged;
            health.Init(takeDamages: new []{ damageSettings});
            damageSettings.OnTakeDamage += OnTakeDamage;
            damageSettings.Init();
        }

        private void HealthChanged((float currentHealth, float maxHealth) value) {
            var (currentHealth, maxHealth) = value;
            if (maxHealthDeath && !dead && currentHealth >= maxHealth) {
                health.Kill();
            }
        }

        private void OnTakeDamage(float obj)
        {
            feedbackDamage.Invoke(transform,this);
        }

        [Button(ButtonMode.EnabledInPlayMode)]
        public void ForceDeath()
        {
            health.TakeDamage(Mathf.Infinity);
        }

        private void PostDeath()
        {
            if (!maxHealthDeath || health.CurrentHealth <= 0) {
                health.CurrentHealth = 0;
                OnNormalDeath?.Invoke();
            }
            else {
                OnMaxHealthDeath?.Invoke();
            }
            
            dead = true;
            damageSettings.SetImmune(true);
            feedbackDeath.Invoke(transform,this);
            OnDeath?.Invoke();
        }

        public void Respawn()
        {
            health.CurrentHealth = _startHealth;
            dead = false;
            damageSettings.SetImmune(false);
            feedbackRespawn.Invoke(transform,this);
            OnRespawn?.Invoke();
        }

        [Button(ButtonMode.DisabledInPlayMode)]
        private void GetTakeDamage() {
            var collisions = GetComponentsInChildren<CollisionEventBase>(true);
            damageSettings.collisionEventComponent = collisions.Where(collision => collision.gameObject.layer == LayerMask.NameToLayer("TakeDamage")).ToArray();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
