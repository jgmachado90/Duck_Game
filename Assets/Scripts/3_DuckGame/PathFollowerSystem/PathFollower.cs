using System.Collections.Generic;
using UnityEngine;
using UtilsEditor;
using CollisionSystem;
using GameFramework;

namespace PathFollow
{
    public class PathFollower : Entity{
        [SerializeField] private float durationMultiplier = 1;
        [SerializeField] private bool activateGizmos;
        [SerializeField] private Transform content;
        [SerializeField] private bool moveOnStart;
            
        [SerializeField] private List<PathPoint> pathPoints;
        [SerializeField] private TriggerEnter2D triggerActivation;

        private bool started;
        private int targetIndex;

        private float timeElapsed;
        private float lerpDuration;
        private AnimationCurve currentCurve;
        private Vector3 startPos;
        private Vector3 endPos;
        private float reachDelay;
        private float delay;

        private Vector3 resetStartPos;

        private void OnEnable()
        {
            triggerActivation.OnCollisionEnter += InvokePathFollow;
        }
        private void OnDisable()
        {
            triggerActivation.OnCollisionEnter -= InvokePathFollow;
        }

        private void Start()
        {
            resetStartPos = content.position;
            if (!moveOnStart) return;
            InitializePathFollow();
        }

        private void InitializePathFollow()
        {
            ValidatePoints();   
            SetMovement(1);
        }

        private void InvokePathFollow(GameObject gO)
        {
            if (started) return;
            InitializePathFollow();
        }

        public void ResetPathFollower()
        {
            content.position = resetStartPos;
            started = false;
        }

        private void SetMovement(int index)
        {
            if (pathPoints.Count < index + 1) return;
            started = true;
            targetIndex = index;
            timeElapsed = 0;
            PathPoint currentPoint = pathPoints[index - 1];
            lerpDuration = durationMultiplier * currentPoint.AnimationDuration;
            currentCurve = currentPoint.AnimationCurve;
            startPos = currentPoint.transform.position;
            endPos = pathPoints[index].transform.position;
            reachDelay = pathPoints[index].ReachDelay;
            pathPoints[index].OnStart();
        }

        private void Update()
        {
            if (!started || Time.time < delay) return;
            Move();
            ReachCheck();
            
        }

        private void Move()
        {
            if (timeElapsed > lerpDuration)
            {
                content.position = endPos;
                return;
            }
            var t = timeElapsed / lerpDuration;
            content.position = Vector3.Lerp(startPos, endPos, currentCurve.Evaluate(t));
            timeElapsed += Time.deltaTime;
        }

        private void ReachCheck()
        {
            if (Vector3.Distance(content.position, endPos) < 0.01f)
            {
                delay = Time.time + reachDelay;
                pathPoints[targetIndex].OnReach();
                SetMovement(targetIndex + 1);
            }
        }
        
        private void OnValidate()
        {
            ValidatePoints();
        }

        [Button(ButtonMode.DisabledInPlayMode)]
        public void ValidatePoints()
        {
            pathPoints.Clear();
            foreach (Transform child in transform)
            {
                PathPoint pathPoint = child.GetComponent<PathPoint>();
                if(pathPoint == null) continue;
                pathPoints.Add(pathPoint);
            }
            
#if UNITY_EDITOR 
            UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
        }

        [Header("Gizmos Settings")]
        [SerializeField] private float pointsRadius = 0.1f;
        private void OnDrawGizmos()
        {
            if (activateGizmos)
            {
                for (int i = 0; i < pathPoints.Count; i++)
                {
                    Gizmos.DrawWireSphere(pathPoints[i].transform.position, pointsRadius);
                    if (pathPoints[i] != pathPoints[pathPoints.Count - 1])
                    {
                        Gizmos.DrawLine(pathPoints[i].transform.position, pathPoints[i + 1].transform.position);
                    }
                }
            }
        }
    }
}
