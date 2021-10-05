using UnityEditor;
using UnityEngine;

namespace TweenSystem
{
    [CustomEditor(typeof(AnimValue))]
    public class AnimValueEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            if(Application.isPlaying && GUILayout.Button("Play")){
                AnimValue v = (AnimValue)target;
                v.ResetAndPlay();
            }
            base.OnInspectorGUI();
        }
    }
}