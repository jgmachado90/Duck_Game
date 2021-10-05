using CoreInterfaces;
using UnityEngine;

namespace TweenSystem
{
    public class ActiveValue : ObjectValue, IActionMonoBehaviour {

        public AnimationCurve curve;

#if UNITY_EDITOR
        [Space(15), Header("Editor"), Space(5)]
        public bool validate = true;
        [Range(0, 1)] public float _value = 0f;

        public float Value{
            get{return _value;}
            set{
                _value = value;
                SetValue(_value);
            }
        }


        void OnValidate() {
            if (!validate || Application.isPlaying)
                return;
            SetValue(_value);
        }
#endif

        public override void SetValue(float value) {
            float r = curve.Evaluate(value);
            if (r > 0 && !gameObject.activeSelf) {
                gameObject.SetActive(true);
            } else if(r <= 0 && gameObject.activeSelf) {
                gameObject.SetActive(false);
            }
        }

        public void InvokeAction() {
            gameObject.SetActive(true);
        }
    }
}
