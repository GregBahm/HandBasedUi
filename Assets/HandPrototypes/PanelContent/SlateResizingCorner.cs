using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateResizingCorner : MonoBehaviour
{
    public BoxCollider GrabBox;

    public bool IsGrabbed;
    public Vector2 ResizingPivot;
    
    public LineRenderer LineRender;

    public bool ShowVisuals;

    private float cornerScale;
    private float cornerOpacity;

    private void Update()
    {
        LineRender.enabled = ShowVisuals;
        UpdateCornerTransitions();
    }

    private void UpdateCornerTransitions()
    {
        float target = ShowVisuals ? 1 : 0;
        cornerScale = Mathf.Lerp(cornerScale, target, Time.deltaTime * 15);
        LineRender.transform.localScale = new Vector3(cornerScale, cornerScale, cornerScale);
    }
}
