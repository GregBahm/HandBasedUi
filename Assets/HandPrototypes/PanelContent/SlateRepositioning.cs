using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateRepositioning : MonoBehaviour
{
    [SerializeField]
    private ScreenspaceFocusable focus;
    public IFocusableItem Focus { get { return this.focus; } }

    [SerializeField]
    private float deadZoneMoveDistance;

    [SerializeField]
    private float snapRotateDistance;

    public float Smoothing;
    private bool wasPinching;

    private Vector3 pinchStartPoint;
    private Quaternion originalRotation;
    private float currentSnapPower;

    public bool CurrentlyRepositioning { get { return Focus.ForceFocus; } }

    private Transform positionTarget;
    private Transform positionTargetOffset;
    private Transform rotationHelper;

    private void Start()
    {
        positionTarget = new GameObject(gameObject.name + "Position Target").transform;
        positionTargetOffset = new GameObject(gameObject.name + "Position Target Offset").transform;
        positionTarget.parent = positionTargetOffset;
        positionTarget.position = transform.position;


        rotationHelper = new GameObject(gameObject.name + "Rotation Helper").transform;
        rotationHelper.position = transform.position;
    }

    public void DoUpdate()
    {
        bool pinching = MainPinchDetector.Instance.Pinching;
        if(CurrentlyRepositioning)
        {
            if(pinching)
            {
                UpdateTargetPosition();
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

    private void UpdateTargetPosition()
    {
        positionTargetOffset.position = MainPinchDetector.Instance.PinchPoint.position;

        rotationHelper.position = transform.position;
        rotationHelper.LookAt(Camera.main.transform);
        rotationHelper.Rotate(0, 180, 0);
    }


    private void UpdatePosition()
    {
        Vector3 deadzoneTarget = GetDeadzoneTarget();
        UpdateSnapPower();
        Quaternion targetRotation = GetTargetRotation();
        transform.position = Vector3.Lerp(transform.position, deadzoneTarget, Time.deltaTime * Smoothing);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * Smoothing);
    }

    private Quaternion GetTargetRotation()
    {
        float snapness = Mathf.Clamp01(currentSnapPower / snapRotateDistance);
        return Quaternion.Lerp(originalRotation, rotationHelper.rotation, snapness);
    }

    private void UpdateSnapPower()
    {
        float dragDist = (MainPinchDetector.Instance.PinchPoint.position - pinchStartPoint).magnitude;
        currentSnapPower = Mathf.Max(currentSnapPower, dragDist);
    }

    private Vector3 GetDeadzoneTarget()
    {
        Vector3 toTarget = positionTarget.position - transform.position;
        float distToTarget = toTarget.magnitude;
        float deadDist = Mathf.Max(0, distToTarget - deadZoneMoveDistance);
        return transform.position + toTarget.normalized * deadDist;
    }

    private void EndRepositioning()
    {
        Focus.ForceFocus = false;
    }
    
    private bool GetShouldStartGrab(bool pinching)
    {
        return pinching && !wasPinching && FocusManager.Instance.FocusedItem == Focus;
    }

    public void StartGrab()
    {
        pinchStartPoint = MainPinchDetector.Instance.PinchPoint.position;
        originalRotation = transform.rotation;
        currentSnapPower = 0;

        positionTargetOffset.position = MainPinchDetector.Instance.PinchPoint.position;
        positionTarget.position = transform.position;

        Focus.ForceFocus = true;
    }
}
