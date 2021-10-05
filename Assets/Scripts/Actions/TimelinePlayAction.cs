using System;
using Addons.EditorButtons.Runtime;
using CoreInterfaces;
using UnityEngine;
using UnityEngine.Playables;

namespace GameFramework
{
    public class TimelinePlayAction : Entity, IActionMonoBehaviour
    {
        public bool onEnable;
        public PlayableDirector playableDirector;
        public Feedback feedbackEnd;

        private void OnEnable()
        {
            if (onEnable)
                InvokeAction();
        }

        [Button(ButtonMode.EnabledInPlayMode)]
        public void InvokeAction()
        {
            playableDirector.Play();
            playableDirector.stopped += OnStopped;
        }

        private void OnStopped(PlayableDirector obj)
        {
            feedbackEnd.Invoke(transform,this);
        }
    }
}
