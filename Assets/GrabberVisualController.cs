using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberVisualController : MonoBehaviour
{
    [SerializeField]
    private SlateRepositioning repositioner;

    [SerializeField]
    private BoxFocusable focus;

    [SerializeField]
    private Transform grabberVisual;

    [SerializeField]
    private SkinnedMeshRenderer grabberMesh;

    float showness;

    public float Pinchedness { get; private set; }

    private Transform baseParent;
    private Vector3 baseOffset;

    private void Start()
    {
        baseParent = transform.parent;
        baseOffset = transform.localPosition;
    }

    void Update()
    {
        Pinchedness = GetPinchedness();
        UpdateShowness();
        UpdateMainPosition();
        grabberMesh.SetBlendShapeWeight(0, Pinchedness * 100);
    }

    private void UpdateMainPosition()
    {
        if(repositioner.CurrentlyRepositioning)
        {
            transform.SetParent(MainPinchDetector.Instance.PinchPoint);
        }
        else
        {
            transform.SetParent(baseParent);
            transform.localPosition = baseOffset;
            transform.localRotation = Quaternion.identity;
            UpdateUnpinchedRotation();
        }
    }

    private void UpdateUnpinchedRotation()
    {
        Quaternion handAlignmentRotation = GetHandAlignmentRotation();

        grabberVisual.rotation = Quaternion.Lerp(grabberVisual.rotation, GetHandAlignmentRotation(), Time.deltaTime * 5);
    }

    private Quaternion GetHandAlignmentRotation()
    {
        Vector3 forwardDir = (transform.position - MainPinchDetector.Instance.PalmProxy.position).normalized;
        Vector3 indexPos = MainPinchDetector.Instance.ThumbProxy.position;
        Vector3 thumbPos = MainPinchDetector.Instance.FingertipProxy.position;
        Vector3 cross = (indexPos - thumbPos).normalized;
        Vector3 up = Vector3.Cross(forwardDir, cross);
        return Quaternion.LookRotation(forwardDir, up);
    }

    private float GetPinchedness()
    {
        if (FocusManager.Instance.FocusedItem != focus)
        {
            return 0;
        }
        if (repositioner.CurrentlyRepositioning)
        {
            return 1;
        }
        if (MainPinchDetector.Instance.Pinching && !repositioner.CurrentlyRepositioning)
        {
            return 0;
        }
        float pinchProg = (MainPinchDetector.Instance.FingerDistance - .03f) / MainPinchDetector.Instance.PinchDist;
        return 1 - Mathf.Clamp01(pinchProg);
    }

    private void UpdateShowness()
    {
        bool shouldShow = FocusManager.Instance.FocusedItem == focus;
        float shownessTarget = shouldShow ? 1 : 0;
        showness = Mathf.Lerp(showness, shownessTarget, Time.deltaTime * 10);
    }
}
