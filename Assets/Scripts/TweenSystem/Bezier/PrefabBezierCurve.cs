using System.Collections.Generic;
using UnityEngine;

namespace TweenSystem
{
	public class PrefabBezierCurve : MonoBehaviour {
	
		public int numObjs;
		public GameObject prefab;
		public BezierCurve bezierCurve;
		public bool lockAt;
		public float curveOffset;
		public float angle=90;
		public Vector3 offset;
		public Vector3 distance;
		public List<GameObject> objs;
	
		void Update(){
			SetPositions();
		}

		void SetPositions(){
			for (int i = 0; i < numObjs; i++) {
				if(objs[i] != null){
					objs[i].transform.position = CalcPos(curveOffset,offset,i,numObjs);
					if(lockAt){
						objs[i].transform.LookAt(CalcPos(curveOffset+0.05f,offset,i,numObjs));
						objs[i].transform.Rotate(0,-90,0);
					}
					Vector3 relative = objs[i].transform.InverseTransformPoint(CalcPos(curveOffset+0.1f,offset,i,numObjs));
					float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
					objs[i].transform.Rotate(0,0, angle + this.angle);
					objs[i].transform.Translate(distance,Space.Self);
				}
			}
		}

		Vector3 CalcPos(float offSetCurva, Vector3 offSet,int i,int num){
			return bezierCurve.GetPos((float)(i+offSetCurva)/(float)num)+offSet;
		}
	
#if UNITY_EDITOR
		[ContextMenu("OnValidate")]
		public void OnValidate(){
			Instantiate();
			SetPositions();
		}

		void Instantiate(){
			numObjs = Mathf.Clamp(numObjs,0,int.MaxValue-1);
			if(!bezierCurve || !prefab) return;
			if(numObjs > objs.Count){
				for (int i = objs.Count; i < numObjs; i++) {
					GameObject g = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject;
					objs.Add(g);
					g.transform.parent = transform;
				}
			}else if(numObjs < objs.Count){
				for (int i = objs.Count-1; i >= numObjs; i--) {
					GameObject g = objs[i];
					UnityEditor.EditorApplication.delayCall +=()=>{
						DestroyImmediate(g);
						objs.Remove(g);
					};
				}
			}
		}
#endif
	}
}
