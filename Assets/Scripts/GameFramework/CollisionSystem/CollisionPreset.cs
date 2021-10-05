using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameFramework {
    [CreateAssetMenu(fileName = "CollisionPreset", menuName = "CollisionPreset")]
    public class CollisionPreset : ScriptableObject {
        public CollisionSettings collisionSettings;
    }

    [Serializable]
    public struct CollisionSettings
    {
        [FormerlySerializedAs("layer")] public string gameObjectLayer;
        public List<string> collisionTags;
        public GameplayFlags gameplayFlags;

        public bool CheckCollisionFilters(GameObject col, GameplayFlags flags) {
            if(collisionTags != null && collisionTags.Count > 0) {
                 return col.ContainsTag(collisionTags);
            }
            
            if(flags == null || gameplayFlags.ContainsOneFlag(flags)) {
                return true;
            }
            return false;
        }
    }
}