using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberPlacer : MonoBehaviour
{
    [SerializeField]
    private float maxRadius;
    [Range(0, 1)]
    [SerializeField]
    private float summon;

    [SerializeField]
    private Transform slateSizing;
    [SerializeField]
    private Transform slateFront;
    [SerializeField]
    private Transform slateBack;
    [SerializeField]
    private Transform grabber;

    [SerializeField]
    private BoxFocusable focus;

    [SerializeField]
    private float maxWidth;
    [SerializeField]
    private float minWidth;
    [SerializeField]
    private float margins;

    [SerializeField]
    private float tabDrop;

    public float GrabberRadius
    {
        get { return maxRadius * summon; }
    }

    public void DoUpdate()
    {
        ScaleGrabber();
        PlaceSlates();
        PlaceGrabber();
    }

    private void PlaceSlates()
    {
        slateFront.localPosition = new Vector3(0, 0, GrabberRadius);
        slateBack.localPosition = new Vector3(0, 0, -GrabberRadius);
    }

    private void ScaleGrabber()
    {
        float width = maxWidth * summon;
        if(FocusManager.Instance.FocusedItem == focus)
        {
            width = MainPinchDetector.Instance.FingerDistance - .03f;
            width = Mathf.Clamp(width, minWidth, maxWidth);
        }
        grabber.localScale = new Vector3(width, GrabberRadius * 2, GrabberRadius * 2);
    }

    private void PlaceGrabber()
    {
        float grabberY = GetGrabberY();
        float grabberXTarget = GetGrabberX();
        float grabberX = Mathf.Lerp(grabber.localPosition.x, grabberXTarget, Time.deltaTime * 4);
        grabber.localPosition = new Vector3(grabberX, grabberY, 0);
    }

    private float GetGrabberX()
    {
        return 0;
        Vector3 grabPos = MainPinchDetector.Instance.PinchPoint.position;
        float maxX = .5f - margins;
        float ret = transform.InverseTransformPoint(grabPos).x;
        ret = Mathf.Clamp(ret, -maxX, maxX);
        return ret;
    }

    private float GetGrabberY()
    {
        float bottom = slateSizing.localPosition.y - slateSizing.localScale.y / 2;
        float grabberRadius = grabber.localScale.y / 2;
        return bottom - grabberRadius - tabDrop * summon;
    }
}
