using System;
using Addons.EditorButtons.Runtime;
using CoreInterfaces;
using UnityEngine;

namespace TweenSystem
{
	public class ScalePunch : CustomPunchBase, IActionMonoBehaviour
	{
		[Header("Scale")]
		public bool useTransformScaleForNormalScale;
		public Vector3 normalScale=Vector3.one;
		public Vector3 secondScale=Vector3.one;
		public EasingCurve easeIn;
		public float duration;
		public EasingCurve easeOut;
		public float duration2;

		private Sequence _mySequence;

		private void Start()
		{
			if (useTransformScaleForNormalScale)
			{
				normalScale = transform.localScale;
			}
		}

		[Button(ButtonMode.EnabledInPlayMode)]
		public override void Punch()
		{
			if(!gameObject.activeSelf) return;
			Vector3 iniScale = normalScale;
			if (_mySequence != null)
			{
				_mySequence.Stop();
				transform.localScale = iniScale;
			}
			
			_mySequence = new Sequence();
			_mySequence.Append(transform.DoScale(secondScale * amplitude, duration,this).SetEase(easeIn));
			_mySequence.Append(transform.DoScale(iniScale, duration2,this).SetEase(easeOut));
			_mySequence.OnComplete(OnComplete);

			//StartCoroutine(TweenExtensions.To(() => transform.localScale, (o) => transform.localScale = o, secondScale,duration));
		}

		private void OnDisable()
		{
			if (_mySequence != null)
			{
				_mySequence.Stop();
			}
		}

		public void Punch(float amplitude){
			this.amplitude = amplitude;
			Punch();
		}

		public void InvokeAction()
		{
			Punch();
		}
	}
}