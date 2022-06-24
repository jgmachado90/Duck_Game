using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UtilsEditor
{
#if UNITY_EDITOR
    public static class EditorUtils
    {
        public static T[] GetAllInstances<T>(string[] folders) where T : Object
        {
            string[] guids = GetAllGUIDs<T>(folders);
            T[] a = new T[guids.Length];
        
            for(int i =0;i<guids.Length;i++)
            {
                a[i] = GetAssetByGUID<T>(guids[i]);
            }
            return a;
        }
        
        public static string[] GetAllGUIDs<T>(string[] folders) where T : Object
        {
            if (EditorApplication.isUpdating) return new string[0];
            return AssetDatabase.FindAssets("t:"+ typeof(T).Name,folders);
        }
        
        public static T GetAssetByGUID<T>(string guid) where T : Object
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        public static T GetAssetAtPath<T>(string path) where  T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
        
        private static EditorWindow GetProjectWindow()
        {
            EditorWindow[] windows = (EditorWindow[])Resources.FindObjectsOfTypeAll(typeof(EditorWindow));
            EditorWindow projectWindow = null;

            foreach (EditorWindow window in windows)
            {
                if (window.GetType().ToString() == "UnityEditor.ProjectBrowser")
                {
                    projectWindow = window;
                    break;
                }
            }

            return projectWindow;
        }
        
        public static void SetProjectSearch(string filter)
        {
            EditorWindow projectWindow = GetProjectWindow();

            if (projectWindow == null)
                return;

            MethodInfo setSearchType = projectWindow.GetType().GetMethod("SetSearch", new[] { typeof(string) });
        
            setSearchType.Invoke(projectWindow, new object[] { filter });
        }
        
        public static T GetAsset<T>(string assetName = null) where T: Object{
            var filter = typeof(T).Name + (string.IsNullOrEmpty(assetName) ? "" : $" {assetName}");
            return AssetDatabase.LoadAssetAtPath<T>(GetPath(filter));
        }
        
        public static string GetPath(string filter){
            var findAssets = AssetDatabase.FindAssets("t:"+filter);
            if (findAssets.Length <= 0) return null;
            return AssetDatabase.GUIDToAssetPath (findAssets[0]);
        }
    }
#endif
}
