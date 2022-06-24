using UnityEngine;

namespace Utils
{
    public static class LayerExtensions  {

        public static bool ContainsLayer(this LayerMask layermask, GameObject gameObject) {
            return (1 << gameObject.layer | layermask) == layermask;
        }
    
        public static bool IsInLayerMask(int layer, LayerMask layermask)
        {
            return layermask == (layermask | (1 << layer));
        }
    }
}