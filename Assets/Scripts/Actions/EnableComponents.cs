using CoreInterfaces;
using UnityEngine;

namespace GameFramework
{
    public class EnableComponents : MonoBehaviour, IActionMonoBehaviour
    {

        public Behaviour[] components;
        public void InvokeAction(){
            foreach (var item in components)
            {
                item.enabled = true;
            }
        }
    }
}
