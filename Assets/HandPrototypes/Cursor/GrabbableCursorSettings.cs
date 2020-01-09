using UnityEngine;

public class GrabbableCursorSettings : CursorSettings
{
    [SerializeField]
    private BoxFocusable focus;


    public override Vector3 GetCursorEndPosition(Vector3 pointerPos)
    {
        Plane plane = new Plane(focus.Box.transform.forward, focus.Box.transform.position);
        return plane.ClosestPointOnPlane(pointerPos);
    }
}
