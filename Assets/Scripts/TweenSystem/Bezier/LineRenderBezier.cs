using UnityEngine;

namespace TweenSystem
{
	public class LineRenderBezierCurve : MonoBehaviour {

		public BezierCurve bezier;
		public LineRenderer lineRenderer;
		public Vector3 offset;
		public int numPoints;

		void Reset(){
			lineRenderer = GetComponent<LineRenderer>();
		}
	
		void Update(){
			OnValidate();
		}
	
		[ContextMenu("OnValidate")]
		void OnValidate(){
			numPoints = Mathf.Clamp(numPoints,0,int.MaxValue-1);
			if(!bezier || !lineRenderer) return;
			lineRenderer.positionCount = numPoints+1;
			for (int i = 0; i < numPoints+1; i++){
				lineRenderer.SetPosition(i, bezier.GetPos((float)i/(float)numPoints)+offset);
			}
		}
	}
}
