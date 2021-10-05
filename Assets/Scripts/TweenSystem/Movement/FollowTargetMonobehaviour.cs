using UnityEngine;

namespace TweenSystem
{
    public abstract class FollowTargetMonobehaviour : MonoBehaviour
    {
        public abstract Transform GetTarget();
        public abstract void SetTarget(Transform t);
    }
}
