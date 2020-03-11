using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberPlacer : MonoBehaviour
{
    [SerializeField]
    private Transform slateFront;
    [SerializeField]
    private Transform slateBack;
    [SerializeField]
    private Transform grabber;

    [SerializeField]
    private float maxWidth;
    [SerializeField]
    private float minWidth;
    [SerializeField]
    private float margins;

    public void DoUpdate()
    {
        PlaceGrabber();
        ScaleGrabber();
    }

    private void ScaleGrabber()
    {
        float width = MainPinchDetector.Instance.FingerDistance - .03f;
        width = Mathf.Clamp(width, minWidth, maxWidth);
        grabber.localScale = new Vector3(width, grabber.localScale.y, grabber.localScale.z);
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
        Vector3 grabPos = MainPinchDetector.Instance.PinchPoint.position;
        float maxX = slateFront.localScale.x / 2 - margins;
        float ret = transform.InverseTransformPoint(grabPos).x;
        ret = Mathf.Clamp(ret, -maxX, maxX);
        return ret;
    }

    private float GetGrabberY()
    {
        float bottom = slateFront.localPosition.y - slateFront.localScale.y / 2;
        float grabberRadius = grabber.localScale.y / 2;
        return bottom - grabberRadius;
    }
}
