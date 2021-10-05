using UnityEngine;

namespace TweenSystem
{
	public class MaterialCanvasRendererValue : ObjectValue {

		[System.Serializable]
		public struct TMaterial {
			public int idMaterial;
			public string prop;
			public float min, max;
		}

		[System.Serializable]
		public struct TRenderer{
			public CanvasRenderer render;
			public TMaterial[] materials;
		}

		public TRenderer[] renderers;

		public override void SetValue(float value) {
			foreach (var rend in renderers) {
				foreach (var mat in rend.materials) {
					rend.render.GetMaterial(mat.idMaterial).SetFloat(mat.prop,Mathf.Lerp(mat.min,mat.max, value));
				}
			}
		}
	}
}