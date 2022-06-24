using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UtilsEditor{


// written by luna olmewe.
// feel free to use it as you wish!

    public abstract class StringRefSource
    {
        /// <summary>creates list of possible values for this context</summary>
        public abstract void Populate(StringRefList list);
    
        /// <summary>returns how a value is compared against others.
        /// for example, if you don't care about case, you can return value.ToLowerInvariant() here.
        /// or, if you don't care about surrounding spaces, value.Trim(), etc</summary>
        public virtual string GetComparisonValue(string value) => value;
        /// <summary>returns name used to display null or whitespace values</summary>
        public virtual string GetEmptyName() => "(none)";
        /// <summary>returns name used to display missing values</summary>
        public virtual string GetMissingName(string value) => value + " (missing)";

        public sealed class Empty : StringRefSource
        {
            public override void Populate(StringRefList list) { }
        }
#if UNITY_EDITOR
        public virtual void OverrideStartPosition(ref Rect position){
            
        }

        public virtual void OnGUI(ref Rect position, SerializedProperty property, GUIContent label){
            
        }

        public virtual float PropertyHeightMultiplier(){
            return 1;
        }
#endif
    }

    public struct StringRefList
    {
        readonly StringRefSource source;
        readonly List<Entry> entries;

        public struct Entry
        {
            public string value, name;
        }

        public StringRefList(StringRefSource source, List<Entry> entries)
        {
            this.source = source;
            this.entries = entries;
        }

        public void Add(string value, string name = null)
        {
            entries.Add(CreateItem(value, name, false));
        }

        public void AddEmpty()
        {
            entries.Add(CreateItem(null, null, false));
        }

        public void AddSeparator()
        {
            entries.Add(CreateSeparator());
        }

        public void AddMissingOnTop(string value)
        {
            entries.Insert(0, CreateSeparator());
            entries.Insert(0, CreateItem(value, null, true));
        }

        public string GetNameAt(int index)
        {
            if (entries != null && entries.Count > index)
            {
                return entries[index].name;
            }

            return null;
        }

        Entry CreateItem(string value, string name, bool missing)
        {
            if (value == null) value = "";
            if (name == null) name = value;
            if (string.IsNullOrWhiteSpace(name)) name = source.GetEmptyName();
            if (missing) name = source.GetMissingName(name);
            return new Entry { value = value, name = name };
        }

        Entry CreateSeparator() => new Entry { value = null, name = "" };
    }

    public class StringRefAttribute : PropertyAttribute
    {
#if UNITY_EDITOR
        public StringRefSource source;

        public StringRefAttribute(System.Type source)
        {
            this.source = null;

            if (source.IsSubclassOf(typeof(StringRefSource)))
            {
                this.source = System.Activator.CreateInstance(source) as StringRefSource;
            }

            if (this.source == null)
            {
                Debug.LogWarning($"the attribute [StringRef(typeof({source.Name}))] can't be used as {source.Name} doesn't inherit StringRefSource.");
            }
        }
#else
    public StringRefAttribute(System.Type source) { }
#endif
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(StringRefAttribute))]
    public class StringRefDrawer : PropertyDrawer{
        private StringRefSource _source;
        private readonly Dictionary<string, Popup> _popups = new Dictionary<string, Popup>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
            return base.GetPropertyHeight(property, label) * (_source?.PropertyHeightMultiplier() ?? 1);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String) return;

            EditorGUI.BeginProperty(position, label, property);
            
            var key = property.propertyPath;
            var value = property.stringValue ?? "";
            _source = (attribute as StringRefAttribute)?.source ?? new StringRefSource.Empty();
            _source.OverrideStartPosition(ref position);
            if (!_popups.TryGetValue(key, out var popup) || popup.value != value){
                popup = GetValues(_source, value);
                _popups[key] = popup;
            }

            EditorGUI.BeginChangeCheck();

            int index = EditorGUI.Popup(position, new GUIContent(property.displayName, property.tooltip), popup.index, popup.names);

            if (EditorGUI.EndChangeCheck()){
                if (popup.values[index] != null) popup.index = index;
                property.stringValue = popup.value = popup.values[popup.index];
            }
            
            _source.OnGUI(ref position,property,label);
            
            EditorGUI.EndProperty();
        }

        class Popup
        {
            public int index;
            public string value;
            public string[] values;
            public GUIContent[] names;
        }

        static Popup GetValues(StringRefSource source, string currentValue)
        {
            int index;
            var entries = new List<StringRefList.Entry>();

            var list = new StringRefList(source, entries);
            source.Populate(list);

            if (entries.Count > 0)
            {
                index = -1;

                var id = source.GetComparisonValue(currentValue);
                for (int a = 0; a < entries.Count; a++)
                {
                    if (entries[a].value == null) continue;

                    var listId = source.GetComparisonValue(entries[a].value);
                    if (listId == id)
                    {
                        index = a;
                        break;
                    }
                }

                if (index < 0)
                {
                    index = 0;
                    list.AddMissingOnTop(currentValue);
                }
            }
            else
            {
                index = 0;
                list.Add(currentValue);
            }

            return new Popup
            {
                index = index,
                value = currentValue,
                values = entries.Select(x => x.value).ToArray(),
                names = entries.Select(x => new GUIContent(x.name, x.value)).ToArray(),
            };
        }
    }
#endif
}