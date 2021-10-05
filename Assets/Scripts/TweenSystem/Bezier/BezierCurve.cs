using UnityEngine;

namespace TweenSystem
{
	public class BezierCurve : MonoBehaviour{
	
		public Transform p1,p2,p3,p4;
		public bool loop;

		[Space(), Header("Editor"), Range(0.001f,1)]
		public float step = 0.01f;
		public bool drawGizmos = true;
		public bool drawSpheres = true;
	
		public void OnValidate()
		{
			step = Mathf.Clamp(step,0.001f,1);
		}

		public Vector3 GetPos(float t)
		{
			return GetPos(p1.position,p2.position,p3.position,p4.position, t,loop);
		}

		public static Vector3 GetPos(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t, bool loop =false) 
		{
			if (loop)
			{
				if (t <= 0.5f)
				{
					t *= 2;
					float u = 1-t;
					return p1*u*u*u + p2*3*t*u*u + p3*3*t*t*u + p4*t*t*t;
				}
				else
				{
					t = (t - 0.5f) * 2;
					float u = 1-t;

					Vector3 newP2 = p1 + (p1 - p2); 
					Vector3 newP3 = p4 + (p4 - p3); 
				
					return p4*u*u*u + newP3*3*t*u*u + newP2*3*t*t*u + p1*t*t*t;
				}
			}
			else
			{
				float u = 1-t;
				return p1*u*u*u + p2*3*t*u*u + p3*3*t*t*u + p4*t*t*t;
			}
		}

		GameObject NewTransform(string nome)
		{
			GameObject g = new GameObject(nome);
			g.transform.parent = transform;
			g.transform.localPosition = Vector3.zero;
			return g;
		}
	
		private void Verifiy()
		{
			if(p1 == null){
				p1 = NewTransform("p1").transform;
				p1.position+=Vector3.left*5;
			}
			if(p2 == null){
				p2 = NewTransform("p2").transform;
				p2.position+=Vector3.left*5;
				p2.position+=Vector3.up*7;
				p2.parent = p1;
			}
			if(p3 == null){
				p3 = NewTransform("p3").transform;
				p3.position+=Vector3.right*5;
				p3.position+=Vector3.up*7;
			}
			if(p4 == null){
				p4 = NewTransform("p4").transform;
				p4.position+=Vector3.right*5;
				p3.parent = p4;
			}
		}

		private void OnDrawGizmos()
		{												
			Verifiy();
			
			if(!drawGizmos) return;
			
			float u=0;
			float i=0;
			
			Vector3 puantes;
			Vector3 Pu = GetPos(p1.position,p2.position,p3.position,p4.position, u,loop);
			Gizmos.DrawLine(p2.position,p1.position);
			Gizmos.DrawLine(p3.position,p4.position);
			
			while(u != 1)
			{
				puantes = Pu;	
				u = Mathf.Lerp(u,1,i);
				Pu = GetPos(p1.position,p2.position,p3.position,p4.position, u,loop);
				if (u != 0.0f) {
					Gizmos.color = Color.green;
					Gizmos.DrawLine (Pu, puantes);
				}
				i += step;
			}
		}
	
		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawLine(p2.position,p1.position);
			Gizmos.DrawLine(p3.position,p4.position);
		}
	}
}


