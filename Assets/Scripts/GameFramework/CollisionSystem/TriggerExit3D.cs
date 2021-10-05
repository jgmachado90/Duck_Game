using UnityEngine;

namespace GameFramework
{
	public class TriggerExit3D : TriggerCollisionEventBase {

		private void OnTriggerExit (Collider col)
		{
			Collision(col.gameObject);
		}
	}
}
