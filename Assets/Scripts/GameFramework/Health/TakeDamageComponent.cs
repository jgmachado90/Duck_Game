using System;
using CoreInterfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameFramework
{
    [Serializable]
    public class TakeDamage
    {
        public Cooldown invulnerabilityTime = new Cooldown();
        [FormerlySerializedAs("layerMask")] public LayerMask damageLayerMask = ~0;
        public GameplayFlags takeDamageFlags;
        public DamageMultiplierFlags damageMultiplierFlags;
        public MonoBehaviour[] takeDamageFeedback;
        public CollisionEventBase[] collisionEventComponent;
        public event Action<float> OnTakeDamage;
        public event Action<IDamager> OnSendDamager;
        [HideInInspector] public float damageMultiplier = 1;

        private LayerMask? _firstMask;

        public virtual void Init()
        {
            foreach (var item in takeDamageFeedback.GetActionsOfType<IActionMonoBehaviour>())
            {
                OnTakeDamage += (o => item.InvokeAction());
            }

            SetCollisionEventComponents(collisionEventComponent);
        }

        public void SetCollisionEventComponents(CollisionEventBase[] components)
        {
            foreach (var collisionEvent in components)
            {
                collisionEvent.OnCollisionEnter +=Damage;
            }
        }

        public void Damage(GameObject col)
        {
            if (!col || !invulnerabilityTime.Finished()){
                return;
            } 

            IDamager damager = col.GetComponent<IDamager>();
            if (damager != null && damageLayerMask.ContainsLayer(col) && takeDamageFlags.ContainsOneFlag(damager.Flags))
            {
                invulnerabilityTime.Init();
                OnSendDamager?.Invoke(damager);
                OnTakeDamage?.Invoke(damager.Damage * damageMultiplier * damageMultiplierFlags.GetDamageMultiplier(damager.Flags.flags));
                damager.OnDamaged();
            }
        }

        public void SetImmuneToLayer(bool immune, string layerName)
        {
            if(_firstMask != null) return;
            int layer = 1 << LayerMask.NameToLayer(layerName);
            if (immune)
            {
                damageLayerMask &= ~layer;
            }
            else
            {
                damageLayerMask |= layer;
            }
        }

        public void SetImmune(bool result){
            if(result){
                if(_firstMask == null)
                    _firstMask = damageLayerMask;
                damageLayerMask = 0;   
            }else if(_firstMask != null){
                damageLayerMask = (LayerMask)_firstMask;
                _firstMask = null;
            }       
        }
    }

    public class TakeDamageComponent : MonoBehaviour
    {
        public VolumeMode mode = VolumeMode.Object3D; 
        public CollisionPreset preset;
        public TakeDamage takeDamage;
        
        public event Action<float> OnTakeDamage;
        public event Action<IDamager> OnSendDamager;

        public void Start()
        {
            SetPreset(preset);
            takeDamage.Init();
        }

        private void SetPreset(CollisionPreset newPreset)
        {
            if(!preset) return;
            preset = newPreset;
            gameObject.GenerateTriggers(new GenerateTriggersParams()
            {
                triggerEnter = true,
                mode = mode,
                collisionPreset = preset,
            }, out var gameObjects, out var triggers);
            
            takeDamage.collisionEventComponent = triggers.ToArray();
            takeDamage.takeDamageFlags = preset.collisionSettings.gameplayFlags;
        }

        private void OnEnable()
        {
            takeDamage.OnTakeDamage += TakeDamageEvent;
            takeDamage.OnSendDamager += SendDamagerEvent;
        }

        private void SendDamagerEvent(IDamager obj)
        {
            OnSendDamager?.Invoke(obj);
        }

        private void TakeDamageEvent(float obj)
        {
            OnTakeDamage?.Invoke(obj);
        }

        private void OnDisable()
        {
            takeDamage.OnTakeDamage -= TakeDamageEvent;
            takeDamage.OnSendDamager -= SendDamagerEvent;
        }
    }
}