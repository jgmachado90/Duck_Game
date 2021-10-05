using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TweenSystem
{
    [TrackColor(0.855f, 0.8623f, 0.87f)]
    [TrackClipType(typeof(TimelineBehaviourControlClip))]
    [TrackBindingType(typeof(TimelineBehaviour))]
    public class TimelineBehaviourControlTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach(var clip in m_Clips)
            {
            
                var playableAsset = clip.asset as TimelineBehaviourControlClip;
                playableAsset.template.clip = clip;
                playableAsset.template.clipStart = clip.start;
                playableAsset.template.clipDuration = clip.duration;
            }
            return ScriptPlayable<TimelineBehaviourControlMixerBehaviour>.Create (graph, inputCount);
        }
    }
}
