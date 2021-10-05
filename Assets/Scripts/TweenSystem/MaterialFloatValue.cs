using UnityEngine;

namespace TweenSystem
{
	public class MaterialFloatValue : ObjectValue {

		[System.Serializable]
		public class TMaterial {
			public int idMaterial;
			public string prop;
			public MaterialPropertyBlock PropertyBlock;
		}

		[System.Serializable]
		public class TOriginalMaterial {
			public Material material;
			public string prop;
		}

		[System.Serializable]
		public class TRenderer{
			public Renderer render;
			public TMaterial[] materials;
		}

		public bool useIniValue;
		public float iniValue;
		public TRenderer[] renderers;
		public TOriginalMaterial[] originalMaterials;
		public string[] globalVariables;

#if UNITY_EDITOR
		[Space(15), Header("Editor"), Space(5)]
		public bool validate=true;
		[Range(-10,10)] public float _value = 0f;

		void OnValidate () {
			if (!validate || Application.isPlaying)
				return;
			if (originalMaterials != null)
			{
				foreach (var mat in originalMaterials) {
					mat.material.SetFloat(mat.prop,_value);
				}
			}

			if (globalVariables != null)
			{
				foreach (var prop in globalVariables) {
					Shader.SetGlobalFloat(prop,_value);
				}
			}
		}
#endif

		private void Start()
		{
			if(useIniValue)
				SetValue(iniValue);
		}

		public override void SetValue(float valor) 
		{
			if (Application.isPlaying)
			{
				foreach (var rend in renderers) {
					foreach (var mat in rend.materials) {
						
						if (mat.PropertyBlock == null) mat.PropertyBlock = new MaterialPropertyBlock();
						mat.PropertyBlock.SetFloat(mat.prop, valor);
						rend.render.SetPropertyBlock(mat.PropertyBlock,mat.idMaterial);
						//rend.render.materials[mat.idMaterial].SetFloat(mat.prop,valor);
					}
				}
			}

			foreach (var mat in originalMaterials) {
				mat.material.SetFloat(mat.prop,valor);
			}

			foreach (var prop in globalVariables) {
				Shader.SetGlobalFloat(prop,valor);
			}
		}
	}
}
