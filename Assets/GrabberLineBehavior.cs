using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberLineBehavior : MonoBehaviour
{
    [SerializeField]
    private GrabbableSlateVisuals slateVisuals;
    [SerializeField]
    private GrabberComponentHalf halfA;
    [SerializeField]
    private GrabberComponentHalf halfB;

    [SerializeField]
    private BoxFocusable focus;

    float showness;

    void Update()
    {
        UpdateShowness();
        float diskADist = (halfA.DiskOnPanel.position - MainPinchDetector.Instance.FingertipProxy.position).magnitude;
        float diskBDist = (halfB.DiskOnPanel.position - MainPinchDetector.Instance.FingertipProxy.position).magnitude;

        float pinchness = 1 - slateVisuals.Pinchedness;
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

    private void UpdateShowness()
    {
        bool shouldShow = FocusManager.Instance.FocusedItem == focus
            && !MainPinchDetector.Instance.Pinching;
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
