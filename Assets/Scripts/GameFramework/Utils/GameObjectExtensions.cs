using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class GenerateTriggersParams {
        public VolumeMode mode;
        public string defaultCollisionTag;
        public CollisionPreset collisionPreset;
        public bool triggerEnter;
        public bool triggerExit;
        public Action<GameObject> enter;
        public Action<GameObject> exit;
    }
    
    public enum VolumeMode
    {
        Object2D,
        Object3D
    }  

    public static class GameObjectExtensions
    {
        public static bool ContainsTag(this GameObject gameObject, List<string> tags) {
            for(int i = 0; i < tags.Count; i++) {
                if(gameObject.CompareTag(tags[i])){
                    return true;
                }
            }
            return false;
        }
        
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
        
        public static TriggerCollisionEventBase CreateTriggerEnter(this GameObject gameObject, GenerateTriggersParams triggersParams) {
            TriggerCollisionEventBase triggerEnter = gameObject.CreateTriggerEnter(triggersParams.mode);
            triggerEnter.OnCollisionEnter += triggersParams.enter;
            if (triggersParams.collisionPreset) {
                triggerEnter.SetCollisionPreset(triggersParams.collisionPreset);
            }else if (!string.IsNullOrEmpty(triggersParams.defaultCollisionTag)) {
                triggerEnter.collisionSettings.collisionTags = new List<string>() {triggersParams.defaultCollisionTag};
            }
            return triggerEnter;
        }

        public static TriggerCollisionEventBase CreateTriggerExit(this GameObject gameObject, GenerateTriggersParams triggersParams){ 
            TriggerCollisionEventBase triggerExit = gameObject.CreateTriggerExit(triggersParams.mode);
            triggerExit.OnCollisionEnter += triggersParams.exit;
            if (triggersParams.collisionPreset) {
                triggerExit.SetCollisionPreset(triggersParams.collisionPreset);
            }else if (!string.IsNullOrEmpty(triggersParams.defaultCollisionTag)) {
                triggerExit.collisionSettings.collisionTags = new List<string>() {triggersParams.defaultCollisionTag};
            }
            return triggerExit;
        }

        public static void GenerateTriggers(this GameObject gameObject, GenerateTriggersParams triggersParams, 
            out List<GameObject> gameObjects,
            out List<CollisionEventBase> triggers) {
            
            gameObjects = new List<GameObject>();
            triggers = new List<CollisionEventBase>();

            if (triggersParams.mode == VolumeMode.Object2D) {
                var colliders = gameObject.GetComponentsInChildren<Collider2D>(true);
                foreach (var item in colliders) {
                    if(!item.isTrigger) continue;
                    GenerateTriggers(triggersParams, item.gameObject, gameObjects,triggers);
                }
            }
            else {
                var colliders = gameObject.GetComponentsInChildren<Collider>(true);
                foreach (var item in colliders) {
                    if(!item.isTrigger) continue;
                    GenerateTriggers(triggersParams, item.gameObject,gameObjects, triggers);
                }
            }
        }

        private static void GenerateTriggers(GenerateTriggersParams triggersParams, GameObject collider, List<GameObject> GOtriggers,  List<CollisionEventBase> triggers) {
            GameObject trigger = collider;
            if (triggersParams.triggerEnter) {
                triggers.Add(trigger.CreateTriggerEnter(triggersParams));
            }

            if (triggersParams.triggerExit) {
                triggers.Add(trigger.CreateTriggerExit(triggersParams));
            }

            if (triggersParams.collisionPreset)
            {
                string layer = triggersParams.collisionPreset.collisionSettings.gameObjectLayer;
                if (!string.IsNullOrEmpty(layer))
                    collider.layer = LayerMask.NameToLayer(layer);
            }
            GOtriggers.Add(collider);
        }

        public static Entity FindEntity(this GameObject gameObject) {
            return gameObject.GetComponentInParent<Entity>();
        }
        
        public static T GetOrAddComponent<T>(this MonoBehaviour monoBehaviour) where T: MonoBehaviour
        {
            return monoBehaviour.gameObject.GetOrAddComponent<T>();
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T: MonoBehaviour
        {
            var comp = gameObject.GetComponent<T>();
            if (comp == null)
                comp = gameObject.AddComponent<T>();
            return comp;
        }
    }
}
