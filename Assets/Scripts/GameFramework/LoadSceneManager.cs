using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework
{
    public class LoadSceneManager : MonoBehaviour
    {

        public event Action OnBeginLoadScene;
        public event Action<Scene> OnBeforeActiveScene;
        private string _lastScene;
        private bool _loading;

        public void Load(string name, float delay)
        {
            if (_loading) return;
            _loading = true;
            _lastScene = SceneManager.GetActiveScene().name;
            OnBeginLoadScene?.Invoke();
            StartCoroutine(LoadAsync(name, delay));
        }

        public void Load(LoadScene loadScene)
        {
            Load(loadScene.sceneReference, loadScene.delay);
        }

        public void Load(SceneReference sceneReference, float delay)
        {
            Load(sceneReference.SceneName, delay);
        }

        public void Load(int index, float delay)
        {
            string n = SceneManager.GetSceneAt(index).name;
            Load(n, delay);
        }

        public void LoadNext(float delay)
        {
            Load(SceneManager.GetActiveScene().buildIndex + 1, delay);
        }

        public void RestartLevel(float delay)
        {
            Load(SceneManager.GetActiveScene().name, delay);
        }

        private IEnumerator LoadAsync(string scene, float delay)
        {

            if (string.IsNullOrEmpty(scene))
            {
                yield break;
            }

            yield return new WaitForSecondsRealtime(delay);

            var async = SceneManager.LoadSceneAsync(scene,
                new LoadSceneParameters());

            async.allowSceneActivation = false;

            while (async.progress < 0.9f)
            {
                yield return null;
            }

            OnBeforeActiveScene?.Invoke(SceneManager.GetSceneByName(scene));
            async.allowSceneActivation = true;
            _loading = false;
        }
    }
}