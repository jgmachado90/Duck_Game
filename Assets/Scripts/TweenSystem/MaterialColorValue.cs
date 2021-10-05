using System;
using System.Collections;
using UnityEngine;

namespace TweenSystem
{
    public class MaterialColorValue : ObjectValue
    {
        public bool setIniValueOnEnable;
        public bool setIniValueOnDisable;
        public bool skinnedMesh;
        public TRenderer[] renderers;
        public TOriginalMaterial[] originalMaterials;
        public TGlobalVariables[] globalVariables;

        public float seekValueVelocity = 0;

        private float _lastvalue;
        private float _target;
        private Coroutine _coroutine = null;
    
        [Serializable]
        public class TMaterial {
            public int idMaterial;
            public string prop;
            [GradientUsage(true)]
            public Gradient colorCurve;
            
            public MaterialPropertyBlock PropertyBlock;
        }

        [Serializable]
        public class TOriginalMaterial {
            public Material material;
            public string prop;
            [GradientUsage(true)]
            public Gradient colorCurve;
        }

        [Serializable]
        public class TRenderer{
            public Renderer[] renders;
            public TMaterial[] materials;
        }
    
        [Serializable]
        public class TGlobalVariables
        {
            public string prop;
            [GradientUsage(true)]
            public Gradient colorCurve;
        }

    

#if UNITY_EDITOR
        [Space(15), Header("Editor"), Space(5)]
        public bool validate=true;
        [Range(-10,10)] public float _value = 0f;

        void OnValidate () {
            if (!validate || Application.isPlaying)
                return;
        
            Color color = Color.black;

            if (originalMaterials != null)
            {
                foreach (var mat in originalMaterials)
                {
                    color = mat.colorCurve.Evaluate(_value);
                    mat.material.SetColor(mat.prop,color);
                }
            }

            if (globalVariables != null)
            {
                foreach (var prop in globalVariables)
                {
                    color = prop.colorCurve.Evaluate(_value);
                    Shader.SetGlobalColor(prop.prop, color);
                }
            }
        }
#endif

        private void OnEnable()
        {
            if(setIniValueOnEnable)
                SetValue(0);
        }

        private void OnDisable()
        {
            if(setIniValueOnDisable)
                SetValue(0);
        }

        public override void SetValue(float value)
        {
            if(!Application.isPlaying) return;
//        Debug.Log(value);
            _target = value;
            if (seekValueVelocity <= 0)
            {
                ChangeColor(value);
            }
            else if(_coroutine == null)
            {
                _coroutine = StartCoroutine(UpdateValue());
            }
        }

        private void ChangeColor(float value)
        {
            Color color = Color.black;
            for (int i = 0; i < renderers.Length; i++)
            {
                var rend = renderers[i];
                for (var index = 0; index < rend.materials.Length; index++)
                {
                    var t = rend.materials[index];
                    TMaterial mat = t;
                    color = mat.colorCurve.Evaluate(value);
                    for (int k = 0; k < rend.renders.Length; k++)
                    {
                        var render = rend.renders[k];
                        
                        if(skinnedMesh)
                        {
                            render.materials[mat.idMaterial].SetColor(mat.prop, color);
                        }
                        else
                        {
                            if (mat.PropertyBlock == null) mat.PropertyBlock = new MaterialPropertyBlock();
                            mat.PropertyBlock.SetColor(mat.prop, color);
                            render.SetPropertyBlock(mat.PropertyBlock, mat.idMaterial);
                        }
                    }
                }
            }

            foreach (var mat in originalMaterials)
            {
                color = mat.colorCurve.Evaluate(value);
                mat.material.SetColor(mat.prop, color);
            }

            foreach (var prop in globalVariables)
            {
                color = prop.colorCurve.Evaluate(value);
                Shader.SetGlobalColor(prop.prop, color);
            }

            _lastvalue = value;
        }

        private IEnumerator UpdateValue()
        {
            while (_lastvalue != _target)
            {
                yield return null;
                _lastvalue = Mathf.MoveTowards(_lastvalue, _target, seekValueVelocity);
                ChangeColor(_lastvalue);
            }

            _coroutine = null;
        }
    }
}
