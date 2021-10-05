using UnityEngine;

namespace TweenSystem
{
    public class AnimatorSpeedValue : ObjectValue
    {
        public Animator animator;
        public AnimationCurve animationCurve = AnimationCurve.Linear(0,0,1,1);

        public override void SetValue(float value)
        {
            if(!Application.isPlaying) return;

            animator.speed = animationCurve.Evaluate(value);
        }
    }
}
