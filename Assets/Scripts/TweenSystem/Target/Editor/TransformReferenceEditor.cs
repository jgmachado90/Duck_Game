using UnityEditor;
using UnityEngine;

namespace TweenSystem.Editor
{
    [CustomPropertyDrawer(typeof(TransformReference))]
    public class TransformReferenceDrawer: PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int oldindentLevel = EditorGUI.indentLevel;
            GUI.Label(position, label);
            EditorGUI.indentLevel++;
            EditorGUI.BeginProperty(position, label, property);

            
            //if(target.objectReferenceValue == null)
            EditorGUILayout.PropertyField(property.FindPropertyRelative("transformReference"), new GUIContent("Reference"));
        
            int i = property.FindPropertyRelative("transformReference").enumValueIndex;
        
            if(i == (int)TransformReferenceType.Custom)
                EditorGUILayout.PropertyField(property.FindPropertyRelative("custom"), new GUIContent("Key"));
            else if(i == (int)TransformReferenceType.Target) {
                var target = property.FindPropertyRelative("target");
                EditorGUILayout.PropertyField(target, new GUIContent("Target"));
            }

            EditorGUI.EndProperty();
            EditorGUI.indentLevel = oldindentLevel;
        }
    }
}