using UnityEngine;

namespace GameFramework
{
    public class TriggerExit2D : TriggerCollisionEventBase
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            Collision(other.gameObject);
        }
    }
}
