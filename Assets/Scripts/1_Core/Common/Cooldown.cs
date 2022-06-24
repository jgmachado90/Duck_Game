using UnityEngine;

namespace Common
{
    [System.Serializable]
    public class Cooldown{
    
        public float coolDownTime = 0f;
    
        [System.NonSerialized]
        private float _initializedTime = -Mathf.Infinity;

        private float? _iniCoolDownTime;
        private bool _locked;

        public Cooldown(float delay) {
            coolDownTime = delay;
        }

        public Cooldown() {
        }

        public virtual void Init()
        {
            _locked = false;
            _initializedTime = Time.time;
        }

        public virtual bool Finished(){
            if (_locked) return false;
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

        public void Lock(){
            _locked = true;
        }
    }

    [System.Serializable]
    public class CooldownRanged : Cooldown
    {
        [Min(0)] public float minDelay;
        [Min(0)] public float maxDelay;
        
        public CooldownRanged(float minDelay, float maxDelay)
        {
            coolDownTime = 0;
            this.minDelay = minDelay;
            this.maxDelay = maxDelay;
        }

        public override void Init()
        {
            coolDownTime = Random.Range(minDelay, maxDelay);
            base.Init();
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