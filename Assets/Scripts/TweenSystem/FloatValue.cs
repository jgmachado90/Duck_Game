using UnityEngine;
using UnityEngine.Events;

namespace TweenSystem
{
	[AddComponentMenu("AnimValue/FloatValue")]
	public class FloatValue : ObjectValue
	{

		public bool notNormalizedTime;
		public float min, max;
		public AnimationCurve curve = AnimationCurve.Linear(0,0,1,1);

		[SerializeField, HideInInspector] private float value;

		[System.Serializable] public class FloatEvent: UnityEvent<float>{}
		public FloatEvent onSetValue;

#if UNITY_EDITOR
		[Space(15), Header("Editor"), Space(5)]
		public bool validate=true;
		[Range(0,1)] public float _value = 0f;


		void OnValidate () {
			if (!validate || Application.isPlaying || onSetValue == null)
				return;
			SetValue(_value);
		}
#endif

		public override float GetValue () {
			return value;
		}

		public override void SetValue (float value) {
			if(!notNormalizedTime)
				value = Mathf.Lerp (0, curve.keys [curve.keys.Length - 1].time, value);
			this.value = Mathf.LerpUnclamped (min, max, curve.Evaluate (value));
			//Debug.Log(value + " " + this.value);
//#if UNITY_EDITOR
//        _value = this.value;
//#endif
			onSetValue.Invoke(this.value);
		}
	}
}
