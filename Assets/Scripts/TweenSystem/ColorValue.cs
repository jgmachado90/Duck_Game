using UnityEngine;
using UnityEngine.Events;

namespace TweenSystem
{
    [AddComponentMenu("AnimValue/ColorValue")]
    public class ColorValue : ObjectValue {

        [GradientUsage(true)] public Gradient gradient;

        //[SerializeField, HideInInspector] private float value;

        [System.Serializable] public class ColorEvent : UnityEvent<Color> { }
        public ColorEvent onSetColor;

#if UNITY_EDITOR
        [Space(15), Header("Editor"), Space(5)]
        public bool validate = true;
        [Range(0, 1)] public float _value = 0f;


        void OnValidate() {
            if (!validate || Application.isPlaying || onSetColor == null)
                return;
            SetValue(_value);
        }
#endif

        /* ublic override float getValue() {
        return value;
    }*/

        public override void SetValue(float value) {
            onSetColor.Invoke(gradient.Evaluate(value));
        }
    }
}
