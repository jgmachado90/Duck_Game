using TweenSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace TweenSystem
{
	public class BezierValue : ObjectValue {
	
		[FormerlySerializedAs("bezier")] public BezierCurve[] beziers;
		public AnimationCurve curve = AnimationCurve.Linear(0,0,1,1);
		public bool lockAt=true;
		public bool invertBezier;

		public override void SetValue(float value) 
		{
			float evaluate = curve.Evaluate(value);
			float idBezier = evaluate * beziers.Length;
		
			if((int)idBezier <beziers.Length){
				float percentageBezier = idBezier%1f;
				float front = percentageBezier+0.1f;
				if(invertBezier){
					front = 1-(percentageBezier-0.1f);
					percentageBezier = 1- percentageBezier;
				}

				BezierCurve bezier = beziers[(int) idBezier];
				if (bezier)
				{
					transform.position = bezier.GetPos(percentageBezier);
					if(lockAt){
						Vector3 pos = bezier.GetPos(front);
						Vector3 relativePos = pos - transform.position;
						Quaternion rotation; 
						if(Application.isPlaying) 
							rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(relativePos),3*Time.deltaTime);
						else
							rotation = Quaternion.LookRotation(relativePos);
						transform.rotation = rotation;
					}
				}
			}
		}
	}
}
