using UnityEngine;

public interface ICursorSettings
{
    CursorMode Mode { get; }

    IconController.IconKey Icon { get; }

    Vector3 GetCursorEndPosition(Vector3 pointerPos);

    Color Color { get; }
}
