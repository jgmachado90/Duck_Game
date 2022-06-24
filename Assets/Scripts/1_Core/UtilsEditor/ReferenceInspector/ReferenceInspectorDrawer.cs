#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
using UnityEngine;

namespace UtilsEditor{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ReferenceInspector))]
    public class ReferenceInspectorDrawer : PropertyDrawer{
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label){
            if (property.propertyType != SerializedPropertyType.ObjectReference) return;
           
           EditorGUI.PropertyField(position, property,new GUIContent(property.displayName));
           var e = Editor.CreateEditor(property.objectReferenceValue);
           if(e == null) return;
           EditorGUILayout.Space();
           DrawLine(property);
           Editor.DrawFoldoutInspector(property.objectReferenceValue,ref e);
           DrawLine(property);
        }

        private static void DrawLine(SerializedProperty property){
            bool inspectorExpanded = InternalEditorUtility.GetIsInspectorExpanded(property.objectReferenceValue);
            if (inspectorExpanded){
                DrawLine();
            }
        }

        private static void DrawLine(){
            var rect = EditorGUILayout.BeginHorizontal();
            Handles.color = Color.gray;
            Handles.DrawLine(new Vector2(rect.x - 15, rect.y), new Vector2(rect.width + 15, rect.y));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
            return base.GetPropertyHeight(property, label);
            
        }
    }
#endif
    public class ReferenceInspector : PropertyAttribute
    {
        public ReferenceInspector()
        {
            
        }
    }

}