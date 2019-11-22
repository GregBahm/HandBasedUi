using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateResizingCorner : MonoBehaviour
{
    public BoxCollider GrabBox;

    public bool IsGrabbed;
    public Vector2 ResizingPivot;

    public LineRenderer LineRender;

    public bool ShowVisuals;

    private void Update()
    {
        LineRender.enabled = ShowVisuals;
    }
}
