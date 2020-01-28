using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationMain : MonoBehaviour
{
    [Range(1, 30)]
    public float AnnotationDepth;
    public Transform Annotation;

    public AnnotationRepositioner AnnotationPlane;
    public AnnotationRepositioner Phone;

    private void Update()
    {
        Phone.UpdateSlatePositioning();

        PlaceAnnotation();
    }

    private void PlaceAnnotation()
    {
        Annotation.localPosition = new Vector3(0, 0, AnnotationDepth * -.1f);
        Annotation.localScale = new Vector3(AnnotationDepth, AnnotationDepth, AnnotationDepth);
    }
}
