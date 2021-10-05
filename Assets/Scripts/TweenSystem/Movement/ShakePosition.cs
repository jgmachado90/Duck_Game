using System.Collections;
using Addons.EditorButtons.Runtime;
using CoreInterfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TweenSystem
{
    public class ShakePosition : MonoBehaviour, IActionMonoBehaviour
    {
        public float time = 0.2f;
        public float distance = 0.1f;
        public float delayBetweenShakes = 0f;
    
        private Vector3 _startPos;
        private float _timer;
        private Vector3 _randomPos;
        private Coroutine _c;
 
        private void Awake()
        {
            _startPos = transform.localPosition;
        }
 
        private void OnValidate()
        {
            if (delayBetweenShakes > time)
                delayBetweenShakes = time;
        }

        [Button(ButtonMode.EnabledInPlayMode)]
        public void InvokeAction()
        {
            StopAnim();
        
            if(isActiveAndEnabled)
                _c = StartCoroutine(Shake());
        }

        private void StopAnim()
        {
            if (_c != null)
            {
                StopCoroutine(_c);
                _c = null;
            }
        }

        private void OnDisable()
        {
            StopAnim();
        }

        private IEnumerator Shake()
        {
            _timer = 0f;
 
            while (_timer < time)
            {
                _timer += Time.deltaTime;
 
                _randomPos = _startPos + (Random.insideUnitSphere * distance);
 
                transform.localPosition = _randomPos;
 
                if (delayBetweenShakes > 0f)
                {
                    yield return new WaitForSeconds(delayBetweenShakes);
                }
                else
                {
                    yield return null;
                }
            }
 
            transform.localPosition = _startPos;
        }
    }
}
