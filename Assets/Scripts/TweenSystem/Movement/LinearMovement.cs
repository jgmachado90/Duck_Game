using CoreInterfaces;
using UnityEngine;

namespace TweenSystem
{
    public class LinearMovement : MonoBehaviour, IActionMonoBehaviour, IStopActionMonoBehaviour
    {
        public Space space = Space.Self;
        public bool unscaledTime;
        public Vector3 velocity;
        public Vector3 acceleration;

        private bool _isPaused = false;
        
        public void Pause()
        {
            _isPaused = true;
        }

        public void Unpause()
        {
            _isPaused = false;
        }

        private void Update()
        {
            if(_isPaused) return;
            float deltaTime = unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                                                    transform.Translate(velocity*deltaTime, space);
            velocity += acceleration * deltaTime;
        }

        public void InvokeAction()
        {
            enabled = true;
        }

        public void StopAction()
        {
            enabled = false;
        }
    }
}
