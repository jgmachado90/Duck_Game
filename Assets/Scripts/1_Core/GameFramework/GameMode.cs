using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    [DefaultExecutionOrder(-999)]
    public class GameMode : Entity, IContainsSubsystems<GameMode>
    {
        public const string PlayerTag = "Player";

        [SerializeField] private Transform playerCharacter;
        [SerializeField] private List<MonoBehaviour> startSubsystems;

        public virtual Transform PlayerCharacter => playerCharacter;

        private List<ISubsystem<GameMode>> _subsystems = new List<ISubsystem<GameMode>>();
        public List<MonoBehaviour> StartSubsystems => startSubsystems;
        public List<ISubsystem<GameMode>> Subsystems => _subsystems;

        public virtual void Init()
        {
            this.InitSubsystems();
        }
    }
}
