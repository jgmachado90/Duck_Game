using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameFramework
{
    public class GameManager : MonoBehaviour
    {


        public LoadSceneManager loadSceneManager;

        private List<LevelManager> _currentLevels = new List<LevelManager>();

        public List<LevelManager> CurrentLevels => _currentLevels;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void CreateInstance()
        {
            const string nameGameManager = "GameManager";
            var gameManagerInstance = Resources.Load<GameManager>(nameGameManager);
            gameManagerInstance = gameManagerInstance ? Instantiate(gameManagerInstance) : new GameObject(nameGameManager).AddComponent<GameManager>();
            DontDestroyOnLoad(gameManagerInstance);
        }

        public void AddLevel(LevelManager levelManager)
        {
            _currentLevels.Add(levelManager);
        }

        public void RemoveLevel(LevelManager levelManager)
        {
            _currentLevels.Remove(levelManager);
        }
    }
}