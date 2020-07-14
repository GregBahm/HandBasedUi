using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlateResizing : MonoBehaviour
{
    public bool CurrentlyResizing
    {
        get
        {
            return LowerLeftCorner.IsGrabbed
                || LowerRightCorner.IsGrabbed
                || UpperLeftCorner.IsGrabbed
                || UpperRightCorner.IsGrabbed;
        }
    }

    [SerializeField]
    private Transform  resizableContent;

    [SerializeField]
    private Transform slate;

    [SerializeField]
    private float minScale;
    [SerializeField]
    private float maxScale;
    
    [SerializeField]
    private GameObject resizingGrabberPrefab;

    [SerializeField]
    private float grabberOffset;
    
    public SlateResizingCorner LowerLeftCorner { get; private set; }
    public SlateResizingCorner LowerRightCorner { get; private set; }
    public SlateResizingCorner UpperLeftCorner { get; private set; }
    public SlateResizingCorner UpperRightCorner { get; private set; }

    public IEnumerable<SlateResizingCorner> Corners { get; private set; }

    private void Start()
    {
        Transform lowerLeftLocator = CreateLocator(-.5f, -.5f);
        Transform lowerRightLocator = CreateLocator(.5f, -.5f);
        Transform upperLeftLocator = CreateLocator(-.5f, .5f);
        Transform upperRightLocator = CreateLocator(.5f, .5f);
        LowerLeftCorner = CreateCorner(lowerLeftLocator, upperRightLocator, -1, -1, 0);
        LowerRightCorner = CreateCorner(lowerRightLocator, upperLeftLocator, 1, -1, 90);
        UpperLeftCorner = CreateCorner(upperLeftLocator, lowerRightLocator, -1, 1, 90);
        UpperRightCorner = CreateCorner(upperRightLocator, lowerLeftLocator, 1, 1, 0);
        Corners = new SlateResizingCorner[] { LowerLeftCorner, LowerRightCorner, UpperLeftCorner, UpperRightCorner };
    }

    private void OnEnable()
    {
        if(Corners != null)
        {
            foreach (SlateResizingCorner corner in Corners)
            {
                corner.gameObject.SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        foreach (SlateResizingCorner corner in Corners)
        {
            if(corner != null)
            {
                corner.gameObject.SetActive(false);
            }
        }
    }

    private Transform CreateLocator(float localX, float localY)
    {
        Transform ret = new GameObject("Corner").transform;
        ret.parent = slate;
        ret.localPosition = new Vector3(localX, localY);
        return ret;
    }

    private SlateResizingCorner CreateCorner(Transform grabberLocation, Transform oppositeCorner, float xDirection, float yDirection, float iconRotation)
    {
        GameObject retObj = Instantiate(resizingGrabberPrefab);
        SlateResizingCorner ret = retObj.GetComponent<SlateResizingCorner>();
        ret.Initialize(resizableContent, oppositeCorner, iconRotation, grabberLocation.localPosition);

        GrabberVisualController grabberVisual = retObj.GetComponent<GrabberVisualController>();
        Vector3 grabberOffset = GetGrabberOffset(xDirection, yDirection);
        grabberVisual.SetGrabberLocation(grabberLocation, grabberOffset);

        return ret;
    }

    private Vector3 GetGrabberOffset(float xDirection, float yDirection)
    {
        return new Vector3(xDirection * grabberOffset, yDirection * grabberOffset, 0);
    }

    public void DoUpdate()
    {
        foreach (SlateResizingCorner corner in Corners)
        {
            corner.DoUpdate();
        }
    }
}