using UnityEngine;

namespace GameFramework
{
    public interface ISubsystem<T> where T : MonoBehaviour, IContainsSubsystems<T>
    {
        public T OwnerManager { get; set; }
    }
    public interface IPostInitAllSubsystems
    {
        public void PostInitAllSubsystems();
    }

}