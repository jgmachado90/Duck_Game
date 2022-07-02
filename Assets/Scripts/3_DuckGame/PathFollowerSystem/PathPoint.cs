using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using MoreMountains.Feedbacks;

namespace PathFollow
{
    public class PathPoint : Entity
    {
        [SerializeField] private float animationDuration;
        [SerializeField] private AnimationCurve animCurve;
        [SerializeField] private MMFeedbacks startFeedbacks;
        [SerializeField] private MMFeedbacks reachFeedbacks;
        [SerializeField] private float reachDelay;
        public float AnimationDuration
        {
            get { return animationDuration; }
        }
        
        public AnimationCurve AnimationCurve
        {
            get { return animCurve; }
        }
        
        public float ReachDelay
        {
            get { return reachDelay; }
        }

        public void OnReach()
        {
            if(reachFeedbacks != null)
                reachFeedbacks.PlayFeedbacks();
        }

        public void OnStart()
        {
            if (startFeedbacks != null)
                startFeedbacks.PlayFeedbacks();
        }
        
    }
}
