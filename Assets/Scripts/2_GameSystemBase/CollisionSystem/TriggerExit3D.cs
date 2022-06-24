using UnityEngine;

namespace CollisionSystem
{
	public class TriggerExit3D : TriggerCollisionEventBase {

		[SerializeField] private bool ignoreTriggers;
		private void OnTriggerExit (Collider col)
		{
			if (ignoreTriggers && col.isTrigger) return;
			Collision(col.gameObject);
		}
	}
}
