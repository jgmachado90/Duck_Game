using CoreInterfaces;
using UnityEngine;

namespace GameFramework
{

    [DefaultExecutionOrder(-10)]
    public class ActiveGameObjectAction : MonoBehaviour, IActionMonoBehaviour
    {
        public enum ActiveMode
        {
            Active,
            Disable,
            Toggle
        }
        
        public bool startDisabled;
        public ActiveMode activeMode = ActiveMode.Active;
        public GameObject[] gameObjects;
        
        private void Awake()
        {
            if (startDisabled)
            {
                ActiveAll(false);
            }
        }

        private void ActiveAll(bool value)
        {
            for (var i = 0; i < gameObjects.Length; i++)
            {
                var item = gameObjects[i];
                if(item)
                    item.SetActive(value);
            }
        }
        
        private void ToggleAll()
        {
            for (var i = 0; i < gameObjects.Length; i++)
            {
                var item = gameObjects[i];
                if(item)
                    item.SetActive(!item.activeSelf);
            }
        }
        
        public void InvokeAction()
        {
            if (activeMode == ActiveMode.Toggle)
                ToggleAll();
            else
                ActiveAll(activeMode == ActiveMode.Active);
        }
    }
}
