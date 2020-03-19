using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrabbableSlateVisuals : MonoBehaviour
{
    [SerializeField]
    private Transform slateResizing;
    [SerializeField]
    private Transform grabber;
    [SerializeField]
    private SkinnedMeshRenderer meshRenderer;
    [SerializeField]
    private Transform root;
    [SerializeField]
    private Transform upperLeftCorner;
    [SerializeField]
    private Transform upperRightCorner;
    [SerializeField]
    private Transform lowerLeftCorner;
    [SerializeField]
    private Transform lowerRightCorner;

    private Transform upLeftGuide;
    private Transform upRightGuide;
    private Transform downRightGuide;
    private Transform downLeftGuide;

    private Transform[] corners;
    private Transform[] cornerGuides;
    private RotationOption[] rotationOptions;

    [SerializeField]
    private float gripSummonDist;
    private float gripSummonness;

    [SerializeField]
    private BoxFocusable grippeFrocus;

    private void Start()
    {
        CreateMidpoints();
        corners = new Transform[]
        {
            lowerRightCorner,
            lowerLeftCorner,
            upperLeftCorner,
            upperRightCorner
        };
        cornerGuides = new Transform[] 
        {
            downRightGuide,
            downLeftGuide,
            upLeftGuide,
            upRightGuide
        };
        rotationOptions = GetRotationOptions();
    }

    private RotationOption[] GetRotationOptions()
    {
        RotationOption[] ret = new RotationOption[4];
        for (int i = 0; i < 4; i++)
        {
            Transform pointA = cornerGuides[i];
            Transform pointB = cornerGuides[(i + 1) % 4];
            ret[i] = new RotationOption(pointA, pointB, i);
        }
        return ret;
    }

    private void CreateMidpoints()
    {
        upLeftGuide = new GameObject("Up Left Guide").transform;
        upLeftGuide.parent = slateResizing;
        upLeftGuide.localPosition = new Vector3(-.5f, .5f, 0);

        upRightGuide = new GameObject("Up Right Guide").transform;
        upRightGuide.parent = slateResizing;
        upRightGuide.localPosition = new Vector3(.5f, .5f, 0);

        downRightGuide = new GameObject("Down Right Guide").transform;
        downRightGuide.parent = slateResizing;
        downRightGuide.localPosition = new Vector3(.5f, -.5f, 0);

        downLeftGuide = new GameObject("Down Left Guide").transform;
        downLeftGuide.parent = slateResizing;
        downLeftGuide.localPosition = new Vector3(-.5f, -.5f, 0);
    }
    
    private void Update()
    {
        Vector3 pinchPoint = MainPinchDetector.Instance.PinchPoint.position;
        RotationOption closestOption = rotationOptions.OrderBy(item => item.GetDist(pinchPoint)).First();

        RotateAndPositionPanel(closestOption.Offset);
        UpdateBlendShapes(closestOption.GetDist(pinchPoint));
        UpdateMaterialProperties();
    }

    private void UpdateMaterialProperties()
    {
        meshRenderer.sharedMaterial.SetVector("_GripPos", MainPinchDetector.Instance.PinchPoint.position);
    }

    private void RotateAndPositionPanel(int rotationOffset)
    {
        root.position = GetPositions(rotationOffset);
        root.localRotation = Quaternion.Euler(0, 180, rotationOffset * 90);
        for (int i = 0; i < 4; i++)
        {
            int cornerGuideIndex = (i + rotationOffset) % 4;
            corners[i].position = cornerGuides[cornerGuideIndex].position;
        }
    }

    private Vector3 GetPositions(int rotationOffset)
    {
        int nextCorner = (rotationOffset + 1) % 4;
        Vector3 posA = cornerGuides[rotationOffset].position;
        Vector3 posB = cornerGuides[nextCorner].position;
        return (posA + posB) / 2;
    }

    private void UpdateBlendShapes(float pinchPointDist)
    {
        bool isSummoned = pinchPointDist < gripSummonDist;
        float summonnessTarget = isSummoned ? 0 : 1;
        gripSummonness = Mathf.Lerp(gripSummonness, summonnessTarget, Time.deltaTime * 10);
        meshRenderer.SetBlendShapeWeight(1, gripSummonness * 100);

        float pinchedness = GetPinchedness();
        meshRenderer.SetBlendShapeWeight(0, pinchedness * 100);
    }

    private float GetPinchedness()
    {
        if (FocusManager.Instance.FocusedItem == grippeFrocus)
        {
            float pinchProg = (MainPinchDetector.Instance.FingerDistance - .03f) / MainPinchDetector.Instance.PinchDist;
            return 1 - Mathf.Clamp01(pinchProg);
        }
        return 0;
    }

    private class RotationOption
    {
        private readonly Transform pointA;
        private readonly Transform pointB;
        public int Offset { get; }

        public RotationOption(Transform pointA, Transform pointB, int offset)
        {
            this.pointA = pointA;
            this.pointB = pointB;
            Offset = offset;
        }

        public float GetDist(Vector3 grabPoint)
        {
            Vector3 mid = (pointA.position + pointB.position) / 2;
            return (mid - grabPoint).magnitude;
        }
    }
}
