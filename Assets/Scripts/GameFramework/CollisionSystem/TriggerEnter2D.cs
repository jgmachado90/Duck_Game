using UnityEngine;

namespace GameFramework
{
	public class TriggerEnter2D : TriggerCollisionEventBase 
	{
		private void OnTriggerEnter2D(Collider2D other)
		{
			Collision(other.gameObject);
		}
	}
}
