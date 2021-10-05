using System;
using System.Collections;
using Addons.EditorButtons.Runtime;
using CoreInterfaces;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TweenSystem
{
    public class MaterialColorBlink : MonoBehaviour , IActionMonoBehaviour, IStopActionMonoBehaviour
    {
        public float duration = 2;
        [ColorUsage(true, true)] public Color targetColor;
        public Renderer[] renders;
        public string propName = "_BaseColor";
        public bool continuousMode;
        public bool notUseMaterialPropertyBlock;
        public AnimationCurve curve = AnimationCurve.Linear(0,0,1,1);

        private Color[] _iniColors;
        private Coroutine _coroutine;

        private int _id;
        private MaterialPropertyBlock[] _matProp;

        private float _totalDuration;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            _totalDuration = duration;
            _id = Shader.PropertyToID(propName);
            _iniColors = new Color[renders.Length];
            _matProp = new MaterialPropertyBlock[renders.Length];

            for (int i = 0; i < renders.Length; i++)
            {
                _matProp[i] = new MaterialPropertyBlock();
                _iniColors[i] = renders[i].sharedMaterial.GetColor(_id);
            }
        }

        [Button(ButtonMode.EnabledInPlayMode)]
        public void InvokeAction()
        {
            if (continuousMode)
            {
                if( _coroutine != null)
                    _totalDuration = duration;
                else
                {
                    _coroutine = StartCoroutine(AnimColor(ContinuousModeCondition));
                }
            }
            else
            {
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                    _coroutine = null;
                }

                _totalDuration = duration;
                _coroutine = StartCoroutine(AnimColor(NormalModeCondition));
            }
        }

        [Button(ButtonMode.DisabledInPlayMode)]
        private void GetChildRenders() {
            renders = GetComponentsInChildren<Renderer>(true);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        private bool NormalModeCondition(float time)
        {
            return time <= duration;
        }

        private bool ContinuousModeCondition(float _)
        {
            return _totalDuration > 0;
        }

        IEnumerator AnimColor(Predicate<float> condition)
        {
            float time = 0;
            _totalDuration = duration;

            while (condition.Invoke(time))
            {
                yield return null;
                for (int i = 0; i < renders.Length; i++)
                {
                    var evaluate = continuousMode ? time : time / duration;
                    Color color = Color.Lerp(_iniColors[i], targetColor,
                        curve.Evaluate(evaluate));
                    if (!notUseMaterialPropertyBlock)
                    {
                        _matProp[i].SetColor(_id, color);
                        renders[i].SetPropertyBlock(_matProp[i]);
                    }
                    else
                    {
                        renders[i].material.SetColor(_id,color);
                    }
                }
            
                time += Time.deltaTime;
                _totalDuration -= Time.deltaTime;
            }

            SetBeginColor();

            _coroutine = null;
        }

        public void StopAction()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            SetBeginColor();
        }

        private void SetBeginColor()
        {
            for (int i = 0; i < renders.Length; i++)
            {
                if (!notUseMaterialPropertyBlock)
                {
                    _matProp[i].SetColor(_id, _iniColors[i]);
                    renders[i].SetPropertyBlock(_matProp[i]);
                }
                else
                {
                    renders[i].material.SetColor(_id, _iniColors[i]);
                }
            }
        }
    }
}
