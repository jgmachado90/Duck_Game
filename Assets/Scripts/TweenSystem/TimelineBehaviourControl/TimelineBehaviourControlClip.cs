using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TweenSystem
{
    [Serializable]
    public class TimelineBehaviourControlClip : PlayableAsset, ITimelineClipAsset
    {
        public TimelineBehaviourControlBehaviour template = new TimelineBehaviourControlBehaviour ();

        public ClipCaps clipCaps
        {
            get { return ClipCaps.Extrapolation | ClipCaps.ClipIn | ClipCaps.SpeedMultiplier; }
        }

        public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TimelineBehaviourControlBehaviour>.Create (graph, template);
            TimelineBehaviourControlBehaviour clone = playable.GetBehaviour ();
            return playable;
        }
    }
}
