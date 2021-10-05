using CoreInterfaces;
using UnityEngine;

namespace GameFramework
{
    public class DestroyAction : MonoBehaviour, IActionMonoBehaviour
    {
        public void InvokeAction()
        {
            Destroy(gameObject);
        }
    }
}
