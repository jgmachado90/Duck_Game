namespace Common
{
    [System.Serializable]
    public class CooldownBySpeed
    {
        public float unitPerSecond;
        public float unitPerSecondMultiplier=1;
        private float? _iniChargeSpeed;
        Cooldown cooldown = new Cooldown();

        public void Init() {
            SetCooldownTime();
            cooldown.Init();
        }

        public bool Finished() {
            SetCooldownTime();
            return cooldown.Finished();
        }

        void SetCooldownTime() {
            cooldown.coolDownTime = GetCooldownTime();
        }

        public float GetCooldownTime() => (unitPerSecond > 0 && unitPerSecondMultiplier > 0) ? 1 / (unitPerSecond * unitPerSecondMultiplier): 0f;

        public void AddSpeed(float value) {
            unitPerSecond += value;
            SetCooldownTime();
        }

        public void ResetSpeed() {
            if (_iniChargeSpeed != null) unitPerSecond = _iniChargeSpeed.Value;
            SetCooldownTime();
        }
    }
}