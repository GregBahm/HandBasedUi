using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlateResizing : MonoBehaviour
{
    public bool CurrentlyResizing
    {
        get
        {
            return LowerLeftCorner.IsGrabbed
                || LowerRightCorner.IsGrabbed
                || UpperLeftCorner.IsGrabbed
                || UpperRightCorner.IsGrabbed;
        }
    }
    public MainPanelArrangement Main;
    private Transform pivotPoint;
    
    public Transform Slate;

    public SlateResizingCorner LowerLeftCorner;
    public SlateResizingCorner LowerRightCorner;
    public SlateResizingCorner UpperLeftCorner;
    public SlateResizingCorner UpperRightCorner;

    public float MinWidth;
    public float MaxWidth;
    public float MinHeight;
    public float MaxHeight;

    public float ResizingMargin;

    private IEnumerable<SlateResizingCorner> corners;
    private bool wasPinching;
    private Vector3 pinchStartPos;

    public SlateResizingCorner HoveredCorner { get; private set; }
    public float CornerGrabMargin = .1f;

    private void Start()
    {
        pivotPoint = new GameObject("Resizing PivotPoint").transform;
        corners = new SlateResizingCorner[] { LowerLeftCorner, LowerRightCorner, UpperLeftCorner, UpperRightCorner };
    }

    public void UpdateSlateResizing()
    {
        bool pinching = MainPinchDetector.Instance.Pinching;

        if (CurrentlyResizing)
        {
            if (pinching)
            {
                DoGrabUpdate();
            }
            else
            {
                EndGrab();
            }
        }
        else
        {
            HoveredCorner = GetHoveredCorner();
            if (ShouldStartGrab(pinching))
            {
                StartGrab(HoveredCorner);
            }
        }
        UpdateCornerVisibility();
        PositionCorners();
        wasPinching = pinching;
    }

    private void UpdateCornerVisibility()
    {
        bool isAvailable = Main.Summonness > .9f;
        foreach (SlateResizingCorner item in corners)
        {
            item.ShowVisuals = HoveredCorner == item;
            item.gameObject.SetActive(isAvailable);
        }
    }

    private void DoGrabUpdate()
    {
        Slate.parent = pivotPoint;
        Vector3 grabPoint = MainPinchDetector.Instance.PinchPoint.position;

        Vector3 relativeStartPoint = pivotPoint.worldToLocalMatrix * new Vector4(pinchStartPos.x, pinchStartPos.y, pinchStartPos.z, 1);
        Vector3 relativeGrabPoint = pivotPoint.worldToLocalMatrix * new Vector4(grabPoint.x, grabPoint.y, grabPoint.z, 1);
        Vector3 clampedGrabPoint = GetClampedGrabPoint(relativeGrabPoint);

        float xRatio = clampedGrabPoint.x / relativeStartPoint.x;
        float yRatio = clampedGrabPoint.y / relativeStartPoint.y;
        pivotPoint.localScale = new Vector3(xRatio, yRatio, 1);
        transform.position = Slate.position;
        Slate.parent = transform;
    }

    private Vector3 GetClampedGrabPoint(Vector3 grabPoint)
    {
        Vector3 test = pivotPoint.worldToLocalMatrix * new Vector4(grabPoint.x, grabPoint.y, grabPoint.z, 0);
        Debug.Log("Test: " + test.y );
        return grabPoint;
        //float clampedX = Mathf.Clamp(grabPoint.x, MinWidth, MaxWidth);
        //float clampedY = Mathf.Clamp(grabPoint.y, MinHeight, MaxHeight);
        //return new Vector3(clampedX, clampedY, grabPoint.z);
    }

    private void EndGrab()
    {
        foreach (SlateResizingCorner corner in corners)
        {
            corner.IsGrabbed = false;
        }
        Main.Repositioning.OnEndResizing();
    }

    private bool ShouldStartGrab(bool pinching)
    {
        return pinching && !wasPinching && HoveredCorner != null;
    }

    private SlateResizingCorner GetHoveredCorner()
    {
        Vector3 grabPoint = MainPinchDetector.Instance.PinchPoint.position;
        float closestGrabDist = CornerGrabMargin;
        SlateResizingCorner ret = null;
        foreach (SlateResizingCorner corner in corners)
        {
            Vector3 closestPoint = corner.GrabBox.ClosestPoint(grabPoint);
            float grabDist = (grabPoint - closestPoint).magnitude;
            if (grabDist < closestGrabDist)
            {
                closestGrabDist = grabDist;
                ret = corner;
            }
        }
        return ret;
    }

    private void StartGrab(SlateResizingCorner grabbedCorner)
    {
        grabbedCorner.IsGrabbed = true;
        
        pivotPoint.SetParent(Slate);
        pivotPoint.localPosition = - grabbedCorner.ResizingPivot / 2;
        pivotPoint.localRotation = Quaternion.identity;
        pivotPoint.SetParent(null);
        pivotPoint.localScale = Vector3.one;
        
        pinchStartPos = MainPinchDetector.Instance.PinchPoint.position;
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