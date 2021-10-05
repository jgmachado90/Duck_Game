using UnityEngine;

namespace TweenSystem
{
    [RequireComponent(typeof(LineRenderer)),ExecuteInEditMode]
    public class BezierLineRender : MonoBehaviour
    {
        public BezierCurve curve;
    
        [Range(0.001f,1)]
        public float step = 0.1f;

        private LineRenderer _lineRenderer;

        private void Start()
        {
            if(!Application.isEditor)
                UpdateRender();
            if (Application.isPlaying) enabled = false;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateRender();
        }

        private void Update()
        {
            UpdateRender();
        }
#endif

        private void UpdateRender()
        {
            if (!_lineRenderer)
                _lineRenderer = GetComponent<LineRenderer>();

            if (!curve || !_lineRenderer) return;

            float u = 0;
            _lineRenderer.positionCount = 0;
            _lineRenderer.positionCount++;

            _lineRenderer.SetPosition(
                _lineRenderer.positionCount - 1,
                transform.InverseTransformPoint(curve.GetPos(u)));

            curve.GetPos(u);
            while (u < 1)
            {
                u += step;
                if (u > 1)
                    u = 1;

                _lineRenderer.positionCount++;

                _lineRenderer.SetPosition(
                    _lineRenderer.positionCount - 1,
                    transform.InverseTransformPoint(curve.GetPos(u)));
            }
        }
    }
}
