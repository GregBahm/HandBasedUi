using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereButton : MonoBehaviour
{
    public PointFocusable Focus;
    public bool IsDisabled;
    public ButtonInteractionStyles InteractionStyle;
    public bool Toggled;

    private bool clickable;
    public event EventHandler Pressed;
    public event EventHandler Released;

    public ButtonStyling Styling;

    private ButtonState state;
    
    private float fingerDistance;
    
    public Renderer SphereRenderer;
    private Material sphereMeshMat;

    public Transform ButtonContent;

    public Color CurrentColor { get; private set; }

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
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        Color colorTarget = GetStateColor();
        CurrentColor = Color.Lerp(CurrentColor, colorTarget, Time.deltaTime * 15);

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

    private bool IsHoveringOver(float fingerDist)
    {
        float radius = transform.lossyScale.x;
        return fingerDist < (Styling.HoverRadius + radius);
    }

    private float GetFingerDist()
    {
        Vector3 fingerTipPos = HandPrototypeProxies.Instance.RightIndex.position;
        return (transform.position - fingerTipPos).magnitude;
    }

    private bool IsHoveringUnder(float fingerDist)
    {
        float radius = transform.lossyScale.x;
        return fingerDist < radius;
    }

    private void UpdateInteraction()
    {
        if(IsDisabled)
        {
            state = ButtonState.Disabled;
        }
        else
        {
            state = FocusManager.Instance.FocusedItem == Focus ? ButtonState.Hovered : ButtonState.Ready;
        }
        //else if (state == ButtonState.Disabled)
        //{
        //    state = ButtonState.Ready;
        //}
        //float fingerDist = GetFingerDist();
        //if(state == ButtonState.Pressing)
        //{
        //    if(!IsHoveringUnder(fingerDist))
        //    {
        //        OnRelease();
        //    }
        //}
        //if(state == ButtonState.Hovered)
        //{
        //    if (IsHoveringUnder(fingerDist))
        //    {
        //        OnPress();
        //    }
        //    else if (!IsHoveringOver(fingerDist))
        //    {
        //        state = ButtonState.Ready;
        //    }
        //}
        //if(state == ButtonState.Ready)
        //{
        //    if(IsHoveringOver(fingerDist))
        //    {
        //        state = ButtonState.Hovered;
        //    }
        //}
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
