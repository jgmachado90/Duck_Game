using System;
using System.Collections.Generic;
using Addons.EditorButtons.Runtime;
using GameFramework;
using UnityEngine;


public class TriggerArea : Entity
{
    public CollisionPreset collisionPreset;
    public VolumeMode mode;
    public bool useTrigger = true;
    public Feedback feedbackEnter;
    public Feedback feedbackExit;
    private List<GameObject> _triggersGameObjects;

    public event Action<TriggerArea> OnEnterArea;
    public event Action<TriggerArea> OnExitArea;

    private bool _active;

    public GameObject DetectedObject { get; private set; }


    private void Start()
    {
        if (useTrigger)
            GenerateTriggers();
    }

    private void GenerateTriggers()
    {
        gameObject.GenerateTriggers(new GenerateTriggersParams()
        {
            exit = Exit,
            enter = Enter,
            triggerEnter = true,
            triggerExit = true,
            mode = mode,
            collisionPreset = collisionPreset,
        }, out _triggersGameObjects, out _);
    }

    private void Enter(GameObject obj)
    {
        if (!_active) DetectedObject = obj;
        Enter();
    }

    private void Exit(GameObject obj)
    {
        Exit();
        if (!_active && obj == DetectedObject) DetectedObject = null;
    }

    [Button(ButtonMode.EnabledInPlayMode)]
    private void Enter()
    {
        if (_active) return;
        if (!enabled) return;
        _active = true;
        feedbackEnter.Invoke(transform, this);
        OnEnterArea?.Invoke(this);
    }

    [Button(ButtonMode.EnabledInPlayMode)]
    private void Exit()
    {
        if (!_active) return;
        if (!enabled) return;
        _active = false;
        feedbackExit.Invoke(transform, this);
        OnExitArea?.Invoke(this);
    }
}

