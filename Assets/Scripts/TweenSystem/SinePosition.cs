using UnityEngine;

namespace TweenSystem
{
    public class SinePosition : MonoBehaviour
    {
        public Axis x, y, z;
        public bool randomizeDelay;

        [System.Serializable]
        public struct Axis
        {
            public bool useAxis;
            public float positionMin, positionMax;
            public float timePeriod, timeDelay;
        }

        private void Awake()
        {
            if (randomizeDelay)
            {
                if (x.useAxis) x.timeDelay = Random.value * x.timePeriod;
                if (y.useAxis) y.timeDelay = Random.value * y.timePeriod;
                if (z.useAxis) z.timeDelay = Random.value * z.timePeriod;
            }
        }

        void Update()
        {
            var position = transform.localPosition;
            Solve(x, ref position.x);
            Solve(y, ref position.y);
            Solve(z, ref position.z);
            transform.localPosition = position;
        }

        void Solve(Axis axis, ref float value)
        {
            if (axis.useAxis && axis.timePeriod > 0)
            {
                float t = (Time.time - axis.timeDelay) / axis.timePeriod;
                float sin = Mathf.Sin(t * Mathf.PI * 2);
                float lerp = (sin + 1) * .5f;
                value = Mathf.Lerp(axis.positionMin, axis.positionMax, lerp);
            }
        }
    }
}
