using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilsEditor;

public class PatrollComponent : MonoBehaviour
{
    public bool activateGizmos;

    private Transform content;

    [Header("PatrollSettings")]
    [SerializeField] private List<Transform> wayPoints = new List<Transform>();
    [SerializeField] private float wanderSpeed;
    [SerializeField] private bool circular;
    [SerializeField] private bool oneWayPath;

    private bool started;
    private float startTime;
    private float journeyLength;
    private Vector3 startMarker;
    private Vector3 endMarker;
    private Transform target;
    public Transform Target { get { return target; } }
    private int dir;

    private void Start() => dir = 1;

    public void InitializePatroll(Transform _content)
    {
        content = _content;
        ValidatePoints();
        target = GetNearestPoint();
        if (target == null) return;
        SetMovement(content.position, target.position);
    }

    public void FinalizePatroll()
    {
        started = false;
    }

    private void SetMovement(Vector3 initial, Vector3 final)
    {
        started = true;
        startTime = Time.time;
        startMarker = initial;
        endMarker = final;
        journeyLength = Vector3.Distance(initial, final);
    }

    private Transform GetNearestPoint()
    {
        float nearestDist = Mathf.Infinity;
        Transform nearest = null;
        foreach (Transform point in wayPoints)
        {
            float dist = Vector3.Distance(content.transform.position, point.position);
            if (dist < nearestDist)
            {
                nearest = point;
                nearestDist = dist;
            }
        }
        return nearest;
    }

    public void Update()
    {
        if (started)
        {
            Move();
            ReachCheck();
        }
    }

    private void ReachCheck()
    {
        if (Vector3.Distance(content.position, target.position) < 0.01f)
        {
            if ((dir > 0 && wayPoints[wayPoints.IndexOf(target)] != wayPoints[wayPoints.Count - 1]) ||
                (dir < 0) && wayPoints[wayPoints.IndexOf(target)] != wayPoints[0])
            {
                target = wayPoints[wayPoints.IndexOf(target) + dir];
                SetMovement(content.position, target.position);
                return;
            }

            if (circular)
            {
                target = wayPoints[0];
                SetMovement(content.position, target.position);
                return;
            }
            if (!oneWayPath)
                ChangeDirection();
        }
    }

    public void ChangeDirection()
    {
        dir *= -1;
        target = wayPoints[wayPoints.IndexOf(target) + dir];
        SetMovement(content.position, target.position);
    }

    private void Move()
    {
        float distCovered = (Time.time - startTime) * wanderSpeed;
        float fractionOfJourney = distCovered / journeyLength;
        content.position = Vector3.Lerp(startMarker, endMarker, fractionOfJourney);
    }


#if UNITY_EDITOR
    [Button(ButtonMode.DisabledInPlayMode)]
    public void CreatePoint()
    {
        ValidatePoints();
        GameObject newPoint = new GameObject("Point" + (wayPoints.Count + 1));
        newPoint.transform.SetParent(transform);
        wayPoints.Add(newPoint.transform);
        UnityEditor.EditorUtility.SetDirty(gameObject);
    }
#endif
    private void OnValidate()
    {
        ValidatePoints();
    }

    [Button(ButtonMode.DisabledInPlayMode)]
    public void ValidatePoints()
    {
        wayPoints.Clear();
        foreach (Transform child in transform)
        {
            wayPoints.Add(child);
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
            for (int i = 0; i < wayPoints.Count; i++)
            {
                Gizmos.DrawWireSphere(wayPoints[i].position, pointsRadius);
                if (wayPoints[i] != wayPoints[wayPoints.Count - 1])
                {
                    Gizmos.DrawLine(wayPoints[i].position, wayPoints[i + 1].position);
                }
            }
            if (target == null || content == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawCube(target.position, Vector3.one * 0.1f);
        }
    }
}


