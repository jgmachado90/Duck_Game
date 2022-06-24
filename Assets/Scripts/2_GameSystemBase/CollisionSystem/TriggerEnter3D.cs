using UnityEngine;

namespace CollisionSystem
{
	public class TriggerEnter3D : TriggerCollisionEventBase
	{
		[SerializeField] private bool ignoreTriggers;
		private void OnTriggerEnter (Collider col)
		{
			if (ignoreTriggers && col.isTrigger) return;
			Collision(col.gameObject);
		}
	}
}
