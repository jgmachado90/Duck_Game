using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TweenSystem
{
    [Serializable]
    public class TimelineBehaviourControlBehaviour : PlayableBehaviour
    {
        public float min;
        public float max =1;
        public AnimationCurve curve = AnimationCurve.Linear(0,0,1,1);

        [HideInInspector] public double clipStart;
        [HideInInspector] public double clipDuration;

        private TimelineBehaviour timelineBehaviour;

        [HideInInspector] public TimelineClip clip;

        public void MixerProcessFrame(Playable thisPlayable, FrameData info, TimelineBehaviour timelineBehaviour, 
            double timelineCurrentTime, double time, float weight)
        {
            this.timelineBehaviour = timelineBehaviour;
            float globaltime = (float)(timelineCurrentTime - clipStart);
            if(globaltime >=0 || clip.hasPreExtrapolation){
                float normalisedTime = (float)time/(float) clipDuration;
                timelineBehaviour.SetProggress(Mathf.Lerp(min,max, curve.Evaluate(normalisedTime)));
                timelineBehaviour.SetTime(globaltime);
            }
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            timelineBehaviour?.TimeStart();
        }
    
        public override void OnGraphStop(Playable playable)
        {
            timelineBehaviour?.TimeStop();
        }
    }
}
