using System;
using System.Collections.Generic;
using Addons.EditorButtons.Runtime;
using CoreInterfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace TweenSystem
{
    public class PositionPunch : CustomPunchBase, IActionMonoBehaviour
    {
        public TranslationSettings translationSettings;

        private Dictionary<int,PositionPunchElement> _positionPunchElements= new Dictionary<int,PositionPunchElement>();

        [Serializable]
        public class TranslationSettings
        {
            public Vector3 localPositionTarget = Vector3.up;
            public EasingCurve easeIn;
            [FormerlySerializedAs("duration")] public float durationEaseIn;
            public EasingCurve easeOut;
            [FormerlySerializedAs("duration2")] public float durationEaseOut;
        }

        private class PositionPunchElement
        {
            private Sequence mySequence;
            private Vector3 iniPos;
            private bool init;

            private Transform transform;

            public void Init(Transform t)
            {
                transform = t;
                init = true;
                iniPos = transform.localPosition;
            }

            public void Punch(TranslationSettings settings, float amplitude, MonoBehaviour monoBehaviour, Action onComplete = null)
            {
                if(!transform.gameObject.activeSelf) return;
            
                if(!init) Init(transform);

                if (mySequence != null)
                {
                    mySequence.Stop();
                    transform.localPosition = iniPos;
                }
			
                mySequence = new Sequence();
                mySequence.Append(transform.DoLocalTranslation(iniPos + settings.localPositionTarget * amplitude, 
                    settings.durationEaseIn,monoBehaviour).SetEase(settings.easeIn));
                mySequence.Append(transform.DoLocalTranslation(iniPos, settings.durationEaseOut,monoBehaviour).SetEase(settings.easeOut));
                if(onComplete != null)
                    mySequence.OnComplete(onComplete);
            }

            public void Stop()
            {
                if (mySequence != null)
                {
                    mySequence.Stop();
                }
            }
        }

        [Button(ButtonMode.EnabledInPlayMode)]
        public override void Punch()
        {
            Punch(transform,OnComplete);
        }

        public override void Punch(Transform t, Action action = null)
        {
            if (_positionPunchElements.TryGetValue(t.GetInstanceID(), out PositionPunchElement element))
            {
                element.Punch(translationSettings,amplitude,this,action);
            }
            else
            {
                PositionPunchElement punchElement = new PositionPunchElement();
                _positionPunchElements.Add(t.GetInstanceID(),punchElement);
                punchElement.Init(t);
                punchElement.Punch(translationSettings,amplitude,this,action);
            }
        }

        private void OnDisable()
        {
            foreach (var item in _positionPunchElements)
            {
                item.Value.Stop();
            }
        }

        public override void Stop(Transform t)
        {
            if (_positionPunchElements.TryGetValue(t.GetInstanceID(), out PositionPunchElement element))
            {
                element.Stop();
            }
        }

        public void InvokeAction()
        {
            Punch();
        }
    }
}
