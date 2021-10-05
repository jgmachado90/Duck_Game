using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public abstract class GameState : MonoBehaviour
    {
        public List<string> visitedScenes;

        public event Action OnStateChanged;

        public void AddVisitedScene(string scene)
        {
            if (visitedScenes.Contains(scene)) return;
            visitedScenes.Add(scene);
            OnStateChanged?.Invoke();
        }
    }
}