using System.Collections;
using CoreInterfaces;
using UnityEngine;

namespace GameFramework
{
    public class FeedbackAction : Entity, IActionMonoBehaviour
    {
        public float delay;
        public Feedback feedback;

        public void InvokeAction() {
            if(delay<=0)
                DoFeedback();
            else {
                StartCoroutine(DelayFeedbackCoroutine());
            }
        }

        private void DoFeedback() {
            feedback.Invoke(this);
        }

        private IEnumerator DelayFeedbackCoroutine()
        {
            yield return new WaitForSecondsRealtime(delay);
            DoFeedback();
        }
    }
}
