using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateRepositioning : MonoBehaviour
{
    private bool wasPinching;
    private Transform pinchPoint;

    public bool CurrentlyRepositioning { get; private set; }
    public SlateResizing Resizing;
    public BoxCollider SlateBox;
    public float GrabThreshold;

    private void Start()
    {
        pinchPoint = new GameObject("Repositioning Point").transform;
    }

    public void UpdateSlatePositioning()
    {
        bool pinching = MainPinchDetector.Instance.Pinching;
        if (!Resizing.CurrentlyResizing)
        {
            UpdateInteraction(pinching);
        }
        wasPinching = pinching;
    }

    private void UpdateInteraction(bool pinching)
    {

        if (!pinching)
        {
            CurrentlyRepositioning = false;
            transform.parent = null;
        }
        if(pinching && CurrentlyRepositioning)
        {
            ContinuePinch();
        }
        if(pinching && !wasPinching)
        {
            bool shoulStartPinch = GetShouldStartGrab();
            if(shoulStartPinch)
            {
                StartGrab();
            }
        }
    }

    private void ContinuePinch()
    {
        Vector3 positionTarget = MainPinchDetector.Instance.PinchPoint.position;
        Quaternion rotationTarget = MainPinchDetector.Instance.PinchPoint.rotation;
        // ToDo: The easing, snapping, etc.

        pinchPoint.position = positionTarget;
        pinchPoint.rotation = rotationTarget;
    }

    private bool GetShouldStartGrab()
    {
        Vector3 grabPoint = MainPinchDetector.Instance.PinchPoint.position;
        Vector3 closestPoint = SlateBox.ClosestPoint(grabPoint);
        float grabDist = (grabPoint - closestPoint).magnitude;
        return grabDist < GrabThreshold;
    }

    private void StartGrab()
    {
        CurrentlyRepositioning = true;
        pinchPoint.position = MainPinchDetector.Instance.PinchPoint.position;
        pinchPoint.rotation = MainPinchDetector.Instance.PinchPoint.rotation;
        transform.parent = pinchPoint;
    }
}
