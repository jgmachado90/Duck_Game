using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public interface IContainsSubsystems<T> where T : MonoBehaviour, IContainsSubsystems<T>
    {

        public List<MonoBehaviour> StartSubsystems { get; }

        public List<ISubsystem<T>> Subsystems { get; }

    }
    public static class SubsystemExtensions
    {

        public static void InitSubsystems<T>(this T containsSubsystems) where T : MonoBehaviour, IContainsSubsystems<T>
        {
            foreach (var subsystem in containsSubsystems.GetComponents<MonoBehaviour>())
            {
                containsSubsystems.AddStartSubsystem(subsystem);
            }
            foreach (var item in containsSubsystems.StartSubsystems)
            {
                if (item is ISubsystem<T> subsystem)
                {
                    containsSubsystems.AddSubsystem(subsystem);
                }
            }

            foreach (var item in containsSubsystems.Subsystems)
            {
                if (item is IPostInitAllSubsystems postInitAllSubsystems)
                {
                    postInitAllSubsystems.PostInitAllSubsystems();
                }
            }
        }

        private static void AddSubsystem<T>(this T containsSubsystems, ISubsystem<T> subsystem) where T : MonoBehaviour, IContainsSubsystems<T>
        {
            if (containsSubsystems.Subsystems.Contains(subsystem)) return;
            containsSubsystems.Subsystems.Add(subsystem);
            subsystem.OwnerManager = containsSubsystems;
        }

        private static void AddStartSubsystem<T>(this T containsSubsystems, MonoBehaviour monoBehaviour) where T : MonoBehaviour, IContainsSubsystems<T>
        {
            var subsystem = monoBehaviour as ISubsystem<T>;
            if (subsystem == null || containsSubsystems.StartSubsystems.Contains(monoBehaviour)) return;
            containsSubsystems.StartSubsystems.Add(monoBehaviour);
        }

        public static void RemoveSubsystem<T>(this T containsSubsystems, ISubsystem<T> subsystem) where T : MonoBehaviour, IContainsSubsystems<T>
        {
            if (!containsSubsystems.Subsystems.Contains(subsystem)) return;
            containsSubsystems.Subsystems.Remove(subsystem);
            containsSubsystems.RemoveSubsystem(subsystem);
        }

        public static T2 GetSubsystem<T1, T2>(this T1 containsSubsystems, bool forceCreate = false)
            where T1 : MonoBehaviour, IContainsSubsystems<T1>
            where T2 : MonoBehaviour, ISubsystem<T1>
        {
            if (containsSubsystems == null) return null;
            if (!Application.isPlaying)
            {
                return GetSubsystemEditor<T1, T2>(containsSubsystems);
            }

            foreach (var subsystem in containsSubsystems.Subsystems)
            {
                if (subsystem is T2 result)
                {
                    return result;
                }
            }

            if (forceCreate == false) return null;
            var newSubsystem = containsSubsystems.gameObject.AddComponent<T2>();
            containsSubsystems.AddSubsystem(newSubsystem);
            return newSubsystem;
        }

        private static T2 GetSubsystemEditor<T1, T2>(T1 containsSubsystems) where T1 : MonoBehaviour, IContainsSubsystems<T1>
            where T2 : MonoBehaviour, ISubsystem<T1>
        {
            var subsystem = containsSubsystems.GetComponent<T2>();
            if (subsystem)
            {
                return subsystem;
            }

            foreach (var item in containsSubsystems.StartSubsystems)
            {
                if (item is T2 result)
                {
                    {
                        return result;
                    }
                }
            }

            return subsystem;
        }
    }
}
