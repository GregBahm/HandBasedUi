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
    private Image buttonBackground;
    private void Start()
    {
        backgroundMat = buttonBackground.material;
        button = GetComponent<PushyButtonController>();
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
        UpdateColors();
        UpdatePulse();
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
