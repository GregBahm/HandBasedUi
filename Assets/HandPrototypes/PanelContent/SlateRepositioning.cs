using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateRepositioning : MonoBehaviour
{
    [SerializeField]
    private BoxFocusable focus;
    public BoxFocusable Focus { get { return this.focus; } }
    
    [SerializeField]
    private AudioSource grabSound;
    [SerializeField]
    private AudioSource grabReleaseSound;

    public float Smoothing;
    public float SnapThreshold;
    private bool wasPinching;

    [SerializeField]
    private float InteractionCooldownDuration;// Just after a click, disable grab for a bit to reduce false grabs
    private float currentInteractionCooldown;

    public bool CurrentlyRepositioning { get; private set; }
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

    public void ResetInteractionCooldown()
    {
        currentInteractionCooldown = InteractionCooldownDuration;
    }

    public void OnEndResizing()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
        grabReleaseSound.Play();
    }

    public void UpdateSlatePositioning()
    {
        UpdateCooldowns();
        bool pinching = MainPinchDetector.Instance.Pinching;
        if(CurrentlyRepositioning)
        {
            if(pinching)
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

    private void UpdateCooldowns()
    {
        currentInteractionCooldown -= Time.deltaTime;
        if(currentInteractionCooldown > 0)
        {
            CurrentlyRepositioning = false;
        }
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
        Focus.ForceFocus = false;
        grabReleaseSound.Play();
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
        return pinching && !wasPinching && FocusManager.Instance.FocusedItem == Focus && currentInteractionCooldown < 0;
    }

    public void StartGrab()
    {
        CurrentlyRepositioning = true;
        unsnappedTransform.position = transform.position;
        unsnappedTransform.rotation = transform.rotation;
        unsnappedTransform.parent = MainPinchDetector.Instance.PinchPoint;

        snappedTransform.position = transform.position;
        snappedTransform.rotation = transform.rotation;
        snappedTransform.parent = MainPinchDetector.Instance.PinchPoint;
        Focus.ForceFocus = true;

        grabSound.Play();
    }

    internal void OnStartResizing()
    {
        grabSound.Play();
    }
}
