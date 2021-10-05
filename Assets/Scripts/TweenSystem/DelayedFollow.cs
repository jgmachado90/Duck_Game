using System.Collections.Generic;
using UnityEngine;

namespace TweenSystem
{
    public class DelayedFollow : MonoBehaviour
    {
        public Transform target;
        public int frameDelay=60;
        public float distFollow = 0.05f;
    
        private Queue<Vector3>_positionList = new Queue<Vector3>();
        private Vector3 _lastPos;

        private void Start()
        {
            _lastPos = target.localPosition;
            for (int i = 0; i < frameDelay; i++)
            {
                _positionList.Enqueue(target.localPosition);
            }
        }

        private void Update()
        {
            if ((_lastPos - target.localPosition).sqrMagnitude >= distFollow)
            {
                var localPosition = target.localPosition;
                _lastPos = localPosition;
                _positionList.Enqueue(localPosition);
                if (_positionList.Count > frameDelay)
                {
                    _positionList.Dequeue();
                    transform.localPosition = _positionList.Peek();
                }
            }

        }
    }
}
