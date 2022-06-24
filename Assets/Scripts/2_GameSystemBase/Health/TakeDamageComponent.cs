using System;
using System.Collections.Generic;
using CollisionSystem;
using Common;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using UtilsEditor;

namespace Health{
    [Serializable]
    public class TakeDamage{
        public Cooldown invulnerabilityTime = new Cooldown();
        [FormerlySerializedAs("layerMask")] public LayerMask damageLayerMask = ~0;
        public GameplayFlags takeDamageFlags;
        public DamageMultiplierFlags damageMultiplierFlags;
        public MonoBehaviour[] takeDamageFeedback;
        public CollisionEventBase[] collisionEventComponent;
        public event Action<float> OnTakeDamage;
        public event Action<IDamager> OnSendDamager;
        public event Action<float,IDamager> OnTakeAndSendDamage;
        [HideInInspector] public float damageMultiplier = 1;

        private LayerMask? _firstMask;
        private GameObject _owner;
        private List<IDamageCondition> _damageConditions = new List<IDamageCondition>();

        public void AddDamageCondition(IDamageCondition damageCondition){
            _damageConditions.Add(damageCondition);
        }
        
        public void RemoveDamageCondition(IDamageCondition damageCondition){
            _damageConditions.Remove(damageCondition);
        }

        public virtual void Init(GameObject owner){
            _owner = owner;
            foreach (var item in takeDamageFeedback.GetActionsOfType<IActionMonoBehaviour>())
            {
                OnTakeDamage += (o => item.InvokeAction());
            }
            SetCollisionEventComponents(collisionEventComponent);
        }

        public void SetCollisionEventComponents(CollisionEventBase[] components){
            for (var i = 0; i < components.Length; i++){
                var collisionEvent = components[i];
                collisionEvent.OnCollisionEnter += Damage;
            }
        }

        public void Damage(GameObject col){
            if (!col || !invulnerabilityTime.Finished()){
                return;
            }
            var damager = col.GetComponent<IDamager>();
            if (damager == null || !damageLayerMask.ContainsLayer(col) || !takeDamageFlags.ContainsOneFlag(damager.Flags)) return;
            float damage = damager.Damage * damageMultiplier * damageMultiplierFlags.GetDamageMultiplier(damager.Flags.flags);
            for (var i = 0; i < _damageConditions.Count; i++){
                var damageCondition = _damageConditions[i];
                if(!damageCondition.CanTakeDamage(damage,damager)) return;
            }
            invulnerabilityTime.Init();
            OnSendDamager?.Invoke(damager);
            OnTakeAndSendDamage?.Invoke(damage,damager);
            OnTakeDamage?.Invoke(damage);
            damager.OnDamaged(_owner);
        }

        public void SetImmuneToLayer(bool immune, string layerName){
            if(_firstMask != null) return;
            int layer = 1 << LayerMask.NameToLayer(layerName);
            if (immune){
                damageLayerMask &= ~layer;
            }else{
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

    public class TakeDamageComponent : MonoBehaviour{
        public CollisionPreset preset;
        public TakeDamage takeDamage;
        
        public event Action<float> OnTakeDamage;
        public event Action<IDamager> OnSendDamager;
        
        public event Action<float,IDamager> OnTakeAndSendDamage;

        public void Start(){
            SetPreset(preset);
            takeDamage.Init(gameObject);
        }

        private void SetPreset(CollisionPreset newPreset){
            if(!preset) return;
            preset = newPreset;
            var result = gameObject.GenerateTriggers(new GenerateTriggersParams()
            {
                triggerEnter = true,
                collisionPreset = preset,
            });
            takeDamage.collisionEventComponent = result.triggersEnter.ToArray();
            takeDamage.takeDamageFlags = preset.collisionSettings.gameplayFlags;
            takeDamage.damageMultiplierFlags = preset.collisionSettings.damageMultiplierFlags;
        }

        private void OnEnable(){
            takeDamage.OnTakeAndSendDamage += TakeAndSendDamage;
            takeDamage.OnTakeDamage += TakeDamageEvent;
            takeDamage.OnSendDamager += SendDamagerEvent;
        }

        private void TakeAndSendDamage(float damage, IDamager damager){
            OnTakeAndSendDamage?.Invoke(damage,damager);
        }

        private void SendDamagerEvent(IDamager obj){
            OnSendDamager?.Invoke(obj);
        }

        private void TakeDamageEvent(float obj){
            OnTakeDamage?.Invoke(obj);
        }

        private void OnDisable(){
            takeDamage.OnTakeAndSendDamage -= OnTakeAndSendDamage;
            takeDamage.OnTakeDamage -= TakeDamageEvent;
            takeDamage.OnSendDamager -= SendDamagerEvent;
        }

        public GameplayFlags GetFlags(){
            return takeDamage.takeDamageFlags;
        }

        public void AddDamageCondition(IDamageCondition damageCondition){
            takeDamage.AddDamageCondition(damageCondition);
        }
        
        public void RemoveDamageCondition(IDamageCondition damageCondition){
            takeDamage.RemoveDamageCondition(damageCondition);
        }
        
#if UNITY_EDITOR        
        [Button(ButtonMode.DisabledInPlayMode)]
        private void GetTakeDamage() {
            var collisions = GetComponentsInChildren<CollisionEventBase>(true);
            takeDamage.collisionEventComponent = collisions;
            UnityEditor.EditorUtility.SetDirty(this);

        }
#endif
    }
}