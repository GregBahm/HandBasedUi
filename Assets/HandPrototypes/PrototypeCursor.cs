using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeCursor : MonoBehaviour
{
    public LineRenderer GrabLineA;
    public LineRenderer GrabLineB;
    public LineRenderer ButtonLineRenderer;

    private HandPrototypeProxies proxies;

    private Vector3[] grabAPositions;
    private Vector3[] grabBPositions;
    private Vector3[] buttonHoverPositions;

    public ProtoCursorState State { get; private set; }

    private Vector3 grabPoint;

    private Vector3 buttonHoverPoint;

    public enum ProtoCursorState
    {
        Nothing,
        GrabHover,
        ButtonHover,
    }

    private void Start()
    {
        grabAPositions = new Vector3[3];
        grabBPositions = new Vector3[3];
        buttonHoverPositions = new Vector3[2];
        proxies = HandPrototypeProxies.Instance;
    }

    private void Update()
    {
        GrabLineA.gameObject.SetActive(false);
        GrabLineB.gameObject.SetActive(false);
        ButtonLineRenderer.gameObject.SetActive(false);

        if (State == ProtoCursorState.GrabHover)
        {
            DoGrabHover();
        }
        if (State == ProtoCursorState.ButtonHover)
        {
            DoButtonHover();
        }
        State = ProtoCursorState.Nothing;
    }

    private void DoButtonHover()
    {
        ButtonLineRenderer.gameObject.SetActive(true);

        buttonHoverPositions[0] = buttonHoverPoint;
        buttonHoverPositions[1] = proxies.RightIndex.position;

        ButtonLineRenderer.SetPositions(buttonHoverPositions);
    }

    private void DoGrabHover()
    {
        GrabLineA.gameObject.SetActive(true);
        GrabLineB.gameObject.SetActive(true);

        grabAPositions[2] = grabPoint;
        grabAPositions[1] = proxies.RightIndex.position;
        grabBPositions[2] = grabPoint;
        grabBPositions[1] = proxies.RightThumb.position;

        Vector3 grabCenter = (proxies.RightIndex.position + proxies.RightThumb.position) / 2;
        Vector3 toA = (proxies.RightIndex.position - grabCenter).normalized * MainPinchDetector.Instance.PinchDist / 2;
        Vector3 toB = -toA;
        grabAPositions[0] = grabAPositions[1] + toB;
        grabBPositions[0] = grabBPositions[1] + toA ;

        GrabLineA.SetPositions(grabAPositions);
        GrabLineB.SetPositions(grabBPositions);
    }

    public void DoButtonHover(Vector3 buttonHoverPoint)
    {
        State = ProtoCursorState.ButtonHover;
        this.buttonHoverPoint = buttonHoverPoint;
    }

    public void DoGrabHover(Vector3 grabPoint)
    {
        if (State == ProtoCursorState.Nothing)
        {
            State = ProtoCursorState.GrabHover;
        }
        this.grabPoint = grabPoint;
    }
}