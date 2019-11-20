using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableItemsManager : MonoBehaviour
{
    public int TimewarpFrames = 10;
    public float PanelSmoothing = 5;
    public float SnapThreshold = .9f;

    public PreviewBox PreviewBox;
    public PrototypeCursor GrabCursor;

    public float PreviewBoxPadding = 0.01f;

    public float GrabSmoothing = 15;
    public float GrabRestoreTime;
    public float GrabMargin = .1f;

    public Grabbable[] Items;

    public GrabPreviewing GrabPreview { get; } = new GrabPreviewing();

    private bool wasGrabbing;
    public Grabbable GrabbedItem { get; set; }

    public Transform SmoothedGrabPoint { get; private set; }

    private void Start()
    {
        SmoothedGrabPoint = new GameObject("Smoothed Grab Point").transform;
    }

    public void Update()
    {
        UpdateSmoothedGrabPoint();
        UpdateGrabbing();
        UpdateGrabPreviewCursor();
        UpdateGrabPreview();
    }

    private void UpdateGrabPreviewCursor()
    {
        bool showCursor = GrabPreview.Grabbable != null && !MainPinchDetector.Instance.Pinching;
        if(showCursor)
        {
            GrabCursor.DoGrabHover(GrabPreview.GrabPosition);
        }
    }

    private void UpdateGrabPreview()
    {
        bool itemIsGrabbed = GrabbedItem != null;
        PreviewBox.gameObject.SetActive(itemIsGrabbed);
    }

    private void UpdateGrabbing()
    {
        if (MainPinchDetector.Instance.Pinching )
        {
            if (!wasGrabbing && GrabPreview.Grabbable != null)
            {
                HandleStartGrab();
            }
        }
        else
        {
            UpdateGrabPreviewing();
        }
        if (!MainPinchDetector.Instance.Pinching && GrabbedItem != null)
        {
            HandleStopGrabbing();
        }
        wasGrabbing = MainPinchDetector.Instance.Pinching;
    }

    private void UpdateSmoothedGrabPoint()
    {
        Vector3 positionTarget = MainPinchDetector.Instance.PinchPoint.position;
        Quaternion rotationTarget = MainPinchDetector.Instance.PinchPoint.rotation;
        SmoothedGrabPoint.position = Vector3.Lerp(positionTarget, SmoothedGrabPoint.position, GrabSmoothing * Time.deltaTime);
        SmoothedGrabPoint.rotation = Quaternion.Lerp(rotationTarget, SmoothedGrabPoint.rotation, GrabSmoothing * Time.deltaTime);
    }

    private void HandleStopGrabbing()
    {
        PreviewBox.EndGrab();
        GrabbedItem.EndGrab();
        GrabbedItem = null;
    }

    private void HandleStartGrab()
    {
        GrabbedItem = GrabPreview.Grabbable;
        if (GrabPreview.WasThumbnail)
        {
            PreviewBox.StartThumbnailGrab();
            GrabbedItem.StartThumbGrab();
        }
        else
        {
            PreviewBox.StartGrab();
            GrabbedItem.StartGrab();
        }
    }

    private void UpdateGrabPreviewing()
    {
        Vector3 grabPoint = MainPinchDetector.Instance.PinchPoint.position;
        
        float closestGrabDist = GrabMargin;
        GrabPreview.Clear();

        foreach (Grabbable item in Items)
        {
            closestGrabDist = UpdateGrabPreviewing(grabPoint, closestGrabDist, item, item.Box, false);
            closestGrabDist = UpdateGrabPreviewing(grabPoint, closestGrabDist, item, item.ThumbnailBox, true);
        }
    }

    private float UpdateGrabPreviewing(Vector3 grabPoint, float closestGrabDist, Grabbable item, BoxCollider box, bool isThumbnail)
    {
        if (box.gameObject.activeInHierarchy)
        {
            Vector3 itemPoint = box.ClosestPoint(grabPoint);
            float itemDist = (grabPoint - itemPoint).magnitude;
            if (itemDist < closestGrabDist)
            {
                GrabPreview.Update(item, itemPoint, isThumbnail);
                return itemDist;
            }
        }
        return closestGrabDist;
    }

    public class GrabPreviewing
    {
        public Grabbable Grabbable { get; private set; }
        public Vector3 GrabPosition { get; private set; }
        public bool WasThumbnail { get; private set; }

        public void Update(Grabbable grabbable, Vector3 grabPosition, bool wasThumbnail)
        {
            Grabbable = grabbable;
            GrabPosition = grabPosition;
            WasThumbnail = wasThumbnail;
        }

        public void Clear()
        {
            Grabbable = null;
        }
    }
}
