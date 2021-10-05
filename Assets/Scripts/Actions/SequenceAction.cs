using System.Collections;
using CoreInterfaces;
using TweenSystem;
using UnityEngine;

namespace GameFramework
{
    public class SequenceAction : MonoBehaviour, IActionMonoBehaviour, IStopActionMonoBehaviour
    {
        public bool useInPause;
        public float delayToBegin;
        public AnimValue animValue;
        public MonoBehaviour[] startActions;
        public MonoBehaviour[] nextActions;
        public GameObject[] activeBegin;
        public GameObject[] disableEnd;

        public void InvokeAction()
        {
            StartCoroutine(Routine());
        }

        private IEnumerator Routine()
        {
            yield return WaitDelay();
        
            gameObject.SetActive(true);

            foreach (var action in startActions.GetActionsOfType<IActionMonoBehaviour>())
            {
                action.InvokeAction();
            }
        
            foreach (var gameObj in activeBegin)
            {
                gameObj.SetActive(true);
            }

            if (animValue)
            {
                animValue.events.OnEnd.AddListener(StopAction);
                animValue.Play();
            }
            else
            {
                StopAction();
            }
        }
    
        private IEnumerator WaitDelay()
        {
            if (useInPause)
            {
                yield return new WaitForSecondsRealtime(delayToBegin);
            }
            else
            {
                yield return new WaitForSeconds(delayToBegin);
            }
        }

        public void StopAction() {
            if (animValue)
                animValue.events.OnEnd.RemoveListener(StopAction);
            foreach (var gameObj in disableEnd)
            {
                gameObj.SetActive(false);
            }
        
            foreach (var action in nextActions.GetActionsOfType<IActionMonoBehaviour>())
            {
                action.InvokeAction();
            }
        }

    
    }
}
