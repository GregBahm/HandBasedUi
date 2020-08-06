using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UguiFocusable : FocusableItemBehavior
{
    private RectTransform rec;

    void Start()
    {
        rec = GetComponent<RectTransform>();
        FocusManager.Instance.RegisterFocusable(this);
    }

    public override float GetDistanceToPointer(Vector3 pointerPos)
    {
        Vector3 transformedPoint = rec.InverseTransformPoint(pointerPos);
        float x = (transformedPoint.x - rec.rect.xMin) / rec.rect.width;
        float y = (transformedPoint.y - rec.rect.yMin) / rec.rect.height;
        if(IsWithinBounds(x, y))
        {
            return Mathf.Abs(transformedPoint.z);
        }
        return float.PositiveInfinity;
    }

    private bool IsWithinBounds(float x, float y)
    {
        return x > 0 && x < 1 && y > 0 && y < 1;
    }
}
