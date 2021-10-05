using System;
using UnityEngine;

namespace TweenSystem
{
	[AddComponentMenu("AnimValue/AnimatorValue")]
	public class AnimatorFloatValue : ObjectValue {

		public string param;
		public Animator anim;
		public AnimationCurve curve;
		public float velSeekValue=1;
		public bool unscaleDeltaTime;
	
		private Func<float> GetDeltaTime;
		public float GetScaledDeltaTime() => Time.deltaTime;
    
		public float GetSUnscaledDeltaTime() => Time.unscaledTime;

		void Reset() {
			anim = GetComponent<Animator>();
		}

		private void Start()
		{
			if (!unscaleDeltaTime)
			{
				GetDeltaTime = GetScaledDeltaTime;
			}
			else
			{
				GetDeltaTime = GetSUnscaledDeltaTime;
			}
		}

		public override void SetValue(float value) {
			anim.SetFloat(param, curve.Evaluate(value) );
		}

		public void SeekValue(float value) {
			value = curve.Evaluate(value);
			float f = anim.GetFloat(param);
			f = Mathf.MoveTowards(f,value,velSeekValue*GetDeltaTime());
			anim.SetFloat(param, f);
		}
	}
}
