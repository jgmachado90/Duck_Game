using System.Collections;
using UnityEngine;

namespace TweenSystem
{
    public static class TweenExtensions
    {
        public delegate T Getter<out T>();

        public delegate void Setter<in T>(T pNewValue);
    
        public static IEnumerator To(Getter<float> getter,Setter<float> setter, float endValue, float duration)
        {
            float f = getter.Invoke();
        
            while(f != endValue){
                yield return null; 
                f = Mathf.MoveTowards(f,endValue,Time.deltaTime/duration);
                setter.Invoke(f);
            }
        }
    
        public static IEnumerator To(Getter<Vector3> getter,Setter<Vector3> setter, Vector3 endValue, float duration)
        {
            Vector3 f = getter.Invoke();
            while(f != endValue){
                yield return null; 
                f = Vector3.MoveTowards(f,endValue,Time.deltaTime/duration);
                setter.Invoke(f);
            }
        }

        public static Tween DoScale(this Transform t, Vector3 target, float duration, MonoBehaviour monoBehaviour)
        {
            Tween tween = new Tween((o) => DoScale(t, target, duration, o ),monoBehaviour);
            return tween;
        }
        
        public static Tween DoLocalTranslation(this Transform t, Vector3 target, float duration, MonoBehaviour monoBehaviour)
        {
            Tween tween = new Tween((o) => DoLocalTranslation(t, target, duration, o ),monoBehaviour);
            return tween;
        }
        
        public static Tween DoLocalTranslationSpherical(this Transform t, Vector3 target, float duration, MonoBehaviour monoBehaviour)
        {
            Tween tween = new Tween((o) => DoLocalTranslationSpherical(t, target, duration, o ),monoBehaviour);
            return tween;
        }

        private static IEnumerator DoScale(Transform t,Vector3 target, float duration,Tween tween)
        {
            Vector3 ini = t.localScale;
            float time = 0;
        
            while(time <= 1){
                yield return null;
                t.localScale = Vector3.LerpUnclamped(ini, target,  tween.ease.Evaluate(time));
                time += Time.deltaTime * 1 / duration;
            }
        
            tween.Complete();
        }
        
        private static IEnumerator DoLocalTranslation(Transform t,Vector3 target, float duration,Tween tween)
        {
            Vector3 ini = t.localPosition;
            float time = 0;
        
            while(time <= 1){
                yield return null;
                t.localPosition = Vector3.LerpUnclamped(ini, target,  tween.ease.Evaluate(time));
                time += Time.deltaTime * 1 / duration;
            }
        
            tween.Complete();
        }
        
        private static IEnumerator DoLocalTranslationSpherical(Transform t,Vector3 target, float duration,Tween tween)
        {
            Vector3 ini = t.localPosition;
            float time = 0;
        
            while(time <= 1){
                yield return null;
                t.localPosition = Vector3.SlerpUnclamped(ini, target,  tween.ease.Evaluate(time));
                time += Time.deltaTime * 1 / duration;
            }
        
            tween.Complete();
        }
    }
}
