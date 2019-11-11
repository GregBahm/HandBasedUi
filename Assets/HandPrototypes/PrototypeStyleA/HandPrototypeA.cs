﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeA : StandardHandPrototype
{
    public float HoverDist = .2f;
    public float FingerRadius = 0.03f;

    private HandPrototypeProxies proxies;

    public RadialItemSet MainSet;
    public RadialItemSet ToolsSet;
    public HorizontalItemSet PanelsSet;

    public MenuItemButton ToolsButton;
    public MenuItemButton PanelsButton;

    public float Radius;

    public UiState State { get; private set; }

    public float BaseRotationOffset { get; private set; }

    public enum UiState
    {
        Unsummoned,
        Summoned,
        BrowsingTools,
        BrowsingWindows
    }

    private void Start()
    {
        proxies = HandPrototypeProxies.Instance;
        ToolsButton.Released += OnToolsButtonReleased;
        PanelsButton.Released += OnPanelsButtonReleased;
    }

    private void OnPanelsButtonReleased(object sender, EventArgs e)
    {
        if (State == UiState.BrowsingWindows)
        {
            State = UiState.Summoned;
        }
        else
        {
            State = UiState.BrowsingWindows;
        }
    }

    private void OnToolsButtonReleased(object sender, EventArgs e)
    {
        if (State == UiState.BrowsingTools)
        {
            State = UiState.Summoned;
        }
        else
        {
            State = UiState.BrowsingTools;
        }
    }

    private void Update()
    {
        UpdatePrimaryVisibility();
        UpdatePosition();
        UpdateBaseRotation();


        if (IsSummoned && State == UiState.Unsummoned)
        {
            State = UiState.Summoned;
        }
        if(!IsSummoned)
        {
            State = UiState.Unsummoned;
        }
        
        MainSet.ShowItems = State != UiState.Unsummoned;
        ToolsSet.ShowItems = State == UiState.BrowsingTools;
        PanelsSet.ShowItems = State == UiState.BrowsingWindows;

        ToolsButton.Toggled = State == UiState.BrowsingTools;
        PanelsButton.Toggled = State == UiState.BrowsingWindows;
    }

    private void UpdateBaseRotation()
    {
        float target = GetBaseRotationOffsetTarget();
        BaseRotationOffset = Mathf.Lerp(BaseRotationOffset, target, Time.deltaTime * 15);
    }

    public float magicNumber = 180;

    private float GetBaseRotationOffsetTarget()
    {
        Vector3 palmPoint = proxies.LeftPalm.position;
        Vector3 fingerTipAverage = GetFingerTipAverage();
        
        Vector3 handVector = (fingerTipAverage - palmPoint).normalized;
        return handVector.y * magicNumber;
    }

    private Vector3 GetFingerTipAverage()
    {
        return (HandPrototypeProxies.Instance.LeftIndex.position
               + HandPrototypeProxies.Instance.LeftRing.position
               + HandPrototypeProxies.Instance.LeftMiddle.position
               + HandPrototypeProxies.Instance.LeftPinky.position) / 4;
    }

    private Quaternion GetItemRotation()
    {
        Vector3 toCamera = Camera.main.transform.forward;
        Vector3 skyTangent = Vector3.Cross(toCamera, Vector3.up);
        Vector3 personalUp = Vector3.Cross(toCamera, skyTangent);
        return Quaternion.LookRotation(proxies.LeftPalm.up, -personalUp);
    }
}
