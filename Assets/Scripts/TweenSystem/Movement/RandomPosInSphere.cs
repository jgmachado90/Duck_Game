using CoreInterfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TweenSystem
{
    public class RandomPosInSphere : MonoBehaviour
    {
        public Transform target;
        public Cooldown cooldown;
        public float radius = 1;

        private void Update()
        {
            if (cooldown.Finished())
            {
                cooldown.Init();
                target.localPosition = Random.insideUnitSphere * radius;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position,radius);
        }
    }
}
