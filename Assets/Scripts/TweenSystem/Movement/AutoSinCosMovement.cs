using UnityEngine;

namespace TweenSystem
{
	public class AutoSinCosMovement : MonoBehaviour {

		public Vector2 vel,amplitude;
		public float lerp=1;
		public bool randomIni;

		Vector3 posIni, newPos, posGizmo;
		float l,sum;
		bool ini;

		void Start(){
			newPos = Vector3.zero;		
			posIni = transform.localPosition;
			ini = true;
			posGizmo = transform.position;
			if(randomIni)
				sum=Random.Range(0f,10f);
		}
	
		void Update () 
		{
			newPos.x = Mathf.Sin(Mathf.PI * Time.time*vel.x+sum)*amplitude.x*l;
			newPos.y = Mathf.Cos(Mathf.PI * Time.time*vel.y+sum)*amplitude.y*l;
			transform.localPosition = posIni + newPos;
			l = Mathf.Lerp(l,1,lerp*Time.deltaTime);
		}

		private void OnDrawGizmos() 
		{

			Gizmos.color = Color.yellow;
			Gizmos.matrix = Matrix4x4.TRS(ini?posGizmo:transform.position,Quaternion.identity,amplitude);
			Gizmos.DrawWireSphere(Vector3.zero,1);
		}
	}
}
