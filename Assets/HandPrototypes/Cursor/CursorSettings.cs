using UnityEngine;

public abstract class CursorSettings : MonoBehaviour, ICursorSettings
{
    [SerializeField]
    private CursorMode mode = CursorMode.PointCursor;
    public CursorMode Mode { get => this.mode; set => this.mode = value; }

    [SerializeField]
    private IconController.IconKey icon = IconController.IconKey.Move;
    public IconController.IconKey Icon { get => this.icon; set => this.icon = value; }

    [SerializeField]
    private Color color;

    public Color Color { get => this.color; set => this.color = value; }

    public abstract Vector3 GetCursorEndPosition(Vector3 pointerPos);
}
