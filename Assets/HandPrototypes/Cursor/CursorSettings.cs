using UnityEngine;

public abstract class CursorSettings : MonoBehaviour, ICursorSettings
{
    public abstract CursorMode Mode { get; }

    [SerializeField]
    private IconController.IconKey icon = IconController.IconKey.Drag;
    public IconController.IconKey Icon { get => icon; set => icon = value; }

    public abstract Vector3 GetCursorEndPosition(Vector3 pointerPos);
}
