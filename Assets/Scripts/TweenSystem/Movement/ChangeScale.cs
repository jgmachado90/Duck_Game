using System;
using System.Collections;
using UnityEngine;

namespace TweenSystem
{
    public class ChangeScale : MonoBehaviour
    {
        public bool useInitialScale;
        public float time=1;
        public Vector3 scale1, scale2;
        public event Action EndScale;

        private void OnEnable() {

            if(!useInitialScale){
                transform.localScale = scale1;
            }else{
                scale1 = transform.localScale;
            }
            StartCoroutine(Scale());
        }

        private void OnDisable() {
            transform.localScale = scale1;
        }

        IEnumerator Scale(){
            float value=0;
            transform.localScale = Vector3.Lerp(scale1,scale2,value);

            while(value < 1){
                yield return null;
                value += Time.deltaTime/time;
                transform.localScale = Vector3.Lerp(scale1,scale2,value);
            }

            EndScale?.Invoke();
        }
    }
}
