using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberLineBehavior : MonoBehaviour
{
    [SerializeField]
    private SlateRepositioning repositioner;
    [SerializeField]
    private GrabberComponentHalf halfA;
    [SerializeField]
    private GrabberComponentHalf halfB;

    [SerializeField]
    private BoxFocusable focus;

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
        UpdateDisks();
    }

    private void UpdateDisks()
    {
        float diskADist = (halfA.DiskOnPanel.position - MainPinchDetector.Instance.FingertipProxy.position).magnitude;
        float diskBDist = (halfB.DiskOnPanel.position - MainPinchDetector.Instance.FingertipProxy.position).magnitude;

        float pinchness = 1 - Pinchedness;
        if (diskADist < diskBDist)
        {
            halfA.DoUpdate(MainPinchDetector.Instance.FingertipProxy.position, showness, pinchness);
            halfB.DoUpdate(MainPinchDetector.Instance.ThumbProxy.position, showness, pinchness);
        }
        else
        {
            halfA.DoUpdate(MainPinchDetector.Instance.ThumbProxy.position, showness, pinchness);
            halfB.DoUpdate(MainPinchDetector.Instance.FingertipProxy.position, showness, pinchness);
        }
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
        }
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

    [Serializable]
    public class GrabberComponentHalf
    {
        [SerializeField]
        private Transform diskForFinger;
        [SerializeField]
        private Transform diskOnPanel;
        public Transform DiskOnPanel => diskOnPanel;
        [SerializeField]
        private LineRenderer line;

        public void DoUpdate(Vector3 fingerTarget, float showness, float pinchedness)
        {
            diskOnPanel.localPosition = new Vector3(0, 0, .07f * pinchedness);

            diskForFinger.localScale = new Vector3(showness, showness, showness);
            Vector3 offsetFingerTarget = GetOffsetFingerTarget(fingerTarget);
            diskForFinger.position = Vector3.Lerp(diskOnPanel.position, offsetFingerTarget, showness);
            diskForFinger.LookAt(diskOnPanel);
            
            diskOnPanel.localScale = new Vector3(showness, showness, showness);

            line.SetPosition(0, diskOnPanel.position);
            line.SetPosition(1, diskForFinger.position);
            line.widthMultiplier = showness * 0.001f;
        }

        private Vector3 GetOffsetFingerTarget(Vector3 fingerTarget)
        {
            Vector3 toPanel = fingerTarget - diskOnPanel.position;
            if(toPanel.magnitude > .01f)
            {
                Vector3 shortened = toPanel.normalized * (toPanel.magnitude - 0.01f);
                return diskOnPanel.position + shortened;
            }
            return diskOnPanel.position;
        }
    }
}
