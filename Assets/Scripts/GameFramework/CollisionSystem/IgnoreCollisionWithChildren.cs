using UnityEngine;

namespace GameFramework
{
    [DefaultExecutionOrder(0)]
    public class IgnoreCollisionWithChildren : MonoBehaviour {
        public Transform parent;
        public Collider collider;

        private void Reset() {
            collider = GetComponent<Collider>();
        }

        private void Awake() {
            var colliders = parent.GetComponentsInChildren<Collider>();
            for (var i = 0; i < colliders.Length; i++) {
                var item = colliders[i];
                Physics.IgnoreCollision(collider, item);
            }
        }
    }
}
