#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Utils
{
    [CustomEditor(typeof(CameraCullDistances))]
    public class CameraCullDistanceEditor : UnityEditor.Editor
    {
        private SerializedProperty m_shortcutData;
        private ReorderableList m_ReorderableList;

        private CameraCullDistances _cameraCullDistances;

        private void OnEnable()
        {
            //Find the list in our ScriptableObject script.
        
            _cameraCullDistances = (CameraCullDistances)target;
            m_shortcutData = serializedObject.FindProperty("layers");

            //Create an instance of our reorderable list.
            m_ReorderableList = new ReorderableList(serializedObject: serializedObject, elements: m_shortcutData, draggable: true, displayHeader: true,
                displayAddButton: true, displayRemoveButton: true);

            //Set up the method callback to draw our list header
            m_ReorderableList.drawHeaderCallback = DrawHeaderCallback;

            //Set up the method callback to draw each element in our reorderable list
            m_ReorderableList.drawElementCallback = DrawElementCallback;

            //Set the height of each element.
            m_ReorderableList.elementHeightCallback += ElementHeightCallback;

            //Set up the method callback to define what should happen when we add a new object to our list.
            //m_ReorderableList.onAddCallback += OnAddCallback;
        }
    
        public override void OnInspectorGUI()
        {
            GUILayout.Space(20);

            serializedObject.Update();

            m_ReorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    
        private void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, "Layers Cull Distances");
        }
    
        private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
        {
            var element = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            var layer = element.FindPropertyRelative("layer");
            var elementTitle = string.IsNullOrEmpty(LayerMask.LayerToName(layer.intValue))
                ? "Unknown Layer"
                : $"{index}: {LayerMask.LayerToName(layer.intValue)}";

            EditorGUI.PropertyField(position:
                new Rect(rect.x += 10, rect.y,  rect.width * 0.95f, height: EditorGUIUtility.singleLineHeight), property:
                element, label: new GUIContent(elementTitle), includeChildren: true);

        }

        private float ElementHeightCallback(int index)
        {
            float propertyHeight = EditorGUI.GetPropertyHeight(m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index), true);

            float spacing = EditorGUIUtility.singleLineHeight / 2;

            return propertyHeight + spacing;
        }
    }
}
#endif