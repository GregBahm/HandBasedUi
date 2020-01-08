using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField]
    private Transform pointerStart;
    [SerializeField]
    private Transform pointerEnd;
    [SerializeField]
    private Transform pointerSpan;
    [SerializeField]
    private Transform topCone;
    [SerializeField]
    private Transform bottomCone;

    [SerializeField]
    private float pointerScale = 0.02f;

    [SerializeField]
    private IconController iconController;
    [SerializeField]
    private TextMeshPro iconText;

    [SerializeField]
    private Material cursorMat;
    [SerializeField]
    private Color defaultEndCursorColor;
    [SerializeField]
    private Color defaultStartCursorColor;
    [SerializeField]
    private Color grabbedStartCursorColor;

    private Color endCursorColor;
    private Color startTargetColor;
    private Color startCursorColor;

    private Vector3 pointerEndTarget;
    private Vector3 startPoint;
    private float pointOrGrabness;

    [SerializeField]
    private float velocityDecay;
    [SerializeField]
    private float velocityGain;
    private float velocity;
    private bool idle;
    private Vector3 lastPosition;

    private void Update()
    {
        SetSettings();
        SetVisuals();
        lastPosition = startPoint;
    }

    private void SetVisuals()
    {
        UpdateVelocity();
        SetObjectPositions();
        SetMaterials();

        pointerSpan.LookAt(pointerEnd);
    }

    private void UpdateVelocity()
    {
        if(idle)
        {
            velocity -= Time.deltaTime * velocityDecay;
            float movement = (lastPosition - startPoint).magnitude;
            velocity += movement * velocityGain;
            velocity = Mathf.Clamp01(velocity);
        }
        else
        {
            velocity = 1;
        }
    }

    private void SetObjectPositions()
    {
        pointerStart.position = startPoint;
        pointerEnd.position = Vector3.Lerp(pointerEnd.position, pointerEndTarget, Time.deltaTime * 15);
        pointerSpan.position = (pointerStart.position + pointerEnd.position) / 2;
        float pointerSpanLength = (pointerStart.position - pointerEnd.position).magnitude / 2;

        pointerStart.localScale = new Vector3(pointerScale, pointerScale, pointerScale) * velocity;
        pointerEnd.localScale = pointerStart.localScale;
        pointerSpan.localScale = new Vector3(pointerScale * velocity, pointerScale * velocity, pointerSpanLength);

        iconController.transform.LookAt(Camera.main.transform);
        iconController.transform.Rotate(0, 180, 0);

        topCone.position = HandPrototypeProxies.Instance.RightIndex.position;
        topCone.LookAt(MainPinchDetector.Instance.PinchPoint.position);

        bottomCone.position = HandPrototypeProxies.Instance.RightThumb.position;
        bottomCone.LookAt(MainPinchDetector.Instance.PinchPoint.position);

        float coneScale = GetConeScale() * velocity;
        bottomCone.localScale = new Vector3(coneScale, coneScale, coneScale);
        topCone.localScale = bottomCone.localScale;
    }

    private float GetConeScale()
    {
        float fingerDistance = (MainPinchDetector.Instance.ThumbProxy.position - MainPinchDetector.Instance.FingertipProxy.position).magnitude;
        float pinchDist = fingerDistance - MainPinchDetector.Instance.PinchDist;
        pinchDist = Mathf.Clamp(pinchDist, 0, pointerScale);
        return pinchDist * (1 - pointOrGrabness);

    }

    private void SetMaterials()
    {
        startCursorColor = Color.Lerp(startCursorColor, startTargetColor, Time.deltaTime * 20);

        cursorMat.SetColor("_EndColor", this.endCursorColor);
        cursorMat.SetColor("_StartColor", this.startCursorColor);
        cursorMat.SetVector("_CursorStart", this.pointerStart.position);
        cursorMat.SetVector("_CursorEnd", this.pointerEnd.position);
    }

    private void SetSettings()
    {
        ICursorSettings cursorSettings = FocusManager.Instance.FocusedItem?.CursorSettings;
        if (cursorSettings != null && cursorSettings.Mode != CursorMode.None)
        {
            SetCursorSettings(cursorSettings);
        }
        else
        {
            SetToDefaultCursor();
        }
    }

    private void SetCursorSettings(ICursorSettings cursorSettings)
    {
        iconController.Icon = cursorSettings.Icon;
        iconText.enabled = true;

        SetGrabPoint(cursorSettings.Mode);
        pointerEndTarget = cursorSettings.GetCursorEndPosition(startPoint);
        endCursorColor = cursorSettings.Color;
    }

    private void SetGrabPoint(CursorMode mode)
    {
        Vector3 pinchPoint = MainPinchDetector.Instance.PinchPoint.position;
        Vector3 grabPoint = HandPrototypeProxies.Instance.RightIndex.position;
        float pointOrGrabTarget = mode == CursorMode.PointCursor ? 1 : 0;
        pointOrGrabness = Mathf.Lerp(pointOrGrabness, pointOrGrabTarget, Time.deltaTime * 15);
        startPoint = Vector3.Lerp(pinchPoint, grabPoint, pointOrGrabness);
        startTargetColor = FocusManager.Instance.FocusForced ? Color.black : this.grabbedStartCursorColor;
        idle = mode == CursorMode.None;
    }

    private void SetToDefaultCursor()
    {
        SetGrabPoint(CursorMode.PointCursor);
        pointerEndTarget = pointerStart.position;
        iconText.enabled = false;
        endCursorColor = this.defaultEndCursorColor;
        startTargetColor = this.defaultStartCursorColor;
        idle = true;
    }
}
