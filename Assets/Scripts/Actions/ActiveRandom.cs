using CoreInterfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFramework
{
    public class ActiveRandom : MonoBehaviour, IActionMonoBehaviour {
        public bool onEnable = true;
        public GameObject[] gameObjects;

        private int _id;

        private void OnEnable() {
            DisableAll();
            if(onEnable)
                InvokeAction();
        }

        public void InvokeAction() {
            gameObjects[_id].SetActive(false);
            _id = Random.Range(0, gameObjects.Length);
           gameObjects[_id].SetActive(true);
        }

        private void DisableAll() {
            for(var i = 0; i < gameObjects.Length; i++) {
                var item = gameObjects[i];
                item.SetActive(false);
            }
        }
    }
}
