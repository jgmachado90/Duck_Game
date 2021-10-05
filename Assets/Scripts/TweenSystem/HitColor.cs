using Addons.EditorButtons.Runtime;
using CoreInterfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace TweenSystem
{
	public class HitColor : MonoBehaviour, IActionMonoBehaviour {

		[System.Serializable]
		public struct TMaterial {
			[FormerlySerializedAs("idsMateriais")] public string idsMaterials;
			[FormerlySerializedAs("corHit")] [ColorUsage(true,true)] public Color colorHit;
			[FormerlySerializedAs("nomeProp")] public string propName;
			[FormerlySerializedAs("cores")] [ColorUsage(true,true)] public Color[] colors;

			[HideInInspector, SerializeField]
			public int[] ids;
		}
	
		public Renderer render;
		public Vector2 fadeIn = new Vector2(0.1f, 0f);
		public Vector2 fadeOut = new Vector2(0.2f, 0.3f);
		[FormerlySerializedAs("materiais")] public TMaterial[] materials;

		private float _tHit = -10f;
	
		private bool _updateColors= true;

		private MaterialPropertyBlock _material;
	
		public void InvokeAction()
		{
			SetHit();
		}

		[Button("Get Colors",ButtonMode.DisabledInPlayMode)]
		private void Start () 
		{
			if (!render)
			{
				render = GetComponentInChildren<Renderer>();
			}

			_material = new MaterialPropertyBlock();
			GetColors();
		}
	
		public void GetColors() 
		{
			if(!render) return;
			string[] split;
			for (int i=0; materials!=null && i<materials.Length; i++) 
			{
				if ( string.IsNullOrEmpty(materials[i].propName) )
					materials[i].propName = "_Color";
				split = materials[i].idsMaterials.Split(',');
				materials[i].ids = new int[split.Length];
				materials[i].colors = new Color[split.Length];
				for (int j = 0; j < split.Length; j++) {
					int.TryParse(split[j], out materials[i].ids[j]);

					if (render.sharedMaterials[materials[i].ids[j]].HasProperty(materials[i].propName)) {
						materials[i].colors[j] = render.sharedMaterials[materials[i].ids[j]].GetColor(materials[i].propName);
					}else if (Application.isPlaying) {
						_updateColors = false;
						enabled = false;
					}
					//throw;
					//materiais[i].cores[j] = render.sharedMaterials[ materiais[i].ids[j] ].GetColor(materiais[i].nomeProp);
				}
			}
		}
	
		public Color GetCorHit(Color corHit, Color corInimigo)
		{
			float t = 0f;
			t += Mathf.InverseLerp(fadeIn.x, fadeIn.y, Time.time-_tHit);
			t += Mathf.InverseLerp(fadeOut.x, fadeOut.y, Time.time-_tHit);
			if(t>=1) enabled = false;
			return Color.Lerp(corHit, corInimigo, t);
		}

		[Button("Set Hit",ButtonMode.EnabledInPlayMode)]
		public void SetHit ()
		{
			if(_updateColors)
				enabled = true;
			_tHit = Time.time;
			//Debug.Log("set hit");
		}

		private void LateUpdate() 
		{
			UpdateColor();
		}

		public void UpdateColor()
		{
			if(!render) return;
		
			for (int i=0; materials!=null && i<materials.Length; i++) 
			{
				for (int j=0; j<materials[i].ids.Length; j++) 
				{
					_material.SetColor(materials[i].propName,GetCorHit( materials[i].colorHit, materials[i].colors[j]));
					render.SetPropertyBlock(_material,materials[i].ids[j]);
					//render.materials[ materials[i].ids[j] ].SetColor(materials[i].propName , GetCorHit( materials[i].colorHit, materials[i].colors[j] ));
				}
			}	
		}

		public void SetColor(Color color)
		{
			for (int i=0; materials!=null && i<materials.Length; i++)
			{
				for (int j=0; j<materials[i].ids.Length; j++) 
				{
					render.materials[ materials[i].ids[j] ].SetColor(materials[i].propName , color);
				}
			}
		}

		public void SetColor2(Color color)
		{
			for (int i=0; materials!=null && i<materials.Length; i++) 
			{
				for (int j=0; j<materials[i].ids.Length; j++) 
				{
					render.material.SetColor(materials[i].propName , color);
				}
			}
		}
	}
}
