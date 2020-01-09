using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeProxies : MonoBehaviour
{
    public static HandPrototypeProxies Instance;

    public bool ShowProxies;

    public Transform LeftPalm;
    public Transform LeftThumb;
    public Transform LeftIndex;
    public Transform LeftMiddle;
    public Transform LeftRing;
    public Transform LeftPinky;

    public Transform RightIndex;
    public Transform RightThumb;
    public Transform RightPalm;

    private IEnumerable<MeshRenderer> proxyRenderers;

    private void Awake()
    {
        Instance = this;
    }

    private void SetTransform(TrackedHandJoint joint, Transform toBindTo, Handedness handedness = Handedness.Left)
    {
        if (HandJointUtils.TryGetJointPose(joint, handedness, out MixedRealityPose trackedDigit))
        {
            toBindTo.position = trackedDigit.Position;
            toBindTo.rotation = trackedDigit.Rotation;
        }
    }

    private void SetHandTransforms()
    {
        SetTransform(TrackedHandJoint.Palm, LeftPalm);
        SetTransform(TrackedHandJoint.ThumbTip, LeftThumb);
        SetTransform(TrackedHandJoint.IndexTip, LeftIndex);
        SetTransform(TrackedHandJoint.MiddleTip, LeftMiddle);
        SetTransform(TrackedHandJoint.RingTip, LeftRing);
        SetTransform(TrackedHandJoint.PinkyTip, LeftPinky);
        
        SetTransform(TrackedHandJoint.IndexTip, RightIndex, Handedness.Right);
        SetTransform(TrackedHandJoint.ThumbTip, RightThumb, Handedness.Right);
        SetTransform(TrackedHandJoint.Palm, RightPalm, Handedness.Right);
    }

    private void Start()
    {
        proxyRenderers = new List<MeshRenderer>() {
            LeftPalm.GetComponent<MeshRenderer>(),
            LeftThumb.GetComponent<MeshRenderer>(),
            LeftIndex.GetComponent<MeshRenderer>(),
            LeftMiddle.GetComponent<MeshRenderer>(),
            LeftRing.GetComponent<MeshRenderer>(),
            LeftPinky.GetComponent<MeshRenderer>(),
            RightIndex.GetComponent<MeshRenderer>(),
            RightThumb.GetComponent<MeshRenderer>(),
            RightPalm.GetComponent<MeshRenderer>(),
        };
    }

    void Update()
    {
        SetHandTransforms();
        UpdateShowProxies();
        UpdateShaders();
    }

    private void UpdateShaders()
    {
        Shader.SetGlobalVector("_FingerPosition", RightIndex.position);
    }

    private void UpdateShowProxies()
    {
        foreach (MeshRenderer renderer in proxyRenderers)
        {
            renderer.enabled = ShowProxies;
        }
    }
}
