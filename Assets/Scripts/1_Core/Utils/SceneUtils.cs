using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public static class SceneUtils
    {
        public static Bounds GetBounds(this Scene scene)
        {
            Bounds b = new Bounds(Vector3.zero,Vector3.zero);
            foreach (var root in scene.GetRootGameObjects())
            {
                foreach (var item in root.GetComponentsInChildren<Renderer>(true)){
                    b.Encapsulate(item.bounds);
                }
            }
            return b;
        }
    }
}