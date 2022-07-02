using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CollisionSystem;
using GameFramework;
using System;
using UtilsEditor;
public class Elevator : Entity
{
    public bool activateGizmos;

    [SerializeField] private TriggerArea triggerArea;
    [SerializeField] Transform content;

    [Header("Elevator Settings")]
    [SerializeField] private Transform lowerLevel;
    [SerializeField] private Transform highLevel;
    [SerializeField] private float speed;
    [SerializeField] private bool lowLevel;
    [SerializeField] private bool loop;
    [SerializeField] private bool initializeOnStart;

    private float startTime;
    private float journeyLength;
    private Vector3 startMarker;
    private Vector3 endMarker;
    private Transform target;
    private bool moving;

    private Player playerRef;

    private void OnEnable()
    {
        if (!initializeOnStart)
        {
            triggerArea.OnEnterArea += EnteringArea;
            triggerArea.OnExitArea += ExitingArea;
        }
    }

    private void OnDisable()
    {
        if (!initializeOnStart)
        {
            triggerArea.OnEnterArea -= EnteringArea;
            triggerArea.OnExitArea -= ExitingArea;
        }
    }

    private void Start()
    {
        content.position = lowerLevel.position;
    }

    private void EnteringArea(GameObject obj)
    {
        Player player = obj.GetComponentInParent<Player>();
        if (player == null) return;
        playerRef = player;
        player.content.parent = transform;
        if (!moving)
            InitializeMovement();
    }
    private void ExitingArea(GameObject obj)
    {
        if (playerRef)
            playerRef.content.parent = playerRef.transform;
    }

    public void InitializeMovement()
    {
        if (lowLevel)
        {
            target = highLevel;
            SetMovement(lowerLevel.position, highLevel.position);
        }
        else
        {
            target = lowerLevel;
            SetMovement(highLevel.position, lowerLevel.position);
        }
    }

    private void SetMovement(Vector3 initial, Vector3 final)
    {
        startTime = Time.time;
        startMarker = initial;
        endMarker = final;
        journeyLength = Vector3.Distance(initial, final);
        moving = true;
    }

    public void Update()
    {
        if (moving)
        {
            Move();
            ReachCheck();
        }
    }
    private void Move()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        content.position = Vector3.Lerp(startMarker, endMarker, fractionOfJourney);
    }
    private void ReachCheck()
    {
        if (Vector3.Distance(content.position, target.position) < 0.01f)
        {
            moving = false;
            lowLevel = !lowLevel;
            if (loop && initializeOnStart)
            {
                InitializeMovement();
            }
        }
    }

}
