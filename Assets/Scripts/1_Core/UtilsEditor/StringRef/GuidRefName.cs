using System.Linq;
using UnityEngine;

namespace UtilsEditor
{
    public class GuidRefNameAttribute : PropertyAttribute
    {
#if UNITY_EDITOR
        public readonly string guidField;
        public readonly string resourcesPath;
        public readonly bool draw;
#endif

        public GuidRefNameAttribute(string guidField = null, bool draw = true, string resourcesPath = GuidFlags.FlagsResourcesPath)
        {
#if UNITY_EDITOR
            this.guidField = guidField;
            this.resourcesPath = resourcesPath;
            this.draw = draw;
#endif
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomPropertyDrawer(typeof(GuidRefNameAttribute), true)]
    public class GuidRefNameDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            bool found = false;
            var nameAttribute = attribute as GuidRefNameAttribute;
            if (!string.IsNullOrWhiteSpace(nameAttribute?.guidField))
            {
                
                UnityEditor.SerializedProperty guidProperty;
                if (property.depth > 0)
                {
                    var parentName = property.propertyPath.Replace($".{property.name}", "");
                    guidProperty = property.serializedObject.FindProperty(parentName)
                        .FindPropertyRelative(nameAttribute.guidField);
                }
                else
                {
                    guidProperty = property.serializedObject.FindProperty(nameAttribute.guidField);
                }
                
                string guidValue = string.Empty;
                if (guidProperty != null)
                {
                    guidValue = guidProperty.propertyType switch
                    {
                        UnityEditor.SerializedPropertyType.String => guidProperty.stringValue,
                        _ => null
                    };
                }

                if (!string.IsNullOrWhiteSpace(guidValue))
                {
                    var flagsFiles = Resources.LoadAll(nameAttribute.resourcesPath);
                    foreach (var flagsFile in flagsFiles)
                    {
                        if (!(flagsFile is GuidFlags guidFlagsFile)) continue;
                        foreach (var flag in guidFlagsFile.flags)
                        {
                            if (flag.Guid.ToString() != guidValue) continue;
                            found = true;
                            property.stringValue = flag.name;
                            property.serializedObject.ApplyModifiedProperties();
                            UnityEditor.EditorUtility.SetDirty(property.serializedObject.targetObject);
                            break;
                        }
                    }
                }
            }
            
            if (!found)
            {
                property.stringValue = string.Empty;
                property.serializedObject.ApplyModifiedProperties();
            }

            if (nameAttribute is{draw: true}){
                GUI.enabled = false;
                UnityEditor.EditorGUI.PropertyField(position, property, label, true);
                GUI.enabled = true;
            }
        }

        public override float GetPropertyHeight(UnityEditor.SerializedProperty property, GUIContent label)
        {
            if (attribute is GuidRefNameAttribute{draw: true}){
                return UnityEditor.EditorGUI.GetPropertyHeight(property, label, true);
            }
            return 0;
        }
    }
#endif
}