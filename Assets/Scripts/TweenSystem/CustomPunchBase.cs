using System;
using UnityEngine;

namespace TweenSystem
{
	public abstract class CustomPunchBase : MonoBehaviour {

		public float amplitude=1;
		
		public Action OnComplete;

		public virtual void Punch()
		{
			
		}

		public virtual void Punch(Transform t,Action action = null)
		{
		}
		
		public virtual void Stop(Transform t)
		{
		}
	}
}
