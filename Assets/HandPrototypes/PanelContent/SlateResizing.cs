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
    private Vector3 pinchHandStartPos;
    private Vector3 pinchCornerStartPos;

    public SlateResizingCorner HoveredCorner { get; private set; }

    [SerializeField]
    private AudioSource resizingHoverSound;

    private void Start()
    {
        pivotPoint = new GameObject("Resizing PivotPoint").transform;
        corners = new SlateResizingCorner[] { LowerLeftCorner, LowerRightCorner, UpperLeftCorner, UpperRightCorner };
    }

    public void UpdateSlateResizing()
    {
        if (CurrentlyResizing)
        {
            if (MainPinchDetector.Instance.Pinching)
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
            UpdateHoveredCorner();
            if (ShouldStartGrab())
            {
                StartGrab(HoveredCorner);
            }
        }
        UpdateCornerVisibility();
        UpdateCornerSounds();
        PositionCorners();
    }

    private void UpdateCornerSounds()
    {
        if(corners.Any(item => item.JustStartedShowing))
        {
            resizingHoverSound.Play();
        }
    }

    private void UpdateHoveredCorner()
    {
        HoveredCorner = corners.FirstOrDefault(corner => corner.Focus == FocusManager.Instance.FocusedItem);
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
        Vector3 grabOffset = grabPoint - pinchHandStartPos;
        Vector3 effectiveGrabPoint = pinchCornerStartPos + grabOffset;

        Vector2 newScale = GetScale(effectiveGrabPoint);

        pivotPoint.localScale = new Vector3(newScale.x, newScale.y, 1);
        transform.position = Slate.position;
        Slate.parent = transform;
    }

    private Vector2 GetScale(Vector3 grabPoint)
    {
        Plane rightPlane = new Plane(pivotPoint.right, pivotPoint.position);
        Vector3 rightProjection = rightPlane.ClosestPointOnPlane(grabPoint);
        float xDist = (grabPoint - rightProjection).magnitude;
        
        Plane upPlane = new Plane(pivotPoint.up, pivotPoint.position);
        Vector3 upProjection = upPlane.ClosestPointOnPlane(grabPoint);
        float yDist = (grabPoint - upProjection).magnitude;

        float clampedX = Mathf.Clamp(Mathf.Abs(xDist), MinWidth, MaxWidth);
        float clampedy = Mathf.Clamp(Mathf.Abs(yDist), MinHeight, MaxHeight);

        return new Vector2(clampedX, clampedy);
    }

    private void EndGrab()
    {
        foreach (SlateResizingCorner corner in corners)
        {
            corner.Focus.ForceFocus = false;
            corner.IsGrabbed = false;
        }
        Main.Repositioning.OnEndResizing();
    }

    private bool ShouldStartGrab()
    {
        return MainPinchDetector.Instance.PinchBeginning && HoveredCorner != null;
    }

    private void StartGrab(SlateResizingCorner grabbedCorner)
    {
        grabbedCorner.Focus.ForceFocus = true;
        grabbedCorner.IsGrabbed = true;
        
        pivotPoint.SetParent(Slate);
        pivotPoint.localPosition = - grabbedCorner.ResizingPivot / 2;
        pivotPoint.localRotation = Quaternion.identity;
        pivotPoint.SetParent(null);
        pivotPoint.localScale = Slate.localScale;

        pinchHandStartPos = MainPinchDetector.Instance.PinchPoint.position;
        pinchCornerStartPos = grabbedCorner.Anchor.position;
        Main.Repositioning.OnStartResizing();
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