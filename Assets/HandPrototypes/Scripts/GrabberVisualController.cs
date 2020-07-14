using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Need to figure out a way to seperate grabber controller from grabber visuals controller or something. This class name is inaccurate.
public class GrabberVisualController : MonoBehaviour
{
    [SerializeField]
    private ScreenspaceFocusable focus;
    public ScreenspaceFocusable Focus { get { return focus; } }

    [SerializeField]
    private Transform movingContent;

    [SerializeField]
    private Transform scalingContent;

    [SerializeField]
    private Transform rotatingContent;

    [SerializeField]
    private SkinnedMeshRenderer grabberMesh;

    [SerializeField]
    private Transform leftRing;
    [SerializeField]
    private Transform rightRing;
    private float ringMaxSeperation;

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

    private void Start()
    {
        ringMaxSeperation = leftRing.localPosition.x;
    }

    void Update()
    {
        Pinchedness = GetPinchedness();
        UpdateShowness();
        UpdateMainPosition();
        if (!IsGrabbed && wasGrabbed)
        {
            EndGrab();
        }
        leftRing.localPosition = new Vector3((1 - Pinchedness) * ringMaxSeperation, 0, 0);
        rightRing.localPosition = -leftRing.localPosition;
        grabberMesh.SetBlendShapeWeight(0, Pinchedness * 100);
        UpdateIcon();
        wasGrabbed = IsGrabbed;
    }

    private void UpdateIcon()
    {
        icon.LookAt(Camera.main.transform);
        icon.Rotate(0, 180, 0);
    }

    private void StartGrab()
    {
        movingContent.SetParent(MainPinchDetector.Instance.PinchPoint, true);
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
            return 1;
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
