using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class ObservedFloat
    {
        [SerializeField]
        private float observedValue;
        public event Action<float> OnValueChanged;

        public float ObservedValue{
            get{
                return observedValue;
            }set{
                if(!observedValue.Equals(value))
                    OnValueChanged?.Invoke(value);
                observedValue = value;
            }
        }

        public void SilentSet(float value){
            observedValue = value;
        }

        public static implicit operator float(ObservedFloat observedFloat){
            return observedFloat.ObservedValue;
        }
    
        public static ObservedFloat operator +(ObservedFloat o1,int o2)
        {
            o1.ObservedValue += o2;
            return o1;
        }
    
        public static ObservedFloat operator -(ObservedFloat o1,int o2)
        {
            o1.ObservedValue -= o2;
            return o1;
        }
    
        public static ObservedFloat operator --(ObservedFloat o1)
        {
            o1.ObservedValue -= 1;
            return o1;
        }
    
        public static ObservedFloat operator ++(ObservedFloat o1)
        {
            o1.ObservedValue += 1;
            return o1;
        }
    }

    [Serializable]
    public class ObservedInt
    {
        [SerializeField]
        private int observedValue;
        public event Action<int> OnValueChanged;

        public int ObservedValue{
            get{
                return observedValue;
            }set{
                if(!observedValue.Equals(value))
                    OnValueChanged?.Invoke(value);
                observedValue = value;
            }
        }

        public void SilentSet(int value){
            observedValue = value;
        }

        public static implicit operator int(ObservedInt observedInt)
        {
            return observedInt.ObservedValue;
        }

        public static ObservedInt operator +(ObservedInt o1,int o2)
        {
            o1.ObservedValue += o2;
            return o1;
        }

        public static ObservedInt operator -(ObservedInt o1,int o2)
        {
            o1.ObservedValue -= o2;
            return o1;
        }
    
        public static ObservedInt operator --(ObservedInt o1)
        {
            o1.ObservedValue -= 1;
            return o1;
        }
    
        public static ObservedInt operator ++(ObservedInt o1)
        {
            o1.ObservedValue += 1;
            return o1;
        }
    }
}