﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeE : HandPrototype
{
    private bool thumbnailsHovered;
    private float morphness;

    public MeshCollider HoverZone;
    public float RingRadius;
    public float SummonTime;
    private float currentSummonTime;
    public float Summonedness { get; private set; }
    public SummonDetector Summoning;

    public Transform HandContent;
    public Transform HandRotationPivot;

    public MorphingThumbnail[] Morphers;

    public override bool IsSummoned
    {
        get
        {
            return Summoning.IsSummoned;
        }
    }

    protected void UpdatePrimaryVisibility()
    {
        HandContent.gameObject.SetActive(Summoning.IsSummoned);
    }

    protected void UpdatePosition()
    {
        Vector3 positionBase = HandPrototypeProxies.Instance.LeftPalm.position;

        Vector3 positionTarget = Vector3.Lerp(HandContent.position, positionBase, Time.deltaTime * Smoothing);
        HandContent.position = positionTarget;
        HandRotationPivot.LookAt(Camera.main.transform.position, Vector3.up);
    }

    private void Update()
    {
        UpdatePrimaryVisibility();
        UpdatePosition();
        thumbnailsHovered = GetIsHovered(HoverZone);
        UpdateMorphing();
    }
    
    private void UpdateMorphing()
    {
        float morphnessTarget = thumbnailsHovered ? 1 : 0;
        morphness = Mathf.Lerp(morphness, morphnessTarget, Time.deltaTime * 10);
        foreach (MorphingThumbnail thumbnail in Morphers)
        {
            thumbnail.Morph(morphness);
        }
    }

    private bool GetIsHovered(MeshCollider collider)
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(HandPrototypeProxies.Instance.RightIndex.position - HoverZone.transform.forward, HoverZone.transform.forward);
        return collider.Raycast(ray, out hitInfo, float.PositiveInfinity);
    }
}
