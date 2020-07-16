using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private Transform leftRing;
    private LineRenderer leftRingLineRenderer;
    [SerializeField]
    private Transform rightRing;
    private LineRenderer rightRingLineRenderer;


    private float ringMaxSeperation;

    [SerializeField]
    private Transform grabberLocation;

    [SerializeField]
    private Vector3 grabberLocationOffset;

    private TextMeshPro iconText;
    [SerializeField]
    private Transform icon;
    [SerializeField]
    private Transform iconPivot;

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
        leftRingLineRenderer = leftRing.gameObject.GetComponent<LineRenderer>();
        rightRingLineRenderer = rightRing.gameObject.GetComponent<LineRenderer>();

        Setup(leftRingLineRenderer);
        Setup(rightRingLineRenderer);

        ringMaxSeperation = leftRing.localPosition.x;
        iconText = icon.GetComponent<TextMeshPro>();
    }

    private void Setup(LineRenderer lineRenderer)
    {
        const int positionsCount = 32;
        lineRenderer.positionCount = positionsCount;
        Vector3[] positions = new Vector3[positionsCount];
        for (int i = 0; i < positionsCount; i++)
        {
            float param = (float)i / positionsCount;
            param *= Mathf.PI * 2;
            float x = Mathf.Cos(param) * .01f;
            float y = Mathf.Sin(param) * .01f;
            positions[i] = new Vector3(0, y, x);
        }
        lineRenderer.SetPositions(positions);
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
        UpdateIconRotation();
        UpdateRingVisuals();
        wasGrabbed = IsGrabbed;
    }

    private void UpdateRingVisuals()
    {
        RotateRing(leftRing);
        RotateRing(rightRing);

        Vector3 toCamera = (transform.position - Camera.main.transform.position).normalized;
        float leftShine = Vector3.Dot(-leftRing.right, toCamera);
        float rightShine = Vector3.Dot(rightRing.right, toCamera);

        leftRingLineRenderer.material.SetFloat("_Shine", leftShine);
        rightRingLineRenderer.material.SetFloat("_Shine", rightShine);
    }

    private void RotateRing(Transform ring)
    {
        Vector3 ringToCamera = (Camera.main.transform.position - ring.position);
        Vector3 projected = Vector3.ProjectOnPlane(ringToCamera, ring.right);
        Vector3 up = Vector3.Cross(projected, ring.right);
        ring.LookAt(ring.position + projected, up);
    }

    private void UpdateIconRotation()
    {
        iconPivot.LookAt(Camera.main.transform);
        iconPivot.Rotate(0, 180, 0);
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
        }
        else
        {
            SetMainPosition();

            this.movingContent.SetParent(transform);
            this.movingContent.localPosition = Vector3.zero;
            this.movingContent.localRotation = Quaternion.Lerp(this.movingContent.localRotation, Quaternion.identity, Time.deltaTime * 4);
            UpdateUnpinchedRotation();
        }
        this.movingContent.localPosition = Vector3.Lerp(this.movingContent.localPosition, Vector3.zero, Time.deltaTime * 2);
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

    private Quaternion rightLook = Quaternion.Euler(0, 90, 0);

    private void UpdateShowness()
    {
        bool shouldShow = FocusManager.Instance.FocusedItem == focus;
        float shownessTarget = shouldShow ? 1 : 0;
        showness = Mathf.Lerp(showness, shownessTarget, Time.deltaTime * 10);

        rotatingContent.localRotation = Quaternion.Lerp(rightLook, rotatingContent.localRotation, showness);
        rightRingLineRenderer.material.SetFloat("_Fade", showness);
        leftRingLineRenderer.material.SetFloat("_Fade", showness);
        iconText.color = new Color(1, 1, 1, showness);
    }

    public void SetTo(GrabberVisualController grabber)
    {
        movingContent.position = grabber.movingContent.position;
        rotatingContent.rotation = grabber.rotatingContent.rotation;
    }
}
