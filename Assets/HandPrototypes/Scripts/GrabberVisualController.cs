using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberVisualController : MonoBehaviour
{
    [SerializeField]
    private ScreenspaceFocusable focus;

    [SerializeField]
    private Transform movingContent;

    [SerializeField]
    private Transform scalingContent;

    [SerializeField]
    private Transform rotatingContent;

    [SerializeField]
    private SkinnedMeshRenderer grabberMesh;

    [SerializeField]
    private Transform grabberLocation;

    [SerializeField]
    private Vector3 grabberLocationOffset;

    [SerializeField]
    private Transform icon;

    [SerializeField]
    private AudioSource grabSound;
    [SerializeField]
    private AudioSource grabReleaseSound;

    float showness;

    public float Pinchedness { get; private set; }

    private bool wasGrabbed;
    public bool IsGrabbed { get { return focus.ForceFocus; } }
    
    public void SetGrabberLocation(Transform location, Vector3 offset)
    {
        this.grabberLocation = location;
        this.grabberLocationOffset = offset;
    }

    void Update()
    {
        Pinchedness = GetPinchedness();
        UpdateShowness();
        UpdateMainPosition();
        grabberMesh.SetBlendShapeWeight(0, Pinchedness * 100);
        UpdateIcon();
        if (!IsGrabbed && wasGrabbed)
        {
            EndGrab();
        }
        wasGrabbed = IsGrabbed;
    }

    private void UpdateIcon()
    {
        icon.LookAt(Camera.main.transform);
        icon.Rotate(0, 180, 0);
    }

    private void StartGrab()
    {
        movingContent.SetParent(MainPinchDetector.Instance.PinchPoint);
        grabSound.Play();
    }

    private void UpdateMainPosition()
    {
        if (IsGrabbed)
        {
            if(!wasGrabbed)
            {
                StartGrab();
            }
            this.movingContent.localPosition *= .9f; // Lerp to the pinch point
        }
        else
        {
            SetMainPosition();

            this.movingContent.SetParent(transform);
            this.movingContent.localPosition = Vector3.zero;
            this.movingContent.localRotation = Quaternion.identity;
            UpdateUnpinchedRotation();
        }
    }

    private void EndGrab()
    {
        grabReleaseSound.Play();
    }

    private void SetMainPosition()
    {
        Vector3 offset = grabberLocation.TransformDirection(grabberLocationOffset);
        transform.position = grabberLocation.position + offset;
        transform.rotation = grabberLocation.rotation;
    }

    private void UpdateUnpinchedRotation()
    {
        Quaternion handAlignmentRotation = GetHandAlignmentRotation();

        rotatingContent.rotation = Quaternion.Lerp(rotatingContent.rotation, GetHandAlignmentRotation(), Time.deltaTime * 5);
    }

    private Quaternion GetHandAlignmentRotation()
    {
        Vector3 forwardDir = (transform.position - MainPinchDetector.Instance.PalmProxy.position).normalized;
        Vector3 indexPos = MainPinchDetector.Instance.ThumbProxy.position;
        Vector3 thumbPos = MainPinchDetector.Instance.FingertipProxy.position;
        Vector3 cross = (indexPos - thumbPos).normalized;
        Vector3 up = Vector3.Cross(forwardDir, cross);
        return Quaternion.LookRotation(forwardDir, up);
    }

    private float GetPinchedness()
    {
        if (FocusManager.Instance.FocusedItem != focus)
        {
            return 0;
        }
        if (IsGrabbed)
        {
            return 1;
        }
        if (MainPinchDetector.Instance.Pinching && !IsGrabbed)
        {
            return 0;
        }
        float pinchProg = (MainPinchDetector.Instance.FingerDistance - .03f) / MainPinchDetector.Instance.PinchDist;
        return 1 - Mathf.Clamp01(pinchProg);
    }

    private void UpdateShowness()
    {
        bool shouldShow = FocusManager.Instance.FocusedItem == focus;
        float shownessTarget = shouldShow ? 1 : 0;
        showness = Mathf.Lerp(showness, shownessTarget, Time.deltaTime * 10);

        scalingContent.localScale = new Vector3(showness, showness, showness);
    }

    public void SetTo(GrabberVisualController grabber)
    {
        movingContent.position = grabber.movingContent.position;
        rotatingContent.rotation = grabber.rotatingContent.rotation;
    }
}
