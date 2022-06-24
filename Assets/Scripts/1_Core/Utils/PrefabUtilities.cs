using UnityEngine;

namespace Utils
{
    public static class PrefabUtilities
    {
        public static bool IsPrefab(this GameObject gameObject)
        {
            return string.IsNullOrEmpty(gameObject.scene.name);
        }
    }
}
