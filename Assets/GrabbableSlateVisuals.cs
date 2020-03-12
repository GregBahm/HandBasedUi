using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrabbableSlateVisuals : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineA; 
    [SerializeField]
    private LineRenderer lineB;
    [SerializeField]
    private Transform slateResizing;
    [SerializeField]
    private SlateVisualController slateFront;
    [SerializeField]
    private Transform grabber;

    [SerializeField]
    private int cornerVerts;
    
    private void Start()
    {
    }

    private void Update()
    {
        SetLinePositions(lineA, 1);
        SetLinePositions(lineB, -1);
    }

    private void SetLinePositions(LineRenderer renderer, float side)
    {
        Vector3[] positions = GetLinePositions(side).ToArray();
        renderer.positionCount = positions.Length;
        renderer.SetPositions(positions);
    }

    private IEnumerable<Vector3> GetLinePositions(float side)
    {
        float rounding = grabber.localScale.y / 2;

        float zDepth = slateFront.transform.localPosition.z;
        float grabberX = grabber.localScale.x * side - grabber.localPosition.x;
        float slateY = slateResizing.localScale.y / 2;
        float slateX = slateResizing.localScale.x / 2;
        float firstRoundingX = -grabberX - (rounding * side);

        float startX = (-slateX + slateFront.Rounding * slateX) * side;
        Vector3 startPos = new Vector3(startX, -slateY, zDepth);
        yield return startPos;

        Vector3 firstRoundingStart = new Vector3(firstRoundingX, -slateY, zDepth);
        yield return firstRoundingStart;

        Vector3 firstRoundingCenter = new Vector3(firstRoundingX, -slateY - rounding, zDepth);
        Vector3 firstRoundingEnd = new Vector3(-grabberX, -slateY - rounding, zDepth);
        
        for (int i = 0; i < cornerVerts; i++)
        {
            float param = (float)i / (cornerVerts - 1);
            yield return SweepAboutCircle(firstRoundingCenter, firstRoundingStart, firstRoundingEnd, param);
        }
        yield return firstRoundingEnd;

        Vector3 grabberCenter = new Vector3(-grabberX, grabber.localPosition.y, 0);
        Vector3 grabberStart = new Vector3(-grabberX, grabber.localPosition.y, zDepth);
        yield return grabberStart;
        Vector3 grabberBottom = new Vector3(-grabberX, grabber.localPosition.y - grabber.localScale.y /2, 0);
        for (int i = 0; i < cornerVerts; i++)
        {
            float param = (float)i / cornerVerts;
            yield return SweepAboutCircle(grabberCenter, grabberStart, grabberBottom, param);
        }
        Vector3 grabberEnd = new Vector3(-grabberX, grabber.localPosition.y, -zDepth);
        for (int i = 0; i < cornerVerts; i++)
        {
            float param = (float)i / cornerVerts;
            yield return SweepAboutCircle(grabberCenter, grabberBottom, grabberEnd, param);
        }
        yield return grabberEnd;

        Vector3 secondRoundingCenter = new Vector3(firstRoundingX, -slateY - rounding, -zDepth);
        Vector3 secondRoundingStart = new Vector3(-grabberX, -slateY - rounding, -zDepth);
        Vector3 secondRoundingEnd = new Vector3(firstRoundingX, -slateY, -zDepth);
        for (int i = 0; i < cornerVerts; i++)
        {
            float param = (float)i / (cornerVerts - 1);
            yield return SweepAboutCircle(secondRoundingCenter, secondRoundingStart, secondRoundingEnd, param);
        }
        Vector3 endPos = new Vector3(startX, -slateY, -zDepth);
        yield return endPos;
    }

    private Vector3 SweepAboutCircle(Vector3 center,
        Vector3 start,
        Vector3 end,
        float param)
    {
        Vector3 toStart = start - center;
        Vector3 toEnd = end - center;
        Vector3 toRet = Vector3.Slerp(toStart, toEnd, param);
        return toRet + center;
    }
}
