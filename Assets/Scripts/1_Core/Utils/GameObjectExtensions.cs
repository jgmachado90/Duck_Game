using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    

    public static class GameObjectExtensions
    {
        public static void ActiveAll(this GameObject[] gameObject,bool active){
            for (var i = 0; i < gameObject.Length; i++){
                var item = gameObject[i];
                item.SetActive(active);
            }
        }

        public static bool ContainsTag(this GameObject gameObject, List<string> tags) {
            for(int i = 0; i < tags.Count; i++) {
                if(gameObject.CompareTag(tags[i])){
                    return true;
                }
            }
            return false;
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
        
        public static bool ContainsTag(this GameObject gameObject,string[] tags){
            if (tags.Length <= 0) return true;
            for (int j = 0; j < tags.Length; j++){
                if (gameObject.CompareTag(tags[j])){
                    return true;
                }
            }
            return false;
        }
        
        public static bool ContainsGameObject(this GameObject gameObject, GameObject[] ignoredColliders){
            for (int j = 0; j < ignoredColliders.Length; j++){
                if (gameObject == ignoredColliders[j]){
                    return true;
                }
            }
            return false;
        }
    }
}
