using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace GameFramework
{
    [DefaultExecutionOrder(-1000)]
    public class LevelManager : Entity, IPauseReceiver, IContainsSubsystems<LevelManager>
    {
        [SerializeField] private bool autoInit = true;
        [SerializeField] private bool injectOnlyChildren;
        [SerializeField] private GameMode gameMode;
        [SerializeField] private ObjectPool objectPool;
        [SerializeField] private List<MonoBehaviour> startSubsystems;
        [SerializeField] private List<LevelManager> subLevelsInScene;
        [SerializeField] private PauseEntitiesRegister pauseEntities;

        private bool _triedEnable;
        private GameManager _gameManager;
        private List<Entity> _entities = new List<Entity>();
        private List<LevelManager> _subLevels = new List<LevelManager>();
        private HashSet<string> _waitForLevels = new HashSet<string>();

        private List<ISubsystem<LevelManager>> _subsystems = new List<ISubsystem<LevelManager>>();
        private bool _injected;
        private LevelManager _parentLevel;
        public List<MonoBehaviour> StartSubsystems => startSubsystems;
        public List<ISubsystem<LevelManager>> Subsystems => _subsystems;

        public List<LevelManager> SubLevels => _subLevels;
        public GameManager GameManager => _gameManager;
        public List<Entity> Entities => _entities;
        public GameMode GameMode => gameMode;
        public ObjectPool ObjectPool => objectPool;

        public event Action<LevelManager> OnSubLevelAdded;
        public event Action<Entity> OnSpawned;
        public event Action OnPostInitAllSystems;
        public event Action OnBeforeDestroy;
        public LevelManager ParentLevel => _parentLevel;

#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (var subLevel in subLevelsInScene)
            {
                if (subLevel.autoInit)
                {
                    subLevel.autoInit = false;
                    UnityEditor.EditorUtility.SetDirty(subLevel);
                }
            }
        }
#endif

        private void Awake()
        {
            if (autoInit) Init();
            this.InitSubsystems();
        }

        private void SubLevelAdded(LevelManager newLevelManager)
        {
            var newLevelName = newLevelManager.gameObject.scene.name;
            if (_waitForLevels.Contains(newLevelName) && newLevelManager._parentLevel == null)
            {
                StoreNewSubLevel(newLevelManager);
                _waitForLevels.Remove(newLevelName);
            }
        }

        private void AddSubLevelsInScene()
        {
            for (int i = 0; i < subLevelsInScene.Count; i++)
            {
                StoreNewSubLevel(subLevelsInScene[i]);
                subLevelsInScene[i].Init();
            }
        }

        private void StoreNewSubLevel(LevelManager newLevelManager)
        {
            _subLevels.Add(newLevelManager);
            newLevelManager._parentLevel = this;
            OnSubLevelAdded?.Invoke(newLevelManager);
#if UNITY_EDITOR
            if (newLevelManager.gameObject.scene.name != gameObject.scene.name)
            {
                UnityEditor.SceneVisibilityManager.instance.Hide(newLevelManager.gameObject.scene.GetRootGameObjects(), true);
            }
#endif
        }

        private void OnEnable()
        {
            OnEnabled();
        }

        private void OnEnabled()
        {
            if (_gameManager)
            {
                _triedEnable = false;
                _gameManager.AddLevel(this);
                _gameManager.OnLevelAdded += SubLevelAdded;
                this.GetTimeManager().RegisterPause(this);
            }
            else
            {
                _triedEnable = true;
            }
        }

        private void OnDisable()
        {
            if (_gameManager)
            {
                _gameManager.OnLevelAdded -= SubLevelAdded;
                _gameManager.RemoveLevel(this);
                this.GetTimeManager().UnregisterPause(this);
            }
        }

        public override void OnDestroy()
        {
            OnBeforeDestroy?.Invoke();
            for (var i = _entities.Count - 1; i >= 0; i--)
            {
                var entity = _entities[i];
                if (!entity) continue;
                entity.OnBeforeDestroyLevel();
            }
        }

        private void Init()
        {
            if (_injected) return;
            if (!_gameManager) FindGameManager();
            if (_gameManager) _gameManager.AddLevel(this);
            Inject();
            if (gameMode) gameMode.Init();
            AddSubLevelsInScene();
            if (_triedEnable)
            {
                OnEnabled();
            }
            OnPostInitAllSystems?.Invoke();
        }

        private void Inject()
        {
            if (injectOnlyChildren)
            {
                InjectOnChildren(gameObject);
            }
            else
            {
                var gameObjects = gameObject.scene.GetRootGameObjects();
                for (var i = 0; i < gameObjects.Length; i++)
                {
                    var item = gameObjects[i];
                    InjectOnChildren(item);
                }
            }
        }

        private void InjectOnChildren(GameObject parent)
        {
            var children = parent.GetComponentsInChildren<Entity>(true);
            for (var i = 0; i < children.Length; i++)
            {
                var containsGameManager = children[i];
                containsGameManager.OwnerLevel = this;
            }
        }

        private void FindGameManager()
        {
            var go = new GameObject("Sacrificio");
            DontDestroyOnLoad(go);

            foreach (var root in go.scene.GetRootGameObjects())
            {
                if (root.TryGetComponent(out GameManager gm))
                {
                    _gameManager = gm;
                }
            }
            Destroy(go);
        }

        public string GetSceneName()
        {
            return gameObject.scene.name;
        }

        public LevelManager GetSubLevelOfName(string sceneName)
        {
            return SubLevels.Find(manager => manager.gameObject.scene.name == sceneName);
        }

        public void AddEntity(Entity entity)
        {
            if (entity == this) return;
            pauseEntities.RegisterPause(entity);
            _entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            if (entity == this) return;
            pauseEntities.UnregisterPause(entity);
            _entities.Remove(entity);
        }

        public void OnPaused()
        {
            pauseEntities.Pause();
        }

        public void OnUnpaused()
        {
            pauseEntities.Unpause();
        }

        public GameObject Spawn(SpawnableInfo spawnableInfo, Vector3 pos, Quaternion rot, Transform parentTransform = null)
        {
            GameObject instance;
            if (spawnableInfo.Spawnable != null)
            {
                instance = spawnableInfo.Spawnable.Spawn(spawnableInfo.Owner, pos, rot, parentTransform);
                if (spawnableInfo.SpawnableEntity)
                {
                    var entity = instance.GetComponent<Entity>();
                    OnSpawnedEntity(spawnableInfo, entity);
                }
            }
            else if (spawnableInfo.SpawnableEntity)
            {
                Entity entity = Instantiate(spawnableInfo.SpawnableEntity, pos, rot, parentTransform);
                instance = entity.gameObject;
                OnSpawnedEntity(spawnableInfo, entity);
            }
            else
            {
                instance = Instantiate(spawnableInfo.Prefab, pos, rot, parentTransform);
            }
            return instance;
        }

        private void OnSpawnedEntity(SpawnableInfo spawnableInfo, Entity entity)
        {
            entity.OwnerLevel = spawnableInfo.Owner.OwnerLevel;
            if (spawnableInfo.CanReceiveEnvInfo && entity.TryGetComponent<IEnvInfoReceiver>(out var envInfoReceiver))
            {
                envInfoReceiver.SetEnvironmentInfo(spawnableInfo.EnvironmentInfoContainerContainer);
            }
            OnSpawned?.Invoke(entity);
        }

        public IEnumerable<T> GetAllEntitiesOfType<T>() where T : Entity
        {
            for (var i = _entities.Count - 1; i >= 0; i--)
            {
                var item = _entities[i];
                if ((item as T) != null)
                {
                    yield return item as T;
                }
            }
        }
    }
}
