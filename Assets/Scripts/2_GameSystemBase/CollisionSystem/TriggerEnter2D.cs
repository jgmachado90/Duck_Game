using UnityEngine;

namespace CollisionSystem
{
	public class TriggerEnter2D : TriggerCollisionEventBase 
	{
		private void OnTriggerEnter2D(Collider2D other)
		{
			Collision(other.gameObject);
		}
	}
}
