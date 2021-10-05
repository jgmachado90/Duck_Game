using System;
using UnityEngine;

namespace TweenSystem
{
	public class BezierCurveNPoints : MonoBehaviour{
	
		public GameObject[] points = new GameObject[3];
	
		Vector3 pos;
		int[ , ] pascal;
	
		void Start ()
		{
			CalcPascalTriangle();		
		}
	
		public void Bezier(Transform objeto, float u) 
		{
			float a = (1 - u);
			float b = u;
			objeto.position = CalcBezier (a, b);
		}

		public Vector3 Bezier(float u)
		{
			float a = (1 - u);
			float b = u;
			return CalcBezier(a, b);
		}
	
		void OnDrawGizmos()
		{
			CalcPascalTriangle ();		
			Vector3 lastPos;
			for (float u = 0.0f; u<1.0f; u+=0.01f) {
				lastPos = pos;
				float a = (1 - u);
				float b = u;	
				pos = CalcBezier (a, b, 0);
			
				if (u != 0.0f) {
					Gizmos.color = Color.red;
					Gizmos.DrawLine (pos, lastPos);
				}
			}
		}
	
		public Vector3 CalcBezier(float a, float b, int index=0){
			if (index > points.Length - 1) {
				return Vector3.zero;
			} else {	
				return pascal [points.Length - 1, index] * 
				       (float)(Math.Pow (a, points.Length - 1 - index)) * 
				       (float)((Math.Pow (b, index))) * 
				       points[index].transform.position + CalcBezier (a, b, index + 1);	
			}
		}
	
		public void CalcPascalTriangle()
		{
			pascal = new int[ points.Length + 1, points.Length + 1 ];
			int n = 0;
			int column = 0;
		
			for (int i = column; i< pascal.GetLength(0); i++){
				for (int j = n; j< pascal.GetLength(0); j++){
					pascal [j, i++] = 1; 
				}
				n ++;
				column++;
			}
			for (int i = column; i< pascal.GetLength(0); i++) {	
				pascal [i, 0] = 1;
			}
			
			for (int i = 2; i< pascal.GetLength(0); i++) {	
				for (int j = 1; j< pascal.GetLength(1)-1; j++) {	
					pascal [i, j] = pascal [i - 1, j - 1] + pascal [i - 1, j];
				}	
			}	
		}
	}
}


