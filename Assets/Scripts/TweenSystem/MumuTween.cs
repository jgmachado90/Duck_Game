using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TweenSystem
{
    [Serializable]
    public class Tween
    {
        public EasingCurve ease = new EasingCurve();
        public event Action OnCompleted;

        public delegate IEnumerator TweenPlayDelegate(Tween t);
    
        private TweenPlayDelegate _action;

        public Coroutine Coroutine;
        public MonoBehaviour monobehaviour;

        public Tween(TweenPlayDelegate action, MonoBehaviour monoBehaviour)
        {
            monobehaviour = monoBehaviour;
            _action = action;
        }

        public Tween SetEase(EasingCurve ease)
        {
            this.ease = ease;
            return this;
        }
    
        public Tween OnComplete(Action action)
        {
            OnCompleted += action;
            return this;
        }
    
        public Tween Complete()
        {
            OnCompleted?.Invoke();
            return this;
        }

        public Tween Play()
        {
            Coroutine = monobehaviour.StartCoroutine(_action?.Invoke(this));
            return this;
        }

        public Tween Stop()
        {
            if (Coroutine != null)
                monobehaviour.StopCoroutine(Coroutine);
            return this;
        }
    }

    [Serializable]
    public class Sequence
    {
        public List<Tween> tweens = new List<Tween>();

        public Sequence Append(Tween tween)
        {
            tweens.Add(tween);
            if (tweens.Count > 1)
            {
                tweens[tweens.Count - 2].OnCompleted += () => tween.Play();
            }
            else
            {
                tween.Play();
            }
            return this;
        }

        public Sequence Stop()
        {
            for (int i = 0; i < tweens.Count; i++)
            {
                tweens[i].Stop();
            } 
            tweens.Clear();
            return this;
        }

        public Sequence OnComplete(Action action)
        {
            if (tweens.Count >= 1)
            {
                tweens[tweens.Count - 1].OnCompleted += action;
            }
            return this;
        }
    }
}