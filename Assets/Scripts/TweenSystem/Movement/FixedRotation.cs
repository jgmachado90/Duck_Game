using UnityEngine;

namespace TweenSystem
{
	public class FixedRotation : MonoBehaviour {

		Vector3 rot;
		void Start () {
			rot = transform.eulerAngles;
		}
	
		public void LateUpdate () {
			transform.eulerAngles = rot;
		}
	}
}
