using UnityEngine;
using UnityEngine.Events;

namespace TweenSystem
{
    public class TimelineBehaviour : MonoBehaviour
    {
        [TextArea(2,5)]
        public string description;

        public UnityEvent onStart;
        public UnityEvent onStop;
        public FloatEvent onProgress;
        public FloatEvent onTime;

        [System.Serializable]
        public class FloatEvent:  UnityEvent<float>{} 

        public void TimeStart()
        {
            onStart.Invoke();
        }

        public void TimeStop()
        {
            onStop.Invoke();
        }

        public void SetProggress(float progress){
            onProgress.Invoke(progress);
        }

        public void SetTime(float time)
        {
            onTime.Invoke(time);
        }
    }
}
