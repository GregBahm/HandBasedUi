using UnityEngine;

[RequireComponent(typeof(PushyButtonController))]
public abstract class ButtonVisualController : MonoBehaviour
{
    protected PushyButtonController button;

    [SerializeField]
    public Color readyColor = Color.black;
    [SerializeField]
    public Color readyToggledColor = Color.gray;
    [SerializeField]
    public Color hoverColor = Color.blue;
    [SerializeField]
    public Color pressingColor = Color.cyan;
    [SerializeField]
    public Color disabledColor = Color.gray;

    protected Color GetStateColor()
    {
        switch (button.State)
        {
            case ButtonState.Ready:
                return button.Toggled ? readyToggledColor : readyColor;
            case ButtonState.Hovered:
                return hoverColor;
            case ButtonState.Pressed:
                return pressingColor;
            case ButtonState.Disabled:
            default:
                return disabledColor;
        }
    }
}
