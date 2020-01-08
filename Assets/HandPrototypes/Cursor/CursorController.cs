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
    private float pointerScale = 0.02f;

    [SerializeField]
    private IconController iconController;
    [SerializeField]
    private TextMeshPro iconText;

    private Vector3 pointerEndTarget;
    private Vector3 startPoint;

    private void Update()
    {
        SetSettings();
        SetVisuals();
    }

    private void SetVisuals()
    {
        SetObjectPositions();
        SetMaterials();

        pointerSpan.LookAt(pointerEnd);
    }

    private void SetObjectPositions()
    {
        pointerStart.position = startPoint;
        pointerEnd.position = Vector3.Lerp(pointerEnd.position, pointerEndTarget, Time.deltaTime * 15);
        pointerSpan.position = (pointerStart.position + pointerEnd.position) / 2;
        float pointerSpanLength = (pointerStart.position - pointerEnd.position).magnitude / 2;

        pointerStart.localScale = new Vector3(pointerScale, pointerScale, pointerScale);
        pointerEnd.localScale = pointerStart.localScale;
        pointerSpan.localScale = new Vector3(pointerScale, pointerScale, pointerSpanLength);

        iconController.transform.LookAt(Camera.main.transform);
        iconController.transform.Rotate(0, 180, 0);
    }

    private void SetMaterials()
    {
        Shader.SetGlobalVector("_CursorStart", pointerStart.position);
        Shader.SetGlobalVector("_CursorEnd", pointerEnd.position);
    }

    private void SetSettings()
    {
        ICursorSettings cursorSettings = FocusManager.Instance.FocusedItem?.CursorSettings;
        if (cursorSettings != null)
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

        startPoint = GetGrabPoint(cursorSettings.Mode);
        pointerEndTarget = cursorSettings.GetCursorEndPosition(startPoint);
    }

    private Vector3 GetGrabPoint(CursorMode mode)
    {
        switch (mode)
        {
            case CursorMode.GrabCursor:
                return MainPinchDetector.Instance.PinchPoint.position;
            case CursorMode.PointCursor:
            default:
                return HandPrototypeProxies.Instance.RightIndex.position;
        }
    }

    private void SetToDefaultCursor()
    {
        startPoint = GetGrabPoint(CursorMode.PointCursor);
        pointerEndTarget = pointerStart.position;
        iconText.enabled = false;
    }
}
