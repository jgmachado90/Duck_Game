using UnityEngine;

namespace Utils{
    public class Gaze{

        private float _gazeAngleThreshold;
        private Transform _cameraTransform;
        private Transform _targetTransform;
        private float DotThreshold => Mathf.Cos(_gazeAngleThreshold * Mathf.Deg2Rad);
        private float DotCamera => Vector3.Dot(_cameraTransform.forward, (_targetTransform.position - _cameraTransform.position).normalized);

        public bool IsVisible => DotCamera >= DotThreshold;

        public Gaze(float gazeAngleThreshold, Transform cameraTransform, Transform targetTransform){
            _gazeAngleThreshold = gazeAngleThreshold;
            _cameraTransform = cameraTransform;
            _targetTransform = targetTransform;
        }
    }
}