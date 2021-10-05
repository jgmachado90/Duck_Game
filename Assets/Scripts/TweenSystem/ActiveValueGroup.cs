using CoreInterfaces;
using UnityEngine;

namespace TweenSystem
{
    public class ActiveValueGroup : ObjectValue, IActionMonoBehaviour
    {
        public AnimationCurve curve =AnimationCurve.Linear(0,0,1,1);
        public GameObject[] group;

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

        public override void SetValue(float value) 
        {
            if(group.Length <= 0) return;
            float r = curve.Evaluate(value);
            if (r > 0 && ! group[0].activeSelf)
            {
                ActiveGroup(true);
            } else if(r <= 0 && group[0].activeSelf) {
                ActiveGroup(false);
            }
        }

        public void InvokeAction() {
            gameObject.SetActive(true);
        }

        public void ActiveGroup(bool active)
        {
            for (var i = 0; i < group.Length; i++)
            {
                group[i].SetActive(active);
            }
        }
    }
}
