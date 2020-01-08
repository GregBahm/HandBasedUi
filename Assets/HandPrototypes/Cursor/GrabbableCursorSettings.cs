using UnityEngine;

public class GrabbableCursorSettings : CursorSettings
{
    [SerializeField]
    private BoxFocusable focus;

    public override CursorMode Mode { get { return CursorMode.GrabCursor; } }

    public override Vector3 GetCursorEndPosition(Vector3 pointerPos)
    {
        return focus.Box.ClosestPoint(pointerPos);
    }
}
