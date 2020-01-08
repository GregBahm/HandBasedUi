using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchDetector : MonoBehaviour
{
    public float PinchDist = 0.03f;
    public float UnpinchDist = 0.09f;
    private HandPrototypeProxies proxies;

    public Transform ThumbProxy;
    public Transform FingertipProxy;
    public Transform PalmProxy;

    private bool wasPinching;

    /// <summary>
    /// True during any frame where the users thumb and index fingers were brought within the distance thresholds
    /// </summary>
    public bool Pinching { get; private set; }

    /// <summary>
    /// True on the frame where a pinch first starts
    /// </summary>
    public bool PinchBeginning { get; private set; }
    
    public Transform PinchPoint { get; private set; }

    private void Start()
    {
        PinchPoint = new GameObject("Pinch Point").transform;
    }

    private void Update()
    {
        float tipDistance = (ThumbProxy.position - FingertipProxy.position).magnitude;
        if(Pinching)
        {
            Pinching = tipDistance < UnpinchDist;
        }
        else
        {
            Pinching = tipDistance < PinchDist;
        }

        UpdateGrabPoint();

        PinchBeginning = Pinching && !wasPinching;
        wasPinching = Pinching;
    }

    private void UpdateGrabPoint()
    {
        Vector3 pinchPos = (ThumbProxy.position + FingertipProxy.position) / 2;

        PinchPoint.position = pinchPos;
        PinchPoint.rotation = PalmProxy.rotation;
    }
}
