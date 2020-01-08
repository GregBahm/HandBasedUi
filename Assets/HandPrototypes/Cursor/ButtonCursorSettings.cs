using UnityEngine;

public class ButtonCursorSettings : CursorSettings
{
    public override CursorMode Mode { get { return CursorMode.PointCursor; } }

    public override Vector3 GetCursorEndPosition(Vector3 pointerPos)
    {
        return transform.position;
    }
}