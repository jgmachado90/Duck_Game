using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(BoxCollider2D)),ExecuteAlways()]
    public class ScaleEdgeBoxCollider2D : MonoBehaviour{
        public Transform scaleReference;
        public float edgeRadius = 0.05f;

        private BoxCollider2D _boxCollider2D;
        private float _lastScale;

        private void OnValidate(){
            UpdateEdge();
        }

        private void UpdateEdge(){
            if (!_boxCollider2D) _boxCollider2D = GetComponent<BoxCollider2D>();
            if (scaleReference && _lastScale != scaleReference.lossyScale.x){
                var lossyScale = scaleReference.lossyScale;
                _boxCollider2D.edgeRadius = edgeRadius * lossyScale.x;
                _lastScale = lossyScale.x;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        private void Update(){
            if (Application.isPlaying){
                enabled = false;
            }
            UpdateEdge();
        }
    }
}
