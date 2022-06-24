using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    [DefaultExecutionOrder(-1001)]
    public class GameManager : MonoBehaviour, IContainsSubsystems<GameManager>
    {
        private const string GameManagerName = "GameManager";

        [SerializeField] private TimeManager timeManager;
        [SerializeField] private List<MonoBehaviour> startSubsystems;

        private List<ISubsystem<GameManager>> _subsystems = new List<ISubsystem<GameManager>>();
        private List<LevelManager> _currentLevels = new List<LevelManager>();
        public TimeManager TimeManager => timeManager;
        public List<MonoBehaviour> StartSubsystems => startSubsystems;
        public List<ISubsystem<GameManager>> Subsystems => _subsystems;
        public List<LevelManager> CurrentLevels => _currentLevels;

        public event Action<LevelManager> OnLevelAdded;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void CreateInstance()
        {

            var gameManagerPrefab = GetAssetFromResources();
            if (gameManagerPrefab)
            {
                gameManagerPrefab.gameObject.SetActive(false);
            }
            var gameManagerInstance = gameManagerPrefab ? Instantiate(gameManagerPrefab) : new GameObject(GameManagerName).AddComponent<GameManager>();
            DontDestroyOnLoad(gameManagerInstance);
            gameManagerInstance.gameObject.SetActive(true);
            if (gameManagerPrefab)
            {
                gameManagerPrefab.gameObject.SetActive(true);
            }
        }

        private void Awake()
        {
            this.InitSubsystems();
        }

        public static GameManager GetAssetFromResources()
        {
            return Resources.Load<GameManager>(GameManagerName);
        }

        public void AddLevel(LevelManager levelManager)
        {
            if (_currentLevels.Contains(levelManager)) return;
            _currentLevels.Add(levelManager);
            OnLevelAdded?.Invoke(levelManager);
        }
        public void RemoveLevel(LevelManager levelManager)
        {
            _currentLevels.Remove(levelManager);
        }

        public bool IsPaused()
        {
            return TimeManager.IsPaused();
        }
    }
}
