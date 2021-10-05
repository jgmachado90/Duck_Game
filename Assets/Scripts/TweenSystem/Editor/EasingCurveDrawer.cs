using UnityEditor;
using UnityEngine;

namespace TweenSystem
{
    [CustomPropertyDrawer(typeof(EasingCurve))]
    public class EasingCurveDrawer: PropertyDrawer {

        private bool ShowCurve(int i)
        {
            return i == (int) easingTypes.custom || i == (int) easingTypes.customCut;
        }

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
            
            int i = property.FindPropertyRelative(nameof(EasingCurve.easingtype)).enumValueIndex;
            return EditorGUIUtility.singleLineHeight * (ShowCurve(i)? 2 : 1);
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        
            EditorGUI.BeginProperty(position, label, property);
            
            

           
            var prop = property.FindPropertyRelative(nameof(EasingCurve.easingtype));
            int i = prop.enumValueIndex;
            bool showCurve = ShowCurve(i);
            
            if(showCurve)
                position.height /=2;
            
            EditorGUI.PropertyField(position,prop, label);
        
            

            if (showCurve)
            {
                var newPos = position;
                newPos.y += 20;
                EditorGUI.PropertyField(newPos,
                    property.FindPropertyRelative(nameof(EasingCurve.curve)),
                    new GUIContent("Animation Curve"));
            }

            EditorGUI.EndProperty();
        }
    }
}
