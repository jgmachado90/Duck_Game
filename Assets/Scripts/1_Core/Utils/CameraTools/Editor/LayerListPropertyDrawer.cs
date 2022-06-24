#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Utils
{
    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public class LayerListPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.Popup(position, property.intValue, LayerAttribute.GetLayerList().ToArray());
        }
    }
}
#endif

