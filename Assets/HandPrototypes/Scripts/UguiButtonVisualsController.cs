using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PushyButtonController))]
public class UguiButtonVisualsController : ButtonVisualController
{
    [SerializeField]
    private IconController iconController;


    [SerializeField]
    private IconController.IconKey toggledKey;
    [SerializeField]
    private IconController.IconKey untoggledKey;

    private Material backgroundMat;

    [SerializeField]
    private float iconZRaiseAmount = -.4f;
    private float iconZ;

    private RectTransform backgroundRect;

    [SerializeField]
    private Image buttonBackground;
    private void Start()
    {
        buttonBackground.material = new Material(buttonBackground.material);
        backgroundMat = buttonBackground.material;
        button = GetComponent<PushyButtonController>();
        backgroundRect = buttonBackground.GetComponent<RectTransform>();
        button.Pressed += Button_Pressed;
    }

    private void Button_Pressed(object sender, EventArgs e)
    {
        if(button.InteractionStyle == PushyButtonController.ButtonInteractionStyles.ToggleButton)
        {
            iconController.Icon = button.Toggled ? untoggledKey : toggledKey;
        }

        backgroundMat.SetVector("_PulsePos", Hands.Instance.RightHandProxy.IndexTip.position);
    }

    private void Update()
    {
        UpdateIconPosition();
        UpdateColors();
        UpdatePulse();
    }

    private void UpdateIconPosition()
    {
        float zTarget = GetIconZTarget();
        iconZ = Mathf.Lerp(iconZ, zTarget, Time.deltaTime * 10);
        iconController.transform.localPosition = new Vector3(iconController.transform.localPosition.x, iconController.transform.localPosition.y, iconZ);
    }

    private float GetIconZTarget()
    {
        if(button.State == ButtonState.Hovered)
        {
            float dist = GetDistToFinger(backgroundRect.transform, Hands.Instance.RightHandProxy.IndexTip.position);
            return Mathf.Max(dist, iconZRaiseAmount);
        }
        if(button.State == ButtonState.Pressed)
        {
            return iconZRaiseAmount;
        }
        return 0;
    }

    private float GetDistToFinger(Transform transform, Vector3 position)
    {
        Plane plane = new Plane(transform.forward, transform.position);
        return plane.GetDistanceToPoint(position);
    }

    private void UpdatePulse()
    {
        float prog = Mathf.Clamp01(button.Outro / button.OutroDuration);
        backgroundMat.SetFloat("_Progression", prog);
    }

    private void UpdateColors()
    {
        Color colorTarget = GetStateColor();
        buttonBackground.color = Color.Lerp(buttonBackground.color, colorTarget, Time.deltaTime * 15);
    }
}
