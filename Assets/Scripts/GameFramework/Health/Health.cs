using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameFramework
{
    public interface IContainsHealth
    {
        Health HealthStatus { get;}
    }

    [Serializable]
    public class Health
    {
        [SerializeField] private float currentHealth = 5;
        [SerializeField] private float maxHealth = 5;
        public bool lockTakeDamage;
        public Transform destroyOnDeath;

        public float CurrentHealth
        {
            get => currentHealth;
            set
            {
                currentHealth = value; 
                ValueChanged();
            }
        }
    
        public float MaxHealth
        {
            get => maxHealth;
            set
            {
                maxHealth = value;
                ValueChanged();
            }
        }

        public event Action<(float currentHealth, float maxHealth)> OnHealthChanged;
        public event Action<float> OnTakeDamage;

        public event Action OnDeath;

        public void Init(float? currentHealth = null, Transform destroyOnDeath= null, TakeDamage[] takeDamages = null)
        {
            if (currentHealth != null)
                this.currentHealth = currentHealth.Value;
            if(destroyOnDeath)
                this.destroyOnDeath = destroyOnDeath;

            if (takeDamages != null)
            {
                foreach (var takeDamage in takeDamages)
                {
                    takeDamage.OnTakeDamage +=(TakeDamage);
                }
            }

            ValueChanged();
        }

        private void ValueChanged()
        {
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            OnHealthChanged?.Invoke((currentHealth, maxHealth));
        }
    
        private void AddMaxHealth(float value)
        {
            maxHealth += value;
            ValueChanged();
        }

        public void AddHealth(float value)
        {
            currentHealth += value;
            ValueChanged();
        }

        public  void ToggleInvincible()
        {
            if (float.IsInfinity(currentHealth))
            {
                currentHealth = 10;
            }
            else
            {
                currentHealth = Mathf.Infinity;
            }
        }

        public void SetHealth(float newValue)
        {
            currentHealth = newValue;
            ValueChanged();
        }

        public void TakeDamage(float amount)
        {
            if(currentHealth <= 0 || lockTakeDamage) return;
            if (currentHealth > 0)
            {
                currentHealth -= amount;
                OnTakeDamage?.Invoke(amount);
            }

            ValueChanged();
            if (currentHealth <= 0) {
                Kill();
            }
        }

        public void Kill() {
            OnDeath?.Invoke();
            if (destroyOnDeath) {
                Object.Destroy(destroyOnDeath.gameObject);
            }
        }
    }
}