using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    [DefaultExecutionOrder(-1000)]
    public class LevelManager : Entity, IPreAwake
    {
        public bool injectOnlyChildren;
        private GameManager _gameManager;
        [SerializeField] private List<Entity> _entities = new List<Entity>();

        public EnemiesRespawner enemiesRespawner;
        public DialogueManager dialogueManager;

        private bool _injected;

        public GameManager GameManager => _gameManager;

        public List<Entity> Entities => _entities;

        private void Awake()
        {
            if (_injected) return;
            Init();
        }

        private void OnEnable()
        {
            if (_gameManager)
                _gameManager.AddLevel(this);
        }

        private void OnDisable()
        {
            if (_gameManager)
                _gameManager.RemoveLevel(this);
        }

        private void Init()
        {
            if (!_gameManager)
                FindGameManager();
            Inject();

            void Inject()
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

            void InjectOnChildren(GameObject parent)
            {
                var children = parent.GetComponentsInChildren<Entity>(true);
                for (var i = 0; i < children.Length; i++)
                {
                    var containsGameManager = children[i];
                    containsGameManager.OwnerLevel = this;
                }
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

        public void PreAwake()
        {
            if (_injected) return;
            Init();
        }

        
    }
}