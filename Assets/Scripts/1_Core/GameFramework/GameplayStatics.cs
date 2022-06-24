using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework
{
    public static class GameplayStatics
    {
        public static GameManager GetGameManager(this Entity entity)
        {
            if (!Application.isPlaying) return GameManager.GetAssetFromResources();
            return entity == null ? null : entity.GetOwnerLevel()?.GameManager;
        }
        public static LevelManager GetOwnerLevel(this Entity entity)
        {
            return entity.OwnerLevel;
        }

        public static bool IsPaused(this Entity entity)
        {
            return entity.GetGameManager().IsPaused();
        }
        public static TimeManager GetTimeManager(this Entity entity)
        {
            return entity.GetGameManager()?.TimeManager;
        }
        public static GameMode GetGameMode(this Entity entity)
        {
            return entity.GetOwnerLevel()?.GameMode;
        }

        public static T GetGameModeSubsystem<T>(this Entity entity, bool forceCreate = false) where T : MonoBehaviour, ISubsystem<GameMode>
        {
            return entity.GetGameMode()?.GetSubsystem<GameMode, T>(forceCreate);
        }
        public static T GetLevelManagerSubsystem<T>(this Entity entity, bool forceCreate = false) where T : MonoBehaviour, ISubsystem<LevelManager>
        {
            return entity.GetOwnerLevel()?.GetSubsystem<LevelManager, T>(forceCreate);
        }
        public static T GetGameManagerSubsystem<T>(this Entity entity) where T : MonoBehaviour, ISubsystem<GameManager>
        {
            return entity.GetGameManager()?.GetSubsystem<GameManager, T>();
        }



    }
}
