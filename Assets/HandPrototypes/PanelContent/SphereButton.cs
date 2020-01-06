using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereButton : MonoBehaviour
{
    public SphereButtonRailing Rail;
    public PointFocusable Focus;
    public bool IsDisabled;
    public ButtonInteractionStyles InteractionStyle;
    public bool Toggled;

    public float RailHeight;
    public event EventHandler Pressed;
    public event EventHandler Released;

    public ButtonStyling Styling;

    private ButtonState state;
    
    public Renderer SphereRenderer;
    private Material sphereMeshMat;

    public Transform ButtonContent;
    public Transform Icon;

    private float hoveredness;

    public Color CurrentColor { get; private set; }
    public Color CurrentGlowColor { get; private set; }

    private enum ButtonState
    {
        Disabled,
        Ready,
        Hovered,
        Pressing,
    }
    public enum ButtonInteractionStyles
    {
        ToggleButton,
        ClickButton
    }

    private void Start()
    {
        sphereMeshMat = SphereRenderer.material;
    }

    private void Update()
    {
        Focus.IsFocusable = !IsDisabled;
        UpdateInteraction();
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        UpdateMaterials();
        UpdateRailing();
        UpdateIconPosition();
    }

    private void UpdateIconPosition()
    {
        Vector3 fingerPos = HandPrototypeProxies.Instance.RightIndex.position;
        Vector3 toFinger = (ButtonContent.transform.position - fingerPos);
        float weight = 1 - Mathf.Clamp01(Mathf.Abs(toFinger.magnitude - .02f) * 50);
        Icon.localPosition = toFinger.normalized * weight * .05f * hoveredness;
        Icon.forward = Vector3.Lerp(transform.forward, -toFinger.normalized, weight * .3f * hoveredness); 
    }

    private void UpdateRailing()
    {
        float hoverednessTarget = state == ButtonState.Hovered ? 1 : 0;
        hoveredness = Mathf.Lerp(hoveredness, hoverednessTarget, Time.deltaTime * 15);
        Rail.VerticalOffset = RailHeight * hoveredness;
    }

    private void UpdateMaterials()
    {
        Color colorTarget = GetStateColor();
        CurrentColor = Color.Lerp(CurrentColor, colorTarget, Time.deltaTime * 15);
        Color targetGlowColor = colorTarget * (state == ButtonState.Hovered ? 1 : 0);
        CurrentGlowColor = Color.Lerp(CurrentGlowColor, targetGlowColor, Time.deltaTime * 15);

        sphereMeshMat.SetColor("_Color", CurrentColor);
        sphereMeshMat.SetFloat("_Disabledness", state == ButtonState.Disabled ? 1 : 0);

    }

    private Color GetStateColor()
    {
        switch (state)
        {
            case ButtonState.Ready:
                return Toggled ? Styling.ReadyToggledColor :Styling.ReadyColor;
            case ButtonState.Hovered:
                return Styling.HoverColor;
            case ButtonState.Pressing:
                return Styling.PressingColor;
            case ButtonState.Disabled:
            default:
                return Styling.DisabledColor;
        }
    }

    private float hoverStartHeight;

    private void UpdateInteraction()
    {
        ButtonState oldState = state;
        if(IsDisabled)
        {
            state = ButtonState.Disabled;
        }
        else
        {
            state = FocusManager.Instance.FocusedItem == Focus ? ButtonState.Hovered : ButtonState.Ready;
        }

        if(state == ButtonState.Hovered)
        {
            float fingerHeight = HandPrototypeProxies.Instance.RightIndex.position.y; // TODO: Make this in localspace instead of worldspace
            if (oldState == ButtonState.Ready)
            {
                hoverStartHeight = fingerHeight;
            }
            float riseAmount = fingerHeight - hoverStartHeight;
            if(riseAmount > RailHeight)
            {
                OnRelease();
            }
            riseAmount = Mathf.Clamp(riseAmount / transform.lossyScale.y, 0, RailHeight);
            ButtonContent.localPosition = new Vector3(0, riseAmount, 0);
        }
        else
        {
            ButtonContent.localPosition = Vector3.Lerp(ButtonContent.localPosition, Vector3.zero, Time.deltaTime * 15);
        }
    }

    private void OnPress()
    {
        state = ButtonState.Pressing;
        //Manager.OnAnyButtonPress();
        Pressed?.Invoke(this, EventArgs.Empty);
    }

    private void OnRelease()
    {
        //Manager.OnAnyButtonRelease();
        state = ButtonState.Ready;
        if(InteractionStyle == ButtonInteractionStyles.ToggleButton)
        {
            Toggled = !Toggled;
        }
        Released?.Invoke(this, EventArgs.Empty);
    }
}
