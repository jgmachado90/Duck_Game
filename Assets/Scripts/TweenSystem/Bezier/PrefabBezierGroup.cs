using System.Collections.Generic;
using UnityEngine;

namespace TweenSystem
{
	public class PrefabBezierGroup : MonoBehaviour {

		public int numObjs;
		public bool lockAt;
		public float angle=90;
		public float step;
		public float offsetCurve;
		public Vector3 distance;
		public List<PrefabBezierCurve> curves;
	
#if UNITY_EDITOR
		[ContextMenu("OnValidate")]
		void OnValidate(){
			for (int i = 0; i < curves.Count; i++) {
				var item = curves[i];
				item.curveOffset = offsetCurve+(step*i);
				item.numObjs = numObjs;
				item.angle = angle;
				item.distance = distance;
				item.lockAt = lockAt;
				item.OnValidate();
			}
		}
#endif
	}
}
