using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using CollisionSystem;
using System;
using MoreMountains.Feedbacks;

public class TriggerFeedback : Entity
{
    [SerializeField] private TriggerEnter2D triggerEnter;
    [SerializeField] private MMFeedbacks feedback;
    public bool once;
    private bool played;
    private void OnEnable()
    {
        triggerEnter.OnCollisionEnter += PlayTriggerFeedback;
    }
    private void OnDisable()
    {
        triggerEnter.OnCollisionEnter -= PlayTriggerFeedback;
    }

    private void PlayTriggerFeedback(GameObject obj)
    {
        if (once && played) return;
        if (feedback == null) return;
        feedback.PlayFeedbacks();
        played = true;
    }
}
