using UnityEngine;
using UnityEngine.Events;

namespace TweenSystem
{
	[System.Serializable]
	public class OnSetValue: UnityEvent<float>{}

	public abstract class ObjectValue: MonoBehaviour {

		public virtual void SetValue (float value) {}

		public virtual float GetValue () {
			return 0f;
		}
	}
}