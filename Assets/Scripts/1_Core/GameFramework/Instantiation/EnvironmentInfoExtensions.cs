using System.Collections.Generic;
using GameFramework;

namespace GameFramework{
    public interface IEnvInfoContainer{
        public List<Entity> EnvInfoComponents{get;}
    }

    public interface IEnvInfoReceiver{
        public void SetEnvironmentInfo(IEnvInfoContainer envInfoContainer);
    }

    public static class EnvironmentInfoExtensions{
        public static T GetEnvironmentComponentOfType<T>(this IEnvInfoContainer envInfoContainer) where T: Entity{
            for (var i = 0; i < envInfoContainer.EnvInfoComponents.Count; i++){
                var item = envInfoContainer.EnvInfoComponents[i];
                if (item is T result){
                    return result;
                }
            }
            return null;
        }
    }
}
