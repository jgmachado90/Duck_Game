using CoreInterfaces;

namespace GameFramework
{
    public interface IEntityActionMonoBehaviour: IActions {
        public void InvokeAction(Entity ownerEntity);
    }
    
    public interface IStopEntityActionMonoBehaviour: IActions {
        public void StopAction(Entity ownerEntity);
    }
}
