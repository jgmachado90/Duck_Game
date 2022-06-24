using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UtilsEditor;

namespace CollisionSystem {

    [AddComponentMenu("ARVORE/Collision/TriggerArea")]
    public class TriggerArea: Entity{
        [Header("Generate Trigger Components")]
        public bool generateTriggerComponents = true;
        public bool generateOnlyTriggerColliders = true;
        public CollisionPreset collisionPreset;
        
        [Header("Triggers")]
        public List<CollisionEventBase> triggersEnter;
        public List<CollisionEventBase> triggersExit;
        
        [Header("Feedback")]
        public Feedback feedbackEnter;
        public Feedback feedbackExit;
        
        public event Action<GameObject> OnEnterArea;
        public event Action<GameObject> OnExitArea;

        public GameObject LastTriggeredObject { get; private set; }
        
        private void Start() {
            if (generateTriggerComponents){
                GenerateTriggers();
            }
        }

        public virtual void OnEnable(){
            AddTriggerCallback();
        }

        public virtual void OnDisable(){
            RemoveTriggerCallback();
        }

        private void AddTriggerCallback(){
            for (var i = 0; i < triggersEnter.Count; i++){
                var item = triggersEnter[i];
                item.OnCollisionEnter += Enter;
            }

            for (var i = 0; i < triggersExit.Count; i++){
                var item = triggersExit[i];
                item.OnCollisionEnter += Exit;
            }
        }

        private void RemoveTriggerCallback(){
            for (var i = 0; i < triggersEnter.Count; i++){
                var item = triggersEnter[i];
                item.OnCollisionEnter -= Enter;
            }

            for (var i = 0; i < triggersExit.Count; i++){
                var item = triggersExit[i];
                item.OnCollisionEnter -= Exit;
            }
        }

        private void GenerateTriggers() {
            var result = gameObject.GenerateTriggers(new GenerateTriggersParams() {
                triggerEnter = true,
                triggerExit =  true,
                generateOnlyTriggerColliders = generateOnlyTriggerColliders,
                collisionPreset = collisionPreset,
            });
            triggersEnter = result.triggersEnter;
            triggersExit = result.triggersExit;
            AddTriggerCallback();
        }

        protected virtual void Enter(GameObject obj) {
            if(!enabled) return;
            LastTriggeredObject = obj;
            Enter();
        }
        
        protected virtual void Exit(GameObject obj) {
            if(!enabled) return;
            LastTriggeredObject = obj;
            Exit();
        }

        [Button(ButtonMode.EnabledInPlayMode)]
        private void Enter() {
            feedbackEnter.Invoke(transform,this);
            OnEnterArea?.Invoke(LastTriggeredObject);
        }
        
        [Button(ButtonMode.EnabledInPlayMode)]
        private void Exit() {
            feedbackExit.Invoke(transform,this);
            OnExitArea?.Invoke(LastTriggeredObject);
            LastTriggeredObject = null;
        }
    }
}
