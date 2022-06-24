using System;
using System.Collections.Generic;
using UnityEngine;

namespace UtilsEditor
{
    public abstract class GuidFlags<T> : ScriptableObject where T : GuidRef
    {
        public List<T> flags;
        
        private string[] _flagIDs;
        public string[] FlagIDs => _flagIDs != null && _flagIDs.Length > 0 ? _flagIDs : CacheIDs();

        public bool TryGetIndexOf(string flagId, out int index)
        {
            for (int i = 0; i < FlagIDs.Length; i++)
            {
                if (FlagIDs[i] != flagId) continue;
                index = i;
                return true;
            }

            index = -1;
            return false;
        }
        
        public int GetIndexOf(string flagId)
        {
            for (int i = 0; i < FlagIDs.Length; i++)
            {
                if (FlagIDs[i] == flagId) return i;
            }

            return -1;
        }
        
        public string GetGuid(int index)
        {
            if (index < 0 || index >= flags.Count) return null;
            return FlagIDs[index];
        }
        
        private string[] CacheIDs()
        {
            _flagIDs = new string[flags.Count];
            for (int i = 0; i < flags.Count; i++)
            {
                _flagIDs[i] = flags[i].Guid.ToString();
            }

            return _flagIDs;
        }

        public static string GetNameFromGUId(string resourcesPath, string guidValue){
            var flagsFiles = Resources.LoadAll(resourcesPath);
            foreach (var flagsFile in flagsFiles)
            {
                if (!(flagsFile is GuidFlags<T> guidFlagsFile)) continue;
                foreach (var flag in guidFlagsFile.flags)
                {
                    if (flag.Guid.ToString() != guidValue) continue;
                    return flag.name;
                }
            }
            return null;
        }
        
        public static T2 GetOwnerAsset<T2>(string guid, T2[] assets, out T flag) where T2 : GuidFlags<T>
        {
            flag = null;
            
            if (assets == null || string.IsNullOrWhiteSpace(guid)) return null;
            
            foreach (var asset in assets)
            {
                foreach (var assetFlag in asset.flags)
                {
                    if (assetFlag.Guid.ToString() != guid) continue;
                    
                    flag = assetFlag;
                    return asset;
                }
            }

            return null;
        }
        
        protected abstract string ResourcesPath { get; }
        
        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            _flagIDs = null;
            UnityEditor.EditorApplication.delayCall += CheckAndFixDuplicates;
#endif
        }
        
#if UNITY_EDITOR
        private void CheckAndFixDuplicates()
        {
            var flagsAssets = Resources.LoadAll<GuidFlags<T>>(ResourcesPath);
            var guids = new List<Guid>();
            
            foreach (var asset in flagsAssets)
            {
                if (asset.flags == null) continue;
                foreach (var flag in asset.flags)
                {
                    if (guids.Contains(flag.Guid))
                    {
                        Debug.LogWarning($"Duplicated Guid detected ({flag.name})");
                        flag.MakeNewGuid();
                    }
                    guids.Add(flag.Guid);
                }
            }
        }
#endif
    }
    
    [CreateAssetMenu(fileName = "Flags")]
    public class GuidFlags : GuidFlags<GuidRef>
    {
        public const string FlagsResourcesPath = "Flags";
        protected override string ResourcesPath => FlagsResourcesPath;
    }
    
    [Serializable]
    public class GuidRef : ISerializationCallbackReceiver
    {
        [Delayed] public string name;

        [SerializeField, HideInInspector]
        protected byte[] serializedGuid;

        private Guid _guid;

        public GuidRef(string name, Guid guid)
        {
            this.name = name;
            _guid = guid;
            serializedGuid = _guid.ToByteArray();
        }

        public GuidRef(string guid){
            _guid = new Guid(guid);
            serializedGuid = _guid.ToByteArray();
        }

        public Guid Guid
        {
            get
            {
                if (_guid != Guid.Empty) return _guid;

                DeserializeGuid();
                return _guid;
            }
        }

        private void SerializeGuid()
        {
            serializedGuid = _guid.ToByteArray();
        }

        private void DeserializeGuid()
        {
            if (serializedGuid?.Length > 0)
            {
                _guid = new Guid(serializedGuid);
            }
            else
            {
                MakeNewGuid();
            }
        }

        internal void MakeNewGuid()
        {
            _guid = Guid.NewGuid();
            SerializeGuid();
        }

        public void OnBeforeSerialize() => SerializeGuid();
        public void OnAfterDeserialize() => DeserializeGuid();
    }
    
    public class SaveEntitiesGuidRefSource : GuidRefSource{
        public const string SaveEntitiesPath = "Flags/SaveEntities/";
        
        protected override string OverridePath => SaveEntitiesPath;
    }
        
    public class SavePropertiesGuidRefSource : GuidRefSource{
        public const string SavePropertiesPath = "Flags/SaveProperties/";
        
        protected override string OverridePath => SavePropertiesPath;
    }
    
    public class GuidRefSource : StringRefSource
    {
        private const string Path = "Flags/";

        protected virtual string OverridePath => Path;
        
        public override void Populate(StringRefList list)
        {
#if UNITY_EDITOR
            list.AddEmpty();
            list.AddSeparator();
        
            var flagsAssets = Resources.LoadAll<GuidFlags>(OverridePath);

            foreach (var asset in flagsAssets)
            {
                if (asset.flags == null) continue;
                var filePrefix = asset.name + "/";
            
                foreach (var flag in asset.flags)
                {
                    list.Add(flag.Guid.ToString(), filePrefix + flag.name);
                }
            }
#endif
        }
    }
}