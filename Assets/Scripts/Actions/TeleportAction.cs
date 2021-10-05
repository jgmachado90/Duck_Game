using CoreInterfaces;
using UnityEngine;

namespace GameFramework
{
    public class TeleportAction : MonoBehaviour, IActionMonoBehaviour
    {
        [SerializeField] private CollisionEventBase collisionEventBase;
        [SerializeField] private Transform destination;
        [SerializeField] private Transform objToTeleport;

        private void Start()
        {
            if (collisionEventBase)
            {
                collisionEventBase.OnCollisionEnter += Invoke;
            }
        }

        public void InvokeAction() {
            if (!objToTeleport || !destination) return;
            Teleport(objToTeleport, destination);
        }

        public void Invoke(GameObject gObject)
        {
            if (!destination) return;
            Teleport(gObject.transform, destination);
        }
        
        public void Invoke(Transform objTransform, Transform teleportTo)
        {
            Teleport(objTransform, teleportTo);
        }

        private void Teleport(Transform obj, Transform dest)
        {
            obj.position = dest.position;
        }
    }
}
