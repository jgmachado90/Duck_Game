using UnityEngine;

namespace TweenSystem
{
    public class SmoothCamera : MonoBehaviour
    {
        public Transform cam;
        [SerializeField] [Range(0, 2)] private float smooth = 1;

        private Camera _camera;
        private Camera _mainCamera;

        private void Awake()
        {
#if UNITY_ANDROID
            Destroy(gameObject);
#endif
        }

        public float SmoothPercentage
        {
            set => smooth = Mathf.Lerp(0, 2, value);
        }

        private void Start()
        {
            if (cam == null && Camera.main != null)
            {
                _mainCamera = Camera.main;
                cam = Camera.main.transform;
            }

            _camera = GetComponent<Camera>();

            transform.parent = null;
        }
    
        private void LateUpdate()
        {
            if (_camera && _mainCamera && _camera.cullingMask != _mainCamera.cullingMask)
            {
                _camera.cullingMask = _mainCamera.cullingMask;
            }

            float f = 1 - 1 / Mathf.Pow(10, smooth);
            transform.position = Vector3.Lerp(cam.position, transform.position, f);
            transform.rotation = Quaternion.Slerp(cam.rotation, transform.rotation, f);
        }
    }
}