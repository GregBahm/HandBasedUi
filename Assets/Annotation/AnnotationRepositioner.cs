using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationRepositioner : MonoBehaviour
{
    [SerializeField]
    private BoxFocusable focus;
    public BoxFocusable Focus { get { return this.focus; } }
    
    public float Smoothing;
    private bool wasPinching;
    
    public bool CurrentlyRepositioning { get; private set; }
    
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Transform unsnappedTransform;

    private void Start()
    {
        unsnappedTransform = new GameObject("Unsnapped Transform").transform;
        unsnappedTransform.position = transform.position;
        unsnappedTransform.rotation = transform.rotation;

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
        if (CurrentlyRepositioning)
        {
            if (pinching)
            {
                UpdatePinchPoint();
            }
            else
            {
                EndRepositioning();
            }
        }
        bool shoulStartPinch = GetShouldStartGrab(pinching);
        if (shoulStartPinch)
        {
            StartGrab();
        }
        UpdatePosition();
        wasPinching = pinching;
    }

    private void UpdatePosition()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * Smoothing);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * Smoothing);
    }

    private void EndRepositioning()
    {
        CurrentlyRepositioning = false;
        unsnappedTransform.parent = null;
        Focus.ForceFocus = false;
    }

    private void UpdatePinchPoint()
    {
        targetPosition = unsnappedTransform.position;
        targetRotation = unsnappedTransform.rotation;
    }

    private bool GetShouldStartGrab(bool pinching)
    {
        return pinching && !wasPinching && FocusManager.Instance.FocusedItem == Focus;
    }

    public void StartGrab()
    {
        CurrentlyRepositioning = true;
        unsnappedTransform.position = transform.position;
        unsnappedTransform.rotation = transform.rotation;
        unsnappedTransform.parent = MainPinchDetector.Instance.PinchPoint;

        Focus.ForceFocus = true;
    }
}
