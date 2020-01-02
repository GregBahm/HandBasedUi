using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereButton : MonoBehaviour
{
    public bool IsDisabled;
    public ButtonInteractionStyles InteractionStyle;
    public bool Toggled;

    private bool clickable;
    public event EventHandler Pressed;
    public event EventHandler Released;

    public ButtonStyling Styling;

    private ButtonState state;

    public MeshCollider Backdrop;
    public MeshCollider BackwardsBackdrop;
    private float fingerDistance;

    public Renderer BackgroundRenderer;
    public Renderer SphereRenderer;
    private Material quadMeshMat;
    private Material sphereMeshMat;

    public Transform ButtonContent;

    private const float BaseDist = -0.08f;
    private const float HoverDist = -0.2f;
    private const float PressDist = 0.08f;

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
        quadMeshMat = BackgroundRenderer.material;
        sphereMeshMat = SphereRenderer.material;
    }

    private void Update()
    {
        UpdateInteraction();
        UpdateMaterial();
        UpdatePositions();
    }

    private void UpdatePositions()
    {
        float buttonContentTarget = state == ButtonState.Hovered ? HoverDist : (state == ButtonState.Pressing ? 0 : BaseDist);
        float newButtonZ = Mathf.Lerp(ButtonContent.localPosition.z, buttonContentTarget, Time.deltaTime * 50);
        ButtonContent.localPosition = new Vector3(ButtonContent.localPosition.x, ButtonContent.localPosition.y, newButtonZ);

        float backdropTarget = state == ButtonState.Pressing ? PressDist : 0;
        float newBackdropZ = Mathf.Lerp(BackgroundRenderer.transform.localPosition.z, backdropTarget, Time.deltaTime * 50);
        BackgroundRenderer.transform.localPosition = new Vector3(BackgroundRenderer.transform.localPosition.x, BackgroundRenderer.transform.localPosition.y, newBackdropZ);
    }

    private void UpdateMaterial()
    {
        Color colorTarget = GetStateColor();
        CurrentColor = Color.Lerp(CurrentColor, colorTarget, Time.deltaTime * 15);

        sphereMeshMat.SetColor("_Color", CurrentColor);
        quadMeshMat.SetColor("_Color", CurrentColor);
        sphereMeshMat.SetFloat("_Disabledness", state == ButtonState.Disabled ? 1 : 0);
        quadMeshMat.SetFloat("_Disabledness", state == ButtonState.Disabled ? 1 : 0);
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
        else if (state == ButtonState.Disabled)
        {
            state = ButtonState.Ready;
        }
        float fingerDist = GetFingerDist();
        if(state == ButtonState.Pressing)
        {
            if(!IsHoveringUnder(fingerDist))
            {
                OnRelease();
            }
        }
        if(state == ButtonState.Hovered)
        {
            if (IsHoveringUnder(fingerDist))
            {
                OnPress();
            }
            else if (!IsHoveringOver(fingerDist))
            {
                state = ButtonState.Ready;
            }
            else
            {
                //Manager.RegisterHover(hoverInfo.point);
            }
        }
        if(state == ButtonState.Ready)
        {
            if(IsHoveringOver(fingerDist))
            {
                state = ButtonState.Hovered;
                //Manager.RegisterHover(hoverInfo.point);
            }
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
