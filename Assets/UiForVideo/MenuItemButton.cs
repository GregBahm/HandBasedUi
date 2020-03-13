using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItemButton : MonoBehaviour
{
    public bool IsDisabled;
    public ButtonInteractionStyles InteractionStyle;
    public bool Toggled;

    private bool clickable;
    public event EventHandler Pressed;
    public event EventHandler Released;

    public ButtonColors Colors;

    private ButtonState state;

    public MeshCollider Backdrop;
    public MeshCollider BackwardsBackdrop;
    private float fingerDistance;

    public Renderer BackgroundRenderer;
    private Material quadMeshMat;

    public Transform ButtonContent;

    private const float BaseDist = -0.08f;
    private const float HoverDist = -0.2f;
    private const float PressDist = 0.08f;

    private Color currentColor;

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
        currentColor = Color.Lerp(currentColor, colorTarget, Time.deltaTime * 15);
        quadMeshMat.SetColor("_Color", currentColor);
        quadMeshMat.SetFloat("_Disabledness", state == ButtonState.Disabled ? 1 : 0);
    }

    private Color GetStateColor()
    {
        switch (state)
        {
            case ButtonState.Ready:
                return Toggled ? Colors.ReadyToggledColor :Colors.ReadyColor;
            case ButtonState.Hovered:
                return Colors.HoverColor;
            case ButtonState.Pressing:
                return Colors.PressingColor;
            case ButtonState.Disabled:
            default:
                return Colors.DisabledColor;
        }
    }

    public class ButtonColors
    {
        public Color ReadyToggledColor;
        public Color ReadyColor;
        public Color HoverColor;
        public Color PressingColor;
        public Color DisabledColor;
    }


    private bool IsHoveringOver(out RaycastHit hitInfo)
    {
        Ray ray = new Ray(HandPrototypeProxies.Instance.RightIndex.position, transform.forward);
        return Backdrop.Raycast(ray, out hitInfo, float.PositiveInfinity);
    }

    private bool IsHoveringUnder()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(HandPrototypeProxies.Instance.RightIndex.position, -transform.forward);
        return BackwardsBackdrop.Raycast(ray, out hitInfo, float.PositiveInfinity);
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
        if(state == ButtonState.Pressing)
        {
            if(!IsHoveringUnder())
            {
                OnRelease();
            }
        }
        RaycastHit hoverInfo;
        if(state == ButtonState.Hovered)
        {
            if (IsHoveringUnder())
            {
                OnPress();
            }
            else if (!IsHoveringOver(out hoverInfo))
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
            if(IsHoveringOver(out hoverInfo))
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
