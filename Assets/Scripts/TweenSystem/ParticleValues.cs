using System;
using CoreInterfaces;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace TweenSystem
{
    public class ParticleValues : ObjectValue , IActionMonoBehaviour, IStopActionMonoBehaviour {
        
        public ParticleSettings[] particles;
        
        
        [Header("Test in playmode")]
        [Range(0,1)]
        public float testValue = 0;
        
        [Serializable]
        public class ParticleSettings
        {
            public ParticleSystem particle;
            
            public bool changeSpeed;
            public AnimationCurve speedCurve;
            
            public bool changeEmissionRate;
            public AnimationCurve emissionRateCurve;
            
            public bool changeSize;
            public AnimationCurve sizeCurve;
            
            public bool changePlay;
            public AnimationCurve activeCurve;
            
            public bool changeColor;
            public Gradient colorCurve;
            
            public void SetValue(float value)
            {
                ParticleSystem.MainModule main = particle.main;

                if (changePlay)
                {
                    bool active = activeCurve.Evaluate(value) >0;
                    switch (particle.isPlaying)
                    {
                        case true when !active:
                            particle.Stop(false,ParticleSystemStopBehavior.StopEmittingAndClear);
                            break;
                        case false when active:
                            particle.Play();
                            break;
                    }
                    
                    if(!active) return;
                }

                if(changeSpeed)
                    main.simulationSpeed = speedCurve.Evaluate(value);
                
                if (changeEmissionRate)
                {
                    ParticleSystem.EmissionModule emissionModule = particle.emission;
                    emissionModule.rateOverTime =emissionRateCurve.Evaluate(value);
                }

                if (changeSize)
                {
                    main.startSize = sizeCurve.Evaluate(value);
                }

                if (changeColor)
                {
                    ParticleSystem.MinMaxGradient startColor = particle.main.startColor;
                    startColor.color = colorCurve.Evaluate(value);
                    main.startColor = startColor;
                }
            }
        }

        private void OnValidate()
        {
            if(!Application.isPlaying) return;
            SetValue(testValue);
        }

        public override void SetValue(float value)
        {
            if(!Application.isPlaying) return;
            for (var i = 0; i < particles.Length; i++)
            {
                var particle = particles[i];
                particle.SetValue(value);
            }
        }

        // public void SetGravityScale(float value)
        // {
        //     for (var i = 0; i < particle.Length; i++)
        //     {
        //         var t = particle[i];
        //         ParticleSystem.MainModule main = t.particle.main;
        //         main.gravityModifierMultiplier = value;
        //     }
        // }

        public void InvokeAction()
        {
            foreach (var t in particles)
            {
                t.particle.Play(true);
            }
        }

        public void StopAction()
        {
            foreach (var t in particles)
            {
                t.particle.Stop(true);
            }
        }

        // public void SetTime(float time)
        // {
        //     for (var i = 0; i < particle.Length; i++)
        //     {
        //         var t = particle[i];
        //         t.particle.Simulate(time);
        //     }
        // }
    }
}
