using UnityEngine;

public class GazeClickable : MonoBehaviour
{
    private bool wasPinching;

    public PinchDetector LeftPinchDetector;

    public MeshCollider Backdrop;
    public Renderer BackgroundRenderer;
    private Material quadMeshMat;

    public ButtonColors Manager;

    private ButtonState state;

    private Color currentColor;

    private HandPrototypeG prototypeG;

    private enum ButtonState
    {
        Ready,
        Hovered,
        Pressing,
    }

    private void Start()
    {
        quadMeshMat = BackgroundRenderer.material;
        prototypeG = Manager.gameObject.GetComponent<HandPrototypeG>();
    }

    private void Update()
    {
        UpdateInteraction();
        wasPinching = LeftPinchDetector.Pinching;
        UpdateMaterial();
    }
    private void UpdateInteraction()
    {
        RaycastHit hitInfo;
        bool isHovering = IsHoveringOver(out hitInfo);
        if (!isHovering)
        {
            state = ButtonState.Ready;
            return;
        }
        else
        {
            prototypeG.GazeCursor.position = hitInfo.point;
        }

        if (state == ButtonState.Ready)
        {
            state = ButtonState.Hovered;
        }
        if (state == ButtonState.Hovered)
        {
            if (!wasPinching && LeftPinchDetector.Pinching)
            {
                state = ButtonState.Pressing;
                OnPress();
            }
        }
        if (state == ButtonState.Pressing)
        {
            if(!LeftPinchDetector.Pinching)
            {
                OnRelease();
                state = ButtonState.Hovered;
            }
        }
    }

    private void OnRelease()
    {
        //Manager.OnAnyButtonRelease();
    }

    private void OnPress()
    {
        //Manager.OnAnyButtonPress();
    }

    private bool IsHoveringOver(out RaycastHit hitInfo)
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        return Backdrop.Raycast(ray, out hitInfo, float.PositiveInfinity);
    }

    private void UpdateMaterial()
    {
        Color colorTarget = GetStateColor();
        currentColor = Color.Lerp(currentColor, colorTarget, Time.deltaTime * 15);
        quadMeshMat.SetColor("_Color", currentColor);
    }

    private Color GetStateColor()
    {
        switch (state)
        {
            case ButtonState.Ready:
                return Manager.ReadyColor;
            case ButtonState.Hovered:
                return Manager.HoverColor;
            case ButtonState.Pressing:
            default:
                return Manager.PressingColor;
        }
    }
}