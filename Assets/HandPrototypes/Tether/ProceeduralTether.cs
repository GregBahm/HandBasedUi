using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ProceeduralTether : MonoBehaviour
{
    [SerializeField]
    private Transform tetherStart;
    [SerializeField]
    private Transform tetherEnd;
    [SerializeField]
    private float tetherFlex;

    private Material tetherMat;
    public Material TetherMat => tetherMat;

    private Vector3 tetherStartAnchor;
    private Vector3 tetherEndAnchor;
    
    private const int VertsCount = 20;
    private Vector3[] verts;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        tetherMat = lineRenderer.material;
        verts = new Vector3[VertsCount];
        lineRenderer.positionCount = VertsCount;
        UpdateAnchors();
    }

    private void DrawLineSegments()
    {
        for (int i = 0; i < VertsCount; i++)
        {
            float param = (float)i / VertsCount;
            verts[i] = GetSplinePoint(param);
        }
        lineRenderer.SetPositions(verts);
    }

    public  void DoUpdate()
    {
        UpdateAnchors();
        DrawLineSegments();
    }

    private void UpdateAnchors()
    {
        tetherStartAnchor = tetherStart.position + new Vector3(0, tetherFlex, 0);
        tetherEndAnchor = tetherEnd.position + new Vector3(0, -tetherFlex, 0);
    }

    public Vector3 GetSplinePoint(float param)
    {
        Vector3 ab = Vector3.Lerp(tetherStart.position, tetherStartAnchor, param);
        Vector3 bc = Vector3.Lerp(tetherStartAnchor, tetherEndAnchor, param);
        Vector3 cd = Vector3.Lerp(tetherEndAnchor, tetherEnd.position, param);

        Vector3 abc = Vector3.Lerp(ab, bc, param);
        Vector3 bcd = Vector3.Lerp(bc, cd, param);
        
        Vector3 abcd = Vector3.Lerp(abc, bcd, param);
        return abcd;
    }
}
