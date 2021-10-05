using UnityEngine;

namespace TweenSystem
{
    public class Vibrator : MonoBehaviour
    {
        public float frequency = 5f;
        public float initialAmplitude = 1f;
        public float decay = 1f;
        public bool RandomizeRotationOnEnable;

        private float amplitude;
        private float timer;

        private void OnEnable()
        {
            amplitude = initialAmplitude;
            timer = 0;

            if (RandomizeRotationOnEnable)
            {
                RandomizeAxis();
            }
        }

        private void Update()
        {
            if (amplitude > 0)
            {
                timer += Time.deltaTime;
                transform.localPosition = Vector3.forward * amplitude * Mathf.Sin(timer * Mathf.PI * 2 * frequency);
                amplitude = Mathf.MoveTowards(amplitude, 0, decay * Time.deltaTime);
            }
        }

        public void RandomizeAxis()
        {
            transform.forward = Random.insideUnitSphere;
        }
    }
}