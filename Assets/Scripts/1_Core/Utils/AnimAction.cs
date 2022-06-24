using System;
using UnityEngine;
using UtilsEditor;

namespace Utils{
    [Serializable]
    public class AnimAction{
        [SerializeField] private ParamType paramType = ParamType.Bollean;
        [SerializeField] private string paramName;
        [ConditionalField(nameof(paramType),false,ParamType.Bollean),SerializeField] private bool boolValue;

        public enum ParamType{
            Bollean,
            Trigger,
            PlayAnim,
        }
        
        public void Play(Animator animator){
            if(string.IsNullOrEmpty(paramName)) return;
            switch (paramType){
                case ParamType.Bollean:
                    animator.SetBool(paramName,boolValue);
                    break;
                case ParamType.Trigger:
                    animator.SetTrigger(paramName);
                    break;
                case ParamType.PlayAnim:
                    animator.Play(paramName);
                    break;
                default:
                    animator.SetBool(paramName,boolValue);
                    break;
            }
        }
    }
}