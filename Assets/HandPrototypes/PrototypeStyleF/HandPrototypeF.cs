﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeF : StandardHandPrototype
{
    private float currentHoverTime;
    public float HoverTime;

    public Transform HoverProgressBar;
    public GameObject CallControlButtons;

    public MeshCollider HoverZone;
    

    private void Update()
    {
        UpdatePrimaryVisibility();
        UpdatePosition();
        UpdateCallControlButtons();
    }

    private void UpdateCallControlButtons()
    {
        bool isHovered = GetIsHovered(HoverZone);
        currentHoverTime += isHovered ? Time.deltaTime : -Time.deltaTime;
        currentHoverTime = Mathf.Clamp(currentHoverTime, 0, HoverTime);
        float hoverness = currentHoverTime / HoverTime;
        HoverProgressBar.localScale = new Vector3(hoverness, 1, 1);

        bool showProgressbar = GetShouldShowProgressbar(hoverness);
        HoverProgressBar.gameObject.SetActive(showProgressbar);

        bool showCallControlButtons = GetShouldShowCallButtons(hoverness);
        CallControlButtons.SetActive(showCallControlButtons);
    }

    private bool GetShouldShowCallButtons(float hoverness)
    {
        return (hoverness > 0.99f);
    }

    private bool GetShouldShowProgressbar(float hoverness)
    {
        return hoverness > float.Epsilon && hoverness < 1;
    }

    private bool GetIsHovered(MeshCollider collider)
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(HandPrototypeProxies.Instance.RightIndex.position - HoverZone.transform.forward, HoverZone.transform.forward);
        return collider.Raycast(ray, out hitInfo, float.PositiveInfinity);
    }
}
