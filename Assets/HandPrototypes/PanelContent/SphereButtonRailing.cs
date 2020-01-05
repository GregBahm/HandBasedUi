using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SphereButtonRailing : MonoBehaviour
{
    public float Radius = 1;
    public float VerticalOffset;
    [Range(0, .005f)]
    public float LineThickness = 1;
    public int Points = 32;

    private Vector3[] pointPositions;
    private LineRenderer lineRenderer;

    private void Start()
    {
        this.pointPositions = new Vector3[Points];

        this.lineRenderer = GetComponent<LineRenderer>();
        this.lineRenderer.positionCount = Points;
    }

    private void Update()
    {
        this.lineRenderer.widthMultiplier = LineThickness;
        SetPointPositions();
        this.lineRenderer.SetPositions(pointPositions);
    }

    private void SetPointPositions()
    {
        float angleIncrement = (Mathf.PI * 2) / Points;
        for (int i = 0; i < Points; i++)
        {
            float angle = angleIncrement * i + (angleIncrement / 2);
            this.pointPositions[i] = GetPointPosition(angle);
        }
    }

    Vector3 GetPointPosition(float angle)
    {
        float effectiveRadius = Radius + LineThickness / 2;
        float x = Mathf.Sin(angle) * effectiveRadius;
        float y = Mathf.Cos(angle) * effectiveRadius;
        float verticalOffset = y > 0 ? VerticalOffset : 0;
        return new Vector3(x, y + verticalOffset, 0);
    }
}
