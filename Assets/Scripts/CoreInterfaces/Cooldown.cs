using UnityEngine;

namespace CoreInterfaces
{
    [System.Serializable]
    public class Cooldown{
    
        public float coolDownTime = 0f;
    
        [System.NonSerialized]
        private float _initializedTime = -Mathf.Infinity;

        private float? _iniCoolDownTime;

        public Cooldown(float delay) {
            coolDownTime = delay;
        }

        public Cooldown() {
        }

        public virtual void Init()
        {
            _initializedTime = Time.time;
        }

        public virtual bool Finished()
        {
            return Time.time - _initializedTime > coolDownTime;
        }

        public float GetTime()
        {
            return Time.time - _initializedTime;
        }
        
        public float GetTimeToFinish()
        {
            return coolDownTime - (Time.time - _initializedTime);
        }

        public virtual void AddTime(float time)
        {
            coolDownTime += time;
        }
    }

    [System.Serializable]
    public class CooldownUnscaled{
    
        public float coolDownTime = 0.1f;
    
        [System.NonSerialized]
        private float _initializedTime;

        public void Init()
        {
            _initializedTime = Time.unscaledTime;
        }

        public bool TimeFinished()
        {
            return Time.unscaledTime - _initializedTime > coolDownTime;
        }

        public float GetTime()
        {
            return Time.unscaledTime - _initializedTime;
        }
    }
}