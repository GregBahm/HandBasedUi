﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMountedUiController : MonoBehaviour
{
    [SerializeField]
    private float summonTransitionTime = .5f;
    public float SummonTransitionTime => summonTransitionTime;

    [SerializeField]
    private TMPro.TMP_Text recentDocumentsLabel;

    public static HandMountedUiController Instance { get; private set; }
    public Transform CoreTransform { get; private set; }

    public float Smoothing;
    public float SummonTime;
    private float currentSummonTime;
    public float Summonedness { get; private set; }
    public SummonDetector Summoning;

    public Transform HandContentRoot;
    public Transform HandLookPivot;

    [SerializeField]
    private ThumbnailGrabberBehavior[] grabbers;

    private void Awake()
    {
        CoreTransform = new GameObject("UiPositionCore").transform;
        Instance = this;
    }

    private void Update()
    {
        UpdateCorePosition();
        UpdateSummonedness();
        UpdatePrimaryVisibility();
        UpdatePosition();
    }

    private void UpdateSummonedness()
    {
        float summonednessTarget = Summoning.IsSummoned ? 1 : 0;
        Summonedness = Mathf.Lerp(Summonedness, summonednessTarget, Time.deltaTime * 10);
    }

    private Vector3 GetUpVector(Vector3 forward)
    {
        Vector3 fingerTipAverage = (Hands.Instance.LeftHandProxy.IndexTip.position +
            Hands.Instance.LeftHandProxy.MiddleTip.position +
            Hands.Instance.LeftHandProxy.LittleTip.position +
            Hands.Instance.LeftHandProxy.RingTip.position) / 4;
        Vector3 toPalm = Hands.Instance.LeftHandProxy.Palm.position - fingerTipAverage;
        return Vector3.Cross(toPalm, forward);
    }

    private Vector3 GetForwardVector()
    {
        Vector3 pointA = (Hands.Instance.LeftHandProxy.IndexTip.position + Hands.Instance.LeftHandProxy.MiddleTip.position) / 2;
        Vector3 pointB = (Hands.Instance.LeftHandProxy.RingTip.position + Hands.Instance.LeftHandProxy.LittleTip.position) / 2;

        Vector3 toPalmA = pointA - Hands.Instance.LeftHandProxy.Palm.position;
        Vector3 toPalmB = pointB - Hands.Instance.LeftHandProxy.Palm.position;
        return Vector3.Cross(toPalmA, toPalmB);
    }

    private void UpdateCorePosition()
    {
        CoreTransform.position = Hands.Instance.LeftHandProxy.Palm.position;
        Vector3 psuedoPalmForward = GetForwardVector();
        Vector3 psuedoPalmUp = GetUpVector(psuedoPalmForward);
        CoreTransform.rotation = Quaternion.LookRotation(psuedoPalmUp, -psuedoPalmForward);
    }

    private void UpdatePrimaryVisibility()
    {
        foreach (ThumbnailGrabberBehavior item in grabbers)
        {
            if(Summoning.JustSummoned)
            {
                item.DoReset();
            }
            item.DoUpdate(Summoning.IsSummoned);
        }
        recentDocumentsLabel.gameObject.SetActive(Summoning.IsSummoned);
    }

    private void UpdatePosition()
    {
        Vector3 positionTarget = Vector3.Lerp(HandContentRoot.position, CoreTransform.position, Time.deltaTime * Smoothing);
        HandContentRoot.position = positionTarget;
        HandContentRoot.LookAt(Camera.main.transform.position, Vector3.up);
        HandLookPivot.LookAt(Camera.main.transform.position, Vector3.up);
        HandLookPivot.Rotate(0, 180, 0);
    }
}
