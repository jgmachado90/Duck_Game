using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashPlatform : MonoBehaviour
{
    [SerializeField] private bool playOnStart;
    [SerializeField] private float warmingDelay;
    [SerializeField] private float onFloorDelay;
    [SerializeField] private AnimationCurve fallingCurve;
    [SerializeField] private AnimationCurve recoveringCurve;
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float recoveringSpeed;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayer = new LayerMask();
    [SerializeField] private float raycastLength;

    private Vector3 startPos;



    private void Start()
    {
        startPos = transform.position;
        if (playOnStart)
        {
            StartCoroutine(SmashPlatformCoroutine());
        }
    }

    public IEnumerator SmashPlatformCoroutine()
    {
        while (true)
        {
            Vector3 finalPos = TryFindFloor();
            if (finalPos != Vector3.zero)
            {
                yield return new WaitForSeconds(warmingDelay);
                float startTime = Time.time;
                float journeyLength = Vector3.Distance(startPos, finalPos);
                //Falling
                while (Vector3.Distance(transform.position, finalPos) > 0.1f)
                {
                    float distCovered = (Time.time - startTime) * fallingSpeed;
                    float fractionOfJourney = distCovered / journeyLength;
                    transform.position = Vector3.Lerp(startPos, finalPos, fallingCurve.Evaluate(fractionOfJourney));
                    yield return null;
                }
                yield return new WaitForSeconds(onFloorDelay);
                startTime = Time.time;
                journeyLength = Vector3.Distance(transform.position, startPos);
                //Recovering
                while (Vector3.Distance(transform.position, startPos) > 0.1f)
                {
                    float distCovered = (Time.time - startTime) * recoveringSpeed;
                    float fractionOfJourney = distCovered / journeyLength;
                    transform.position = Vector3.Lerp(finalPos, startPos, recoveringCurve.Evaluate(fractionOfJourney));
                    yield return null;
                }
            }
            yield return null;

        }
    }

    private Vector3 TryFindFloor()
    {
        Physics2D.queriesHitTriggers = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up * raycastLength, raycastLength, groundLayer);
        Physics2D.queriesHitTriggers = true;
        if (hit)
            return hit.point;
        else 
            return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, -transform.up * raycastLength);
    }
}
