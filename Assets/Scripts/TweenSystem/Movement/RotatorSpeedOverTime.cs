using UnityEngine;

namespace TweenSystem
{
    [RequireComponent(typeof(Rotator))]
    public class RotatorSpeedOverTime : MonoBehaviour
    {
        public AnimationCurve speedMultiplierOverTime;

        private Rotator rotator;
        private float timer;

        void Awake()
        {
            rotator = GetComponent<Rotator>();
        }

        private void OnEnable()
        {
            timer = 0;
        }

        void Update()
        {
            timer += Time.deltaTime;
            rotator.multiplier = speedMultiplierOverTime.Evaluate(timer);
//            Debug.Log("oi "+rotator.multiplier);
        }
    }
}
