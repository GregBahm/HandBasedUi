using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizingBorderIndicator : MonoBehaviour
{
    [SerializeField]
    private LineRenderer ResizingIndicatorA;
    [SerializeField]
    private LineRenderer ResizingIndicatorB;

    [SerializeField]
    private SlateResizing resizing;

    private Vector3[] linePositionsA = new Vector3[2];
    private Vector3[] linePositionsB = new Vector3[2];

    private float highlightness;

    private void Start()
    {
    }

    private void Update()
    {
        bool showLine = false;
        foreach (SlateResizingCorner item in resizing.Corners)
        {
            if(FocusManager.Instance.FocusedItem == item.Focus)
            {
                DoIndicateResizingBorder(item.LocalSlatePosition);
                showLine = true;
            }
        }
        UpdateHighlight(showLine);
    }

    private void UpdateHighlight(bool showLine)
    {
        float highlightTarget = showLine ? 1 : 0;
        highlightness = Mathf.Lerp(highlightness, highlightTarget, Time.deltaTime * 5);
        ResizingIndicatorA.material.SetFloat("_Highlight", highlightness);
        ResizingIndicatorB.material.SetFloat("_Highlight", highlightness);
    }

    private void DoIndicateResizingBorder(Vector3 basePos)
    {
        linePositionsA[0] = new Vector3(basePos.x, -basePos.y, basePos.z);
        linePositionsA[1] = basePos;
        ResizingIndicatorA.SetPositions(linePositionsA);

        float lengthA = GetWorldLength(linePositionsA[0], linePositionsA[1]);
        ResizingIndicatorA.material.SetFloat("_Length", lengthA);

        linePositionsB[0] = new Vector3(-basePos.x, basePos.y, basePos.z);
        linePositionsB[1] = basePos;
        ResizingIndicatorB.SetPositions(linePositionsB);

        float lengthB = GetWorldLength(linePositionsB[0], linePositionsB[1]);
        ResizingIndicatorB.material.SetFloat("_Length", lengthB);
    }

    private float GetWorldLength(Vector3 localStart, Vector3 localEnd)
    {

        Vector3 worldStart = transform.TransformPoint(localStart);
        Vector3 worldEnd = transform.TransformPoint(localEnd);
        return (worldStart - worldEnd).magnitude;
    }
}
