using UnityEditor;
using UnityEngine;

namespace TweenSystem.Editor
{
	[CustomEditor(typeof(BezierCurve))]
	public class BezierEditor : UnityEditor.Editor {
	
		private float _sphereSize  = 1f;
		private Color _colorSphere1 = new Color(1f, 0f, 0f,0.5f);
		private Color _colorSphere2 = new Color(0, 0f, 1f,0.5f);
	
		void OnSceneGUI () 
		{
			BezierCurve myTarget = (BezierCurve)target;
			
			if (myTarget.drawSpheres)
			{
				_sphereSize = myTarget.transform.localScale.magnitude/3;
				
				if (myTarget.p1)
				{
					Handles.color = _colorSphere1;
					myTarget.p1.position = Handles.FreeMoveHandle(
						myTarget.p1.position,
						Quaternion.identity,_sphereSize,myTarget.transform.localScale,Handles.SphereHandleCap);
				}

				if (myTarget.p2)
				{
					Handles.color = _colorSphere2;
					myTarget.p2.position = Handles.FreeMoveHandle(
						myTarget.p2.position,
						Quaternion.identity,_sphereSize,myTarget.transform.localScale,Handles.SphereHandleCap);
				}

				if (myTarget.p3)
				{
					Handles.color = _colorSphere2;
					myTarget.p3.position = Handles.FreeMoveHandle(
						myTarget.p3.position,
						Quaternion.identity,_sphereSize,myTarget.transform.localScale,Handles.SphereHandleCap);
				}

				if (myTarget.p4)
				{
					Handles.color = _colorSphere1;
					myTarget.p4.position = Handles.FreeMoveHandle(
						myTarget.p4.position,
						Quaternion.identity,_sphereSize,myTarget.transform.localScale,Handles.SphereHandleCap);
				}
			}
			else
			{
				Handles.color = new Color(1,1,1,0.5f);
				if(myTarget.p1)
					myTarget.p1.position = Handles.PositionHandle(myTarget.p1.position, Quaternion.identity);
				if(myTarget.p2)
					myTarget.p2.position = Handles.PositionHandle(myTarget.p2.position, Quaternion.identity);
				if(myTarget.p3)
					myTarget.p3.position = Handles.PositionHandle(myTarget.p3.position, Quaternion.identity);
				if(myTarget.p4)
					myTarget.p4.position = Handles.PositionHandle(myTarget.p4.position, Quaternion.identity);
			}
		}
	}
}