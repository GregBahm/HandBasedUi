using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationMain : MonoBehaviour
{
    [Range(1, 30)]
    public float AnnotationDepth;
    public Transform Annotation;
    public Transform Frustum;
    
    public AnnotationRepositioner Phone;
    public AnnotationRepositioner PlaneTarget;
    
    private void Update()
    {
        Phone.UpdateSlatePositioning();
        UpdateAnnotationPlane();
        PlaceAnnotation();
    }

    private void UpdateAnnotationPlane()
    {
        PlaneTarget.UpdateSlatePositioning();
        if(PlaneTarget.CurrentlyRepositioning)
        {
            AnnotationDepth = GetAnnotationDepth();
        }
        else
        {
            PlaneTarget.transform.position = Annotation.position;
        }
    }

    private float GetAnnotationDepth()
    {
        float ret = PlaneTarget.transform.localPosition.z * -10;
        ret = Mathf.Max(1, ret);
        return ret;
    }

    private void PlaceAnnotation()
    {
        Annotation.localPosition = new Vector3(0, 0, AnnotationDepth * -.1f);
        Annotation.localScale = new Vector3(AnnotationDepth, AnnotationDepth, AnnotationDepth);
    }
}
