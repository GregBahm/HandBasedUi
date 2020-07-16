using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateResizingCorner : MonoBehaviour
{
    [SerializeField]
    private ScreenspaceFocusable focus;
    public IFocusableItem Focus => focus;

    [SerializeField]
    private Transform icon;
    
    private Transform resizingPivot;
    private Transform resizingContent;

    public bool IsGrabbed { get; private set; }

    public Vector3 LocalSlatePosition { get; private set; }

    public void DoUpdate()
    {
        if(IsGrabbed)
        {
            if(MainPinchDetector.Instance.Pinching)
            {
                DoGrabUpdate();
            }
            else
            {
                EndGrab();
            }
        }
        else if(ShouldStartGrab())
        {
            StartGrab();
        }
    }
    
    private float resizeStartDistance;

    private Transform scalePivot;

    private Transform resizingContentBaseParent;
    
    public void Initialize(Transform resizingContent, Transform resizingPivot, float iconRotation, Vector3 localSlatePosition)
    {
        this.resizingContent = resizingContent;
        this.resizingPivot = resizingPivot;

        scalePivot = new GameObject("Scale Pivot").transform;
        scalePivot.parent = resizingContent.parent;

        icon.Rotate(new Vector3(0, 0, iconRotation));
        LocalSlatePosition = localSlatePosition;
    }

    private void DoGrabUpdate()
    {
        float scale = GetScale();
        scalePivot.localScale = new Vector3(scale, scale, scale);
    }

    private float GetScale()
    {
        Vector3 grabPoint = MainPinchDetector.Instance.PinchPoint.position;
        float resizeDistance = (scalePivot.position - grabPoint).magnitude;
        return resizeDistance / resizeStartDistance;
    }

    private void EndGrab()
    {
        focus.ForceFocus = false;
        IsGrabbed = false;
        resizingContent.parent = resizingContentBaseParent;
    }

    private void StartGrab()
    {
        focus.ForceFocus = true;
        IsGrabbed = true;
        
        scalePivot.position = resizingPivot.position;
        scalePivot.rotation = resizingPivot.rotation;
        scalePivot.localScale = Vector3.one;

        resizingContentBaseParent = resizingContent.parent;
        resizingContent.parent = scalePivot;

        Vector3 grabPoint = MainPinchDetector.Instance.PinchPoint.position;
        resizeStartDistance = (scalePivot.position - grabPoint).magnitude;
    }

    private bool ShouldStartGrab()
    {
        return MainPinchDetector.Instance.PinchBeginning
            && focus.Equals(FocusManager.Instance.FocusedItem);
    }
}
