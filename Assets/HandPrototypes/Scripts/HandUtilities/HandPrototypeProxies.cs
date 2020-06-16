using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeProxies : MonoBehaviour
{
    public static HandPrototypeProxies Instance;
    
    [SerializeField]
    private Hands handsSource;

    public Transform LeftPalm { get { return handsSource.LeftHandProxy.Palm; } }
    public Transform LeftThumb { get { return handsSource.LeftHandProxy.ThumbTip; } }
    public Transform LeftIndex { get { return handsSource.LeftHandProxy.IndexTip; } }
    public Transform LeftMiddle { get { return handsSource.LeftHandProxy.MiddleTip; } }
    public Transform LeftRing { get { return handsSource.LeftHandProxy.RingTip; } }
    public Transform LeftPinky { get { return handsSource.LeftHandProxy.LittleTip; } }

    public Transform RightIndex { get { return handsSource.RightHandProxy.IndexTip; } }
    public Transform RightThumb { get { return handsSource.RightHandProxy.ThumbTip; } }
    public Transform RightPalm { get { return handsSource.RightHandProxy.Palm; } }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        UpdateShaders();
    }

    private void UpdateShaders()
    {
        Shader.SetGlobalVector("_FingerPosition", RightIndex.position);
    }
}
