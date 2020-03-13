using UnityEngine;

public class ButtonColors : MonoBehaviour
{
    public static ButtonColors Instance;

    public Color ReadyToggledColor;
    public Color ReadyColor;
    public Color HoverColor;
    public Color PressingColor;
    public Color DisabledColor;

    void Awake()
    {
        Instance = this;
    }
}
