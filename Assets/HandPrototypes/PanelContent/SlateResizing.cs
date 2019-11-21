using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlateResizing : MonoBehaviour
{
    private Transform pivotPoint;
    private Transform videoProxy;

    public Transform MasterPosition;
    public Transform Slate;
    public Transform Video;

    public SlateResizingCorner LowerLeftCorner;
    public SlateResizingCorner LowerRightCorner;
    public SlateResizingCorner UpperLeftCorner;
    public SlateResizingCorner UpperRightCorner;

    public float MinScale;
    public float MaxScale;

    public float ResizingMargin;

    private IEnumerable<SlateResizingCorner> corners;
    private bool wasPinching;
    private float startDistToPivot;
    
    private void Start()
    {
        pivotPoint = new GameObject("PivotPoint").transform;
        videoProxy = new GameObject("video Proxy").transform;
        videoProxy.SetParent(pivotPoint);
        corners = new SlateResizingCorner[] { LowerLeftCorner, LowerRightCorner, UpperLeftCorner, UpperRightCorner };
    }

    private void Update()
    {
        bool anyCornersGrabbed = corners.Any(item => item.IsGrabbed);
        if (!anyCornersGrabbed)
        {
            MaybeStartGrab();
        }
        else
        {
            HandleContinueGrab();
        }
        PositionCorners();
    }

    private void HandleContinueGrab()
    {
        bool pinching = MainPinchDetector.Instance.Pinching;
        if(pinching)
        {
            DoGrabUpdate();
        }
        else
        {
            EndGrab();
        }
    }

    private void DoGrabUpdate()
    {
        Vector3 grabPoint = MainPinchDetector.Instance.PinchPoint.position;
        float distToPivot = (grabPoint - pivotPoint.position).magnitude;
        distToPivot = Mathf.Clamp(distToPivot, MinScale, MaxScale);

        float ratio = distToPivot / startDistToPivot;
        pivotPoint.localScale = new Vector3(ratio, ratio, ratio);
        Video.localScale = videoProxy.lossyScale;
        MasterPosition.position = videoProxy.position;
    }

    private void EndGrab()
    {
        foreach (SlateResizingCorner corner in corners)
        {
            corner.IsGrabbed = false;
        }
    }

    private void MaybeStartGrab()
    {
        bool pinching = MainPinchDetector.Instance.Pinching;
        if(pinching && !wasPinching)
        {
            SlateResizingCorner corner = TryGetPinchedCorner();
            if(corner != null)
            {
                StartGrab(corner);
            }
        }
        wasPinching = pinching;
    }

    private SlateResizingCorner TryGetPinchedCorner()
    {
        Vector3 grabPoint = MainPinchDetector.Instance.PinchPoint.position;
        float closestGrabDist = float.PositiveInfinity;
        SlateResizingCorner ret = null;
        foreach (SlateResizingCorner corner in corners)
        {
            Vector3 closestPoint = corner.GrabBox.ClosestPoint(grabPoint);
            float grabDist = (grabPoint - closestPoint).magnitude;
            if(grabDist < closestGrabDist)
            {
                closestGrabDist = grabDist;
                ret = corner;
            }
        }
        return ret;
    }

    private void StartGrab(SlateResizingCorner grabbedCorner)
    {
        // Start a pinch
        grabbedCorner.IsGrabbed = true;

        float x = Slate.localScale.x / 2;
        x *= -grabbedCorner.ResizingPivot.x;
        float y = Slate.localScale.y / 2;
        y *= -grabbedCorner.ResizingPivot.y;

        pivotPoint.SetParent(MasterPosition.transform);
        pivotPoint.localPosition = new Vector3(x, y, 0);
        pivotPoint.SetParent(null);
        pivotPoint.localScale = Vector3.one;

        videoProxy.position = Video.position;
        videoProxy.localScale = Video.localScale;

        Vector3 pinchStartPos = MainPinchDetector.Instance.PinchPoint.position;
        startDistToPivot = (pinchStartPos - pivotPoint.position).magnitude;
    }

    private void PositionCorners()
    {
        PositionCorner(-1, -1, LowerLeftCorner);
        PositionCorner(-1, 1, UpperLeftCorner);
        PositionCorner(1, 1, UpperRightCorner);
        PositionCorner(1, -1, LowerRightCorner);
    }

    private void PositionCorner(int horizontal, int vertical, SlateResizingCorner corner)
    {
        float x = Slate.localScale.x / 2 + ResizingMargin;
        float y = Slate.localScale.y / 2 + ResizingMargin;
        Vector3 pos = new Vector3(x * horizontal, y * vertical, 0);
        corner.transform.localPosition = pos;
    }
}