using Addons.EditorButtons.Runtime;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

#endif

//[ExecuteInEditMode]
namespace TweenSystem
{
    public class NoiseRotation : MonoBehaviour
    {
        public float rotationSpeed;
        public float noiseTiltInfluence;
        public float noiseFrequency = 0.2f;
        public bool randomizeOnStart;

        private Quaternion rotation;
        private float noiseValue;
        private float targetNoiseValue;

        [Button("Randomize Rotation")]
        public void RandomRotation()
        {
#if UNITY_EDITOR
            Randomize();
            EditorUtility.SetDirty(this);
#endif  
        }

        private void Start()
        {
            if (randomizeOnStart)
            {
                Randomize();
            }
        }

        void Update()
        {
            var direction = (targetNoiseValue > noiseValue ? 1 : -1);
            noiseValue += direction * noiseFrequency * Time.deltaTime;
            //angularTilt = Mathf.MoveTowards(angularTilt, targetTiltValue, angularAcceleration * Time.deltaTime);

            if ((noiseValue >= targetNoiseValue) == (direction >= 0))
            {
                noiseValue = targetNoiseValue;
                targetNoiseValue = (-1 + Random.value * 2);
            }

            var angularSteering = RotateVector2(Vector2.up * rotationSpeed, noiseValue * noiseTiltInfluence);
            transform.rotation = Quaternion.Euler(angularSteering.x * Time.deltaTime, angularSteering.y * Time.deltaTime, 0) * transform.rotation;
        }

        Vector2 RotateVector2(Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }

        void Randomize()
        {
            transform.rotation = Random.rotation;
        }

        public void Pause()
        {
            enabled = false;
        }

        public void Unpause()
        {
            enabled = true;
        }
    }
}