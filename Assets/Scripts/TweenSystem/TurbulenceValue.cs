using UnityEngine;
using UnityEngine.Events;

namespace TweenSystem
{
    public class TurbulenceValue : ObjectValue
    {
        public float modulation=0.1f;
        public float speed=1f;

        [System.Serializable] public class FloatEvent: UnityEvent<float>{}
        public FloatEvent onSetValue;
        public override void SetValue(float value)
        {
            float add = Mathf.PerlinNoise(Time.time*speed, Time.time*speed) * 2 - 1;
            onSetValue.Invoke(value * (1 + add*modulation));
        }
    }
}
