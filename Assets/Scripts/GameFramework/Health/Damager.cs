﻿using System;
using Addons.EditorButtons.Runtime;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

#endif

namespace GameFramework
{
    public class Damager : Entity, IDamager {
        public bool setDamagerChilds;
        public float damage=1;
        public bool destroyOnDamage;
        public GameplayFlags flags;
        public Feedback feedbackOnDamaged;  

        private bool damaged;

        public GameplayFlags Flags => flags;
        public float Damage => damage;
        public Transform Transform => transform;

        public event Action OnDamagedEvent;

        private void Start() {
            if(setDamagerChilds)
                SetDamagerChild();
        }

        public void OnDamaged() {
            if (destroyOnDamage && damaged) return;
            if (destroyOnDamage) damaged = true;
            
            OnDamagedEvent?.Invoke();
            feedbackOnDamaged.Invoke(transform,this);
            
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


