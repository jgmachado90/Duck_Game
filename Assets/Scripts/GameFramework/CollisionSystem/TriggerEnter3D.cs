using UnityEngine;

namespace GameFramework
{
	public class TriggerEnter3D : TriggerCollisionEventBase {

		private void OnTriggerEnter (Collider col)
		{
			Collision(col.gameObject);
		}
	}
}
