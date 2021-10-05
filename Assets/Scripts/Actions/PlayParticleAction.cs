using CoreInterfaces;
using UnityEngine;

namespace GameFramework
{
    public class PlayParticleAction : MonoBehaviour, IActionMonoBehaviour
    {
        public bool playOnEnable;
        public ParticleSystem particle;

        private void OnEnable() {
            if (playOnEnable) InvokeAction();
        }

        public void InvokeAction()
        {
            particle.Play();
        }
    }
}
