using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SphereButton : MonoBehaviour
{
    public TextMeshPro Label;
    public SphereButtonRailing Rail;
    public ScreenspaceFocusable Focus;
    public bool IsDisabled;
    public ButtonInteractionStyles InteractionStyle;
    public bool Toggled;

    public float RailHeight;
    public event EventHandler Pressed;
    public event EventHandler Released;

    public float ActiviationScreenDistance = 100;
    public float DeactiviationScreenDistance = 120;

    public ButtonStyling Styling;

    private ButtonState state;
    
    public Renderer SphereRenderer;
    private Material sphereMeshMat;
    public Renderer RailRenderer;
    private Material railMat;

    public Transform ButtonContent;
    public Transform Icon;
    [SerializeField]
    private IconController.IconKey iconKey;
    [SerializeField]
    private IconController iconFront;
    [SerializeField]
    private IconController iconShadow;
    [SerializeField]
    private ButtonCursorSettings cursorSettings;

    public float ClickAnimationDuration;
    private float clickProgression;

    private float hoveredness;

    public Color CurrentColor { get; private set; }
    public Color CurrentGlowColor { get; private set; }

    private enum ButtonState
    {
        Disabled,
        Ready,
        Hovered,
        ClickOutro,
    }
    public enum ButtonInteractionStyles
    {
        ToggleButton,
        ClickButton
    }

    private void Start()
    {
        sphereMeshMat = SphereRenderer.material;
        railMat = RailRenderer.material;
        iconFront.Icon = iconKey;
        iconShadow.Icon = iconKey;
        cursorSettings.Icon = iconKey;
    }

    private void Update()
    {
        UpdateInteraction();
        UpdateFocus();
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        UpdateMaterials();
        UpdateHoverFeatures();
        UpdateIconPosition();
        UpdateClickAnimation();
    }

    private void UpdateFocus()
    {
        Focus.IsFocusable = state != ButtonState.Disabled;

        Focus.ActivationDistance = state == ButtonState.Hovered ? DeactiviationScreenDistance : ActiviationScreenDistance;
        Focus.ForceFocus = state == ButtonState.ClickOutro;
        this.cursorSettings.Mode = state == ButtonState.ClickOutro ? CursorMode.None : CursorMode.PointCursor;
    }

    private void UpdateClickAnimation()
    {
        clickProgression -= Time.deltaTime;
        clickProgression = Mathf.Max(0, clickProgression);
        float param = clickProgression / ClickAnimationDuration;
        param = Mathf.Pow(param, 2);
        ButtonContent.localRotation = Quaternion.Euler(0, param * 360, 0); 
    }

    private void UpdateIconPosition()
    {
        if(state == ButtonState.Hovered)
        {
            Vector3 fingerPos = HandPrototypeProxies.Instance.RightIndex.position;
            Vector3 toFinger = (ButtonContent.transform.position - fingerPos);
            float weight = 1 - Mathf.Clamp01(Mathf.Abs(toFinger.magnitude - .02f) * 50);
            Icon.localPosition = toFinger.normalized * weight * .05f * hoveredness;
            Icon.forward = Vector3.Lerp(transform.forward, -toFinger.normalized, weight * .3f * hoveredness);
        }
        else
        {
            Icon.localPosition = Vector3.zero;
            Icon.localRotation = Quaternion.identity;
        }
    }

    private void UpdateHoverFeatures()
    {
        float hoverednessTarget = (state == ButtonState.Hovered || state == ButtonState.ClickOutro) ? 1 : 0;
        hoveredness = Mathf.Lerp(hoveredness, hoverednessTarget, Time.deltaTime * 15);
        Rail.VerticalOffset = RailHeight * hoveredness;
        Label.color = new Color(1, 1, 1, hoveredness);
        railMat.SetFloat("_Highlight", hoveredness);
    }

    private void UpdateMaterials()
    {
        Color colorTarget = GetStateColor();
        CurrentColor = Color.Lerp(CurrentColor, colorTarget, Time.deltaTime * 15);
        Color targetGlowColor = colorTarget * (state == ButtonState.Hovered ? 1 : 0);
        CurrentGlowColor = Color.Lerp(CurrentGlowColor, targetGlowColor, Time.deltaTime * 15);

        sphereMeshMat.SetColor("_Color", CurrentColor);
        sphereMeshMat.SetFloat("_Disabledness", state == ButtonState.Disabled ? 1 : 0);

        cursorSettings.Color = CurrentColor;
    }

    private Color GetStateColor()
    {
        switch (state)
        {
            case ButtonState.Ready:
                return Toggled ? Styling.ReadyToggledColor :Styling.ReadyColor;
            case ButtonState.Hovered:
                return Styling.HoverColor;
            case ButtonState.ClickOutro:
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
            if (clickProgression > 0)
            {
                state = ButtonState.ClickOutro;
            }
            else
            {
                if(state == ButtonState.ClickOutro)
                {
                    state = ButtonState.Ready;
                }
                else
                {
                    state = FocusManager.Instance.FocusedItem == Focus ? ButtonState.Hovered : ButtonState.Ready;
                }
            }
        }

        if(state == ButtonState.Hovered)
        {
            float fingerHeight = HandPrototypeProxies.Instance.RightIndex.position.y; // TODO: Make this in localspace instead of worldspace
            if (oldState == ButtonState.Ready)
            {
                OnHoverStart(fingerHeight);
            }
            UpdateHoveringInteraction(fingerHeight);
        }
        if(!(state == ButtonState.Hovered || state == ButtonState.ClickOutro))
        {
            ButtonContent.localPosition = Vector3.Lerp(ButtonContent.localPosition, Vector3.zero, Time.deltaTime * 15);
        }
    }

    private void UpdateHoveringInteraction(float fingerHeight)
    {
        float riseAmount = fingerHeight - hoverStartHeight;
        riseAmount = riseAmount / transform.lossyScale.y;

        if (riseAmount > RailHeight)
        {
            OnClick();
        }

        riseAmount = Mathf.Clamp(riseAmount, 0, RailHeight);
        ButtonContent.localPosition = new Vector3(0, riseAmount, 0);
    }

    private void OnHoverStart(float fingerHeight)
    {
        hoverStartHeight = Mathf.Max(transform.position.y, fingerHeight);
        Styling.HoverSound.Play();
    }

    private void OnClick()
    {
        clickProgression = ClickAnimationDuration;

        Styling.ClickSound.Play();
        state = ButtonState.ClickOutro;
        if(InteractionStyle == ButtonInteractionStyles.ToggleButton)
        {
            Toggled = !Toggled;
        }
        Released?.Invoke(this, EventArgs.Empty);
    }
}
