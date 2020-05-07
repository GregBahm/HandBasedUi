using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushyButtonController : MonoBehaviour
{
    [SerializeField]
    private ScreenspaceFocusable focus;

    public bool IsFocused { get { return FocusManager.Instance.FocusedItem == focus; } }

    public event EventHandler Released;
    public Transform ButtonContent;
    public Color CurrentGlowColor { get; set; }
    public Color CurrentColor { get; private set; }
    
    public ButtonStyling Styling;
    public bool Toggled;
    private ButtonState state;

    public Renderer SphereRenderer;
    private Material sphereMeshMat;

    private float baseGlobalSize;
    private Vector3 baseLocalSize;

    private void Start()
    {
        sphereMeshMat = SphereRenderer.material;
        state = ButtonState.Ready;
        baseGlobalSize = transform.lossyScale.x;
        baseLocalSize = transform.localScale;
    }


    private void Update()
    {
        state = IsFocused ? ButtonState.Hovered : ButtonState.Ready;
        UpdateMaterials();
        UpdateSize();
    }

    private void UpdateSize()
    {
        Vector3 scaleTarget = GetScaleTarget();
        transform.localScale = Vector3.Lerp(transform.localScale, scaleTarget, Time.deltaTime * 4);
    }

    private Vector3 GetScaleTarget()
    {
        if (IsFocused)
        {
            float factor = Mathf.Min(transform.lossyScale.x, baseGlobalSize);
            float newScale = baseGlobalSize / factor;
            newScale = Mathf.Pow(newScale, 2);
            return baseLocalSize * newScale;
        }
        else
        {
            return baseLocalSize;
        }
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
                return Toggled ? Styling.ReadyToggledColor : Styling.ReadyColor;
            case ButtonState.Hovered:
                return Styling.HoverColor;
            case ButtonState.ClickOutro:
                return Styling.PressingColor;
            case ButtonState.Disabled:
            default:
                return Styling.DisabledColor;
        }
    }

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
}
