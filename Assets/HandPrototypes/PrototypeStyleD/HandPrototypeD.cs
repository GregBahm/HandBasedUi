using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeD : StandardHandPrototype
{
    public MeshCollider ButtonHoverSpace;
    public MeshCollider PanelHoverCore;
    public MeshCollider PanelHoverFull;

    public MorphingThumbnail[] Thumbnails;
    
    public StyleDButton[] ToolButtons;

    public UiState state;
    private float diskExpandedness;
    private float panelExpandedness;
    public float TransitionTime;
    public float ThumbTransitionArc;
    public float PanelTransitionArc;

    public enum UiState
    {
        Ready,
        ButtonsHovered,
        PanelsHovered
    }

    private void Update()
    {
        UpdatePrimaryVisibility();
        UpdatePosition();
        UpdateState();
        UpdateThumbs();
        UpdateButtons();
        UpdateButtonRenderers();
    }

    private void UpdateButtons()
    {
        foreach (StyleDButton item in ToolButtons)
        {
            bool isDisabled = state == UiState.PanelsHovered;
            item.Button.IsDisabled = isDisabled;
            item.LabelText.color = state == UiState.ButtonsHovered ? Color.white : Color.black;
            item.IconText.color = isDisabled ? Color.gray : Color.white;
        }
    }

    private void UpdateButtonRenderers()
    {
        float expandednessTarget = state == UiState.ButtonsHovered ? 1 : -1;
        diskExpandedness += expandednessTarget * Time.deltaTime;
        diskExpandedness = Mathf.Clamp(diskExpandedness, 0, TransitionTime);

        float param = diskExpandedness / TransitionTime;
        param = Mathf.Pow(param, ThumbTransitionArc);
        float effectiveVal = 100 - (param * 100);
        foreach (StyleDButton item in ToolButtons)
        {
            item.Morpher.SetBlendShapeWeight(0, effectiveVal);
        }
    }

    private void UpdateThumbs()
    {
        float expandednessTarget = state == UiState.PanelsHovered ? 1 : -1;
        panelExpandedness += expandednessTarget * Time.deltaTime;
        panelExpandedness = Mathf.Clamp(panelExpandedness, 0, TransitionTime);

        float param = panelExpandedness / TransitionTime;
        param = Mathf.Pow(param, PanelTransitionArc);
        foreach (MorphingThumbnail item in Thumbnails)
        {
            item.Morph(param);
        }
    }

    private bool htest;

    private void UpdateState()
    {
        bool buttonsHovered = GetIsHovered(ButtonHoverSpace);
        htest = buttonsHovered;
        if (state == UiState.ButtonsHovered)
        {
            state = buttonsHovered ? UiState.ButtonsHovered : UiState.Ready;
        }
        if(state == UiState.PanelsHovered)
        {
            if(MainPinchDetector.Instance.Pinching)
            {
                return;
            }
            bool fullPanelsHovered = GetIsHovered(PanelHoverFull);
            state = fullPanelsHovered ? UiState.PanelsHovered : UiState.Ready;
        }
        if(state == UiState.Ready)
        {
            bool corePanelsHovered = GetIsHovered(PanelHoverFull);
            state = corePanelsHovered ? UiState.PanelsHovered : (buttonsHovered ? UiState.ButtonsHovered : UiState.Ready);
        }
    }

    private bool GetIsHovered(MeshCollider collider)
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(HandPrototypeProxies.Instance.RightIndex.position - collider.transform.forward, collider.transform.forward);
        return collider.Raycast(ray, out hitInfo, float.PositiveInfinity);
    }
}
