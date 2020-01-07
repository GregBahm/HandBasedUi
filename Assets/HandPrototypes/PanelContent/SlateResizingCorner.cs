﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateResizingCorner : MonoBehaviour
{
    [SerializeField]
    private BoxFocusable focus;
    public BoxFocusable Focus { get { return focus; } }

    public Transform Slate;

    public bool IsGrabbed;
    public Vector2 ResizingPivot;
    
    public LineRenderer LineRender;
    private Material lineMat;
    private float lineHighlight;

    private bool lastShowVisuals;
    public bool ShowVisuals;

    private float cornerScale;
    private float cornerOpacity;

    public Transform Anchor { get; private set; }

    public bool JustStartedShowing { get; private set; }

    private void Start()
    {
        Anchor = new GameObject("RisizingCornerAnchor").transform;
        Anchor.SetParent(Slate, false);
        Anchor.localPosition = new Vector3(ResizingPivot.x / 2, ResizingPivot.y / 2, 0);
        lineMat = LineRender.material;
    }

    private void Update()
    {
        JustStartedShowing = ShowVisuals && !lastShowVisuals;
        LineRender.enabled = ShowVisuals;
        UpdateCornerTransitions();
        UpdateMaterials();
        lastShowVisuals = ShowVisuals;
    }

    private void UpdateMaterials()
    {
        float lineHighlightTarget = IsGrabbed ? 1 : 0;
        lineHighlight = Mathf.Lerp(lineHighlight, lineHighlightTarget, Time.deltaTime * 15);
        lineMat.SetFloat("_Highlight", lineHighlight);
    }

    private void UpdateCornerTransitions()
    {
        float target = ShowVisuals ? 1 : 0;
        cornerScale = Mathf.Lerp(cornerScale, target, Time.deltaTime * 15);
        LineRender.transform.localScale = new Vector3(cornerScale, cornerScale, cornerScale);
    }
}
