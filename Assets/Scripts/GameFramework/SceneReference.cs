using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

#endif

namespace GameFramework
{
    [CreateAssetMenu(fileName = "SceneReference", menuName = "SceneReference/Scene Reference")]
    public class SceneReference : ScriptableObject
    {
        [SerializeField] private string sceneName;
#if UNITY_EDITOR
        [SerializeField] private SceneAsset scene;
#endif

        public string SceneName
        {
            get { return GetSceneName(); }
        }

        protected virtual string GetSceneName()
        {
#if UNITY_EDITOR
            if (scene && sceneName != scene.name)
            {
                UpdateName();
                EditorUtility.SetDirty(this);
            }
#endif
            return sceneName;
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            UpdateName();
        }

        private void OnValidate()
        {
            UpdateName();
        }

        protected virtual void UpdateName()
        {
            if (scene)
                sceneName = scene.name;
        }
#endif
    }
}