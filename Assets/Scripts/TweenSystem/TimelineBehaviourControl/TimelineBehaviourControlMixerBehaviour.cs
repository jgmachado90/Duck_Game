using UnityEngine.Playables;

namespace TweenSystem
{
    public class TimelineBehaviourControlMixerBehaviour : PlayableBehaviour
    {
        // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            TimelineBehaviour trackBinding = playerData as TimelineBehaviour;

            if (!trackBinding)
                return;

            int inputCount = playable.GetInputCount ();
            double timelineCurrentTime = (playable.GetGraph().GetResolver() as PlayableDirector).time;

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
            
            
                ScriptPlayable<TimelineBehaviourControlBehaviour> inputPlayable = (ScriptPlayable<TimelineBehaviourControlBehaviour>)playable.GetInput(i);
                TimelineBehaviourControlBehaviour input = inputPlayable.GetBehaviour();
            
                // Use the above variables to process each frame of this playable.
                //Debug.Log(inputPlayable.GetTime ());
                //float normalisedTime = (float)(inputPlayable.GetTime() / inputPlayable.GetDuration ());
                input.MixerProcessFrame(inputPlayable, info, trackBinding, timelineCurrentTime,inputPlayable.GetTime(),inputWeight);
            
            }
        }
    }
}
