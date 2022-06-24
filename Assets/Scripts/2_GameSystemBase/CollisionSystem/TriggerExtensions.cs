using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollisionSystem{
    public class GenerateTriggersParams {
        public CollisionPreset collisionPreset;
        public bool triggerEnter;
        public bool triggerExit;
        public bool generateOnlyTriggerColliders = true;
    }
    
    public class GenerateTriggersResult {
        public List<GameObject> gameObjects = new List<GameObject>();
        public List<CollisionEventBase> triggersEnter = new List<CollisionEventBase>();
        public List<CollisionEventBase> triggersExit = new List<CollisionEventBase>();
    }
    
    public enum VolumeMode
    {
        Object2D,
        Object3D
    }  

    public static class TriggerExtensions
    {
        public static TriggerCollisionEventBase CreateTriggerExit(this GameObject gameObject, VolumeMode mode) {
            if(mode == VolumeMode.Object2D) 
                return gameObject.GetComponent<TriggerExit2D>() ?? gameObject.AddComponent<TriggerExit2D>();
            return gameObject.GetComponent<TriggerExit3D>() ?? gameObject.AddComponent<TriggerExit3D>();
        }
        
        public static TriggerCollisionEventBase CreateTriggerEnter(this GameObject gameObject, VolumeMode mode) {
            if(mode == VolumeMode.Object2D) 
                return gameObject.GetComponent<TriggerEnter2D>() ?? gameObject.AddComponent<TriggerEnter2D>();
            return gameObject.GetComponent<TriggerEnter3D>() ?? gameObject.AddComponent<TriggerEnter3D>();
        }
        
        public static TriggerCollisionEventBase CreateTriggerEnter(this GameObject gameObject, GenerateTriggersParams triggersParams, VolumeMode volumeMode) {
            TriggerCollisionEventBase triggerEnter = gameObject.CreateTriggerEnter(volumeMode);
            if (triggersParams.collisionPreset){
                triggerEnter.SetCollisionPreset(triggersParams.collisionPreset);
            }
            return triggerEnter;
        }

        public static TriggerCollisionEventBase CreateTriggerExit(this GameObject gameObject, GenerateTriggersParams triggersParams, VolumeMode volumeMode){ 
            TriggerCollisionEventBase triggerExit = gameObject.CreateTriggerExit(volumeMode);
            if (triggersParams.collisionPreset) {
                triggerExit.SetCollisionPreset(triggersParams.collisionPreset);
            }
            return triggerExit;
        }

        public static GenerateTriggersResult GenerateTriggers(this GameObject gameObject, GenerateTriggersParams triggersParams) {
            
            GenerateTriggersResult generateTriggersResult = new GenerateTriggersResult();
            
            var colliders2D = gameObject.GetComponentsInChildren<Collider2D>(true);
            foreach (var item in colliders2D) {
                if(!item.isTrigger && triggersParams.generateOnlyTriggerColliders) continue;
                GenerateTriggers(triggersParams, item.gameObject, VolumeMode.Object2D, generateTriggersResult);
            }
       
            var colliders = gameObject.GetComponentsInChildren<Collider>(true);
            foreach (var item in colliders) {
                if(!item.isTrigger && triggersParams.generateOnlyTriggerColliders) continue;
                GenerateTriggers(triggersParams,  item.gameObject,VolumeMode.Object3D, generateTriggersResult);
            }

            return generateTriggersResult;
        }

        private static void GenerateTriggers(GenerateTriggersParams triggersParams, GameObject collider, VolumeMode volumeMode, GenerateTriggersResult generateTriggersResult) {
            GameObject trigger = collider;
            if (triggersParams.triggerEnter) {
                generateTriggersResult.triggersEnter.Add(trigger.CreateTriggerEnter(triggersParams,volumeMode));
            }

            if (triggersParams.triggerExit) {
                generateTriggersResult.triggersExit.Add(trigger.CreateTriggerExit(triggersParams,volumeMode));
            }

            if (triggersParams.collisionPreset)
            {
                string layer = triggersParams.collisionPreset.collisionSettings.gameObjectLayer;
                if (!string.IsNullOrEmpty(layer))
                    collider.layer = LayerMask.NameToLayer(layer);
            }
            generateTriggersResult.gameObjects.Add(collider);
        }
    }
}