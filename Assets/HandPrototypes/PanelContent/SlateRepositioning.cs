using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateRepositioning : MonoBehaviour
{
    [SerializeField]
    private BoxFocusable focus;
    public BoxFocusable Focus { get { return this.focus; } }
    
    [SerializeField]
    private float deadZoneMoveDistance;

    [SerializeField]
    private float snappingThreshold;
    
    public float Smoothing;
    public float SnapThreshold;
    private bool wasPinching;
    
    public bool CurrentlyRepositioning { get; private set; }
    private Transform unsnappedTransform;
    private SnappedRepositioner snappedRepositioner;
    private FinalRepositioner finalRepositioner;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Start()
    {
        unsnappedTransform = new GameObject("Unsnapped Transform").transform;
        unsnappedTransform.position = transform.position;
        unsnappedTransform.rotation = transform.rotation;
        snappedRepositioner = new SnappedRepositioner();
        finalRepositioner = new FinalRepositioner();

        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }
    
    public void DoUpdate()
    {
        snappedRepositioner.UpdateSnappedPivot();

        bool pinching = MainPinchDetector.Instance.Pinching;
        if(CurrentlyRepositioning)
        {
            if(pinching)
            {
                UpdatePositionTargets();
            }
            else
            {
                EndRepositioning();
            }
        }
        bool shoulStartPinch = GetShouldStartGrab(pinching);
        if (shoulStartPinch)
        {
            StartGrab();
        }
        UpdatePosition();
        wasPinching = pinching;
    }

    private void UpdatePosition()
    {
        Vector3 deadzoneTarget = GetDeadzoneTarget();

        transform.position = Vector3.Lerp(transform.position, deadzoneTarget, Time.deltaTime * Smoothing);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * Smoothing);
    }

    private Vector3 GetDeadzoneTarget()
    {
        Vector3 toTarget = targetPosition - transform.position;
        float distToTarget = toTarget.magnitude;
        float deadDist = Mathf.Max(0, distToTarget - deadZoneMoveDistance);
        return transform.position + toTarget.normalized * deadDist;
    }

    private void EndRepositioning()
    {
        CurrentlyRepositioning = false;
        unsnappedTransform.parent = null;
        Focus.ForceFocus = false;
    }

    private void UpdatePositionTargets()
    {
        finalRepositioner.UpdateFinalPivot(unsnappedTransform, snappedRepositioner.Transform, snappingThreshold);
        
        targetPosition = finalRepositioner.Transform.position;
        targetRotation = finalRepositioner.Transform.rotation;
    }
    
    private bool GetShouldStartGrab(bool pinching)
    {
        return pinching && !wasPinching && FocusManager.Instance.FocusedItem == Focus;
    }

    public void StartGrab()
    {
        CurrentlyRepositioning = true;
        unsnappedTransform.position = transform.position;
        unsnappedTransform.rotation = transform.rotation;
        unsnappedTransform.parent = MainPinchDetector.Instance.PinchPoint;

        snappedRepositioner.StartGrab(transform);
        finalRepositioner.StartGrab(transform);
        Focus.ForceFocus = true;
    }

    private class FinalRepositioner
    {
        public Transform Transform { get; }
        private Transform pivot;

        public FinalRepositioner()
        {
            Transform = new GameObject("Final Transform Proxy").transform;
            pivot = new GameObject("Final Pivot").transform;
            pivot.parent = MainPinchDetector.Instance.PinchPoint;
            pivot.localPosition = Vector3.zero;

            Transform.parent = pivot;
        }

        public void UpdateFinalPivot(Transform unsnappedTransform, Transform snappedTransform, float snappingThreshold)
        {
            Vector3 unsnappedEuler = unsnappedTransform.rotation.eulerAngles;
            Vector3 snappedEuler = snappedTransform.rotation.eulerAngles;
            Vector3 finalEuler = GetFinalEuler(unsnappedEuler, snappedEuler, snappingThreshold);
            pivot.rotation = Quaternion.Euler(finalEuler);
        }

        private Vector3 GetFinalEuler(Vector3 unsnappedEuler, Vector3 snappedEuler, float snappingThreshold)
        {
            float finalX = GetFinalDimension(unsnappedEuler.x, snappedEuler.x, snappingThreshold);
            float finalY = GetFinalDimension(unsnappedEuler.y, snappedEuler.y, snappingThreshold);
            float finalZ = GetFinalDimension(unsnappedEuler.z, snappedEuler.z, snappingThreshold);
            return new Vector3(finalX, finalY, finalZ);
        }

        private float GetFinalDimension(float unsnappedRot, float snappedRot, float snappingThreshold)
        {
            // Return snapped rot if the difference between snapped and unsnapped is below the threshold
            // rots will be between -180 and 180. So I could add 180 to both. 0  to 360. Then subtract. Then if one is 355 and the other is 5, so they have a difference of 350, I do 
            // min diff, 360 - diff
            float offsetUnsnappedRot = unsnappedRot + 180;
            float offsetSnappedRot = snappedRot + 180;
            float diff = Math.Abs(offsetUnsnappedRot - offsetSnappedRot);
            diff = Mathf.Min(diff, 360 - diff);
            if(diff < snappingThreshold)
            {
                return snappedRot;
            }
            return unsnappedRot;
        }

        public void StartGrab(Transform contentTransform)
        {
            pivot.rotation = contentTransform.rotation;
            Transform.position = contentTransform.position;
            Transform.localRotation = Quaternion.identity;
        }
    }

    private class SnappedRepositioner
    {
        public Transform Transform { get; }
        private Transform pivot;

        public SnappedRepositioner()
        {
            Transform = new GameObject("Snapped Transform Proxy").transform;
            pivot = new GameObject("Snapped Pivot").transform;
            pivot.parent = MainPinchDetector.Instance.PinchPoint;
            pivot.localPosition = Vector3.zero;

            Transform.parent = pivot;
        }

        public void UpdateSnappedPivot()
        {
            Vector3 forward = Transform.position - Camera.main.transform.position;
            Quaternion lookRot = Quaternion.LookRotation(forward, Vector3.up);
            pivot.rotation = lookRot;
        }

        public void StartGrab(Transform contentTransform)
        {
            pivot.rotation = contentTransform.rotation;
            Transform.position = contentTransform.position;
            Transform.localRotation = Quaternion.identity;
        }
    }
}
