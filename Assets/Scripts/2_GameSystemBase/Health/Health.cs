using System;
using UnityEngine;
using UtilsEditor;
using Object = UnityEngine.Object;
using Interfaces;

namespace Health
{
    public interface IContainsHealth
    {
        Health HealthStatus { get;}
    }

    [Serializable]
    public class Health
    {
        [SerializeField,ConditionalField(nameof(entityState),true)] private float currentHealth = 5;
        [SerializeField,ConditionalField(nameof(entityState),true)] private float maxHealth = 5;
        public bool lockTakeDamage;
        public Transform destroyOnDeath;

        [Header("Entity State")]
        public MonoBehaviour entityState;
        [SerializeField,ConditionalField(nameof(entityState)), StringRef(typeof(SavePropertiesGuidRefSource))]
        public string healthKey;
        [SerializeField,ConditionalField(nameof(entityState)), StringRef(typeof(SavePropertiesGuidRefSource))]
        public string maxHealthKey;

        private IAttributeProvider _attributeProvider;

        private IAttributeProvider GetAttributeProvide(){
            if (entityState && _attributeProvider == null){
                _attributeProvider = entityState as IAttributeProvider;
            }
            return _attributeProvider; 
        }

        public float CurrentHealth{
            get{
                var provider = GetAttributeProvide();
                if (provider != null){
                    return provider.GetAttribute(healthKey);
                }
                return currentHealth;
            }
            set
            {
                var provider = GetAttributeProvide();
                if (provider != null){
                    provider.SetAttribute(healthKey,value);
                    OnHealthChanged?.Invoke((CurrentHealth, MaxHealth));
                }
                else{
                    currentHealth = value; 
                    ValueChanged();
                }
            }
        }

        public float MaxHealth{
            get{
                var provider = GetAttributeProvide();
                if (provider != null){
                    return provider.GetAttribute(maxHealthKey);
                }
                return maxHealth;
            }
            set{
                var provider = GetAttributeProvide();
                if (provider != null){
                    provider.SetAttribute(maxHealthKey,value);
                    OnHealthChanged?.Invoke((CurrentHealth, MaxHealth));
                }
                else{
                    maxHealth = value; 
                    ValueChanged();
                }
            }
        }

        public event Action<(float currentHealth, float maxHealth)> OnHealthChanged;
        public event Action<float> OnTakeDamage;

        public event Action OnDeath;

        public void Init(TakeDamage[] takeDamages = null) {
            if (CurrentHealth <= 0){
                CurrentHealth = MaxHealth;
            }
            if (takeDamages != null){
                foreach (var takeDamage in takeDamages){
                    takeDamage.OnTakeDamage +=(TakeDamage);
                }
            }
        }

        private void ValueChanged() {
            if (!float.IsInfinity(CurrentHealth)) {
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            }
            OnHealthChanged?.Invoke((currentHealth, maxHealth));
        }
    
        private void AddMaxHealth(float value) {
            MaxHealth += value;
        }

        public void AddHealth(float value) {
            CurrentHealth += value;
        }

        public void ToggleInvincible() {
            if (float.IsInfinity(CurrentHealth)) {
                CurrentHealth = 10;
            }
            else{
                CurrentHealth = Mathf.Infinity;
            }
        }

        public void SetHealth(float newValue) {
            CurrentHealth = newValue;
        }

        public void TakeDamage(float amount) {
            if(CurrentHealth <= 0 || lockTakeDamage) return;
            if (CurrentHealth > 0){
                CurrentHealth -= amount;
                OnTakeDamage?.Invoke(amount);
            }
            
            if (CurrentHealth <= 0) {
                Kill();
            }
        }

        public void Kill() {
            OnDeath?.Invoke();
            if (destroyOnDeath) {
                Object.Destroy(destroyOnDeath.gameObject);
            }
        }

        public bool IsAlive => CurrentHealth > 0;
    }
}