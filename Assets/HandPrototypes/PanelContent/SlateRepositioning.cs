using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateRepositioning : MonoBehaviour
{
    public float Smoothing;
    public float SnapThreshold;
    private bool wasPinching;

    public bool CurrentlyRepositioning { get; private set; }
    public SlateResizing Resizing;
    public BoxCollider SlateBox;
    public float GrabThreshold;
    private Transform unsnappedTransform;
    private Transform snappedTransform;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Start()
    {
        unsnappedTransform = new GameObject("Unsnapped Transform").transform;
        unsnappedTransform.position = transform.position;
        unsnappedTransform.rotation = transform.rotation;

        snappedTransform = new GameObject("Snapped Transform").transform;
        snappedTransform.position = transform.position;
        snappedTransform.rotation = transform.rotation;

        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    public void OnEndResizing()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
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
        if(pinching)
        {
            if(CurrentlyRepositioning)
            {
                UpdatePinchPoint();
            }
        }
        else
        {
            EndRepositioning();
        }
        bool shoulStartPinch = GetShouldStartGrab(pinching);
        if (shoulStartPinch)
        {
            StartGrab();
        }
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * Smoothing);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * Smoothing);
    }

    private void EndRepositioning()
    {
        CurrentlyRepositioning = false;
        snappedTransform.parent = null;
        unsnappedTransform.parent = null;
    }

    private void UpdatePinchPoint()
    {
        targetPosition = snappedTransform.position;
        targetRotation = GetSnappedRotation();
    }
    private Quaternion GetSnappedRotation()
    {

        snappedTransform.position = snappedTransform.position;
        snappedTransform.LookAt(Camera.main.transform);
        snappedTransform.Rotate(0, 180, 0, Space.Self);


        float dot = Vector3.Dot(unsnappedTransform.forward, snappedTransform.forward);
        if (dot > SnapThreshold)
        {
            return snappedTransform.rotation;
        }
        return unsnappedTransform.rotation;
    }

    private bool GetShouldStartGrab(bool pinching)
    {
        if(pinching && !wasPinching)
        {
            Vector3 grabPoint = MainPinchDetector.Instance.PinchPoint.position;
            Vector3 closestPoint = SlateBox.ClosestPoint(grabPoint);
            float grabDist = (grabPoint - closestPoint).magnitude;
            return grabDist < GrabThreshold;
        }
        return false;
    }

    private void StartGrab()
    {
        CurrentlyRepositioning = true;
        unsnappedTransform.position = transform.position;
        unsnappedTransform.rotation = transform.rotation;
        unsnappedTransform.parent = MainPinchDetector.Instance.PinchPoint;

        snappedTransform.position = transform.position;
        snappedTransform.rotation = transform.rotation;
        snappedTransform.parent = MainPinchDetector.Instance.PinchPoint;
    }
}
