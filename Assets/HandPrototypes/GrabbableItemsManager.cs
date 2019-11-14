﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableItemsManager : MonoBehaviour
{
    public float GrabSmoothing = 15;
    public float GrabRestoreTime;
    public float GrabMargin = .1f;
    public float GrabSnapThreshold;

    public Grabbable[] Items;

    private bool wasGrabbing;
    public Grabbable GrabbedItem { get; private set; }

    public Transform SmoothedGrabPoint { get; private set; }

    private void Start()
    {
        SmoothedGrabPoint = new GameObject("Smoothed Grab Point").transform;
    }

    public void Update()
    {
        UpdateSmoothedGrabPoint();
        if(GrabDetector.Instance.Grabbing && !wasGrabbing)
        {
            if(GrabbedItem == null)
            {
                HandleStartGrab();
            }
        }
        if(!GrabDetector.Instance.Grabbing && GrabbedItem != null)
        {
            HandleStopGrabbing();
        }
        wasGrabbing = GrabDetector.Instance.Grabbing;
    }

    private void UpdateSmoothedGrabPoint()
    {
        Vector3 positionTarget = GrabDetector.Instance.GrabPoint.position;
        Quaternion rotationTarget = GrabDetector.Instance.GrabPoint.rotation;
        SmoothedGrabPoint.position = Vector3.Lerp(positionTarget, SmoothedGrabPoint.position, GrabSmoothing * Time.deltaTime);
        SmoothedGrabPoint.rotation = Quaternion.Lerp(rotationTarget, SmoothedGrabPoint.rotation, GrabSmoothing * Time.deltaTime);
    }

    private void HandleStopGrabbing()
    {
        GrabbedItem.EndGrab();
        GrabbedItem = null;
    }

    private void HandleStartGrab()
    {
        GrabbedItem = GetGrabbable();
        if(GrabbedItem != null)
        {
            GrabbedItem.StartGrab();
        }
    }

    private Grabbable GetGrabbable()
    {
        float closestGrabDist = GrabMargin;
        Grabbable ret = null;
        foreach (Grabbable item in Items)
        {
            if(item.Box.gameObject.activeInHierarchy)
            {
                float grabDist = item.GetDistanceToGrab();
                if(grabDist < closestGrabDist)
                {
                    closestGrabDist = grabDist;
                    ret = item;
                }
            }
        }
        return ret;
    }
}
