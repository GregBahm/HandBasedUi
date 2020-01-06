using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SlateVisualController))]
public class MainSlateLightingController : MonoBehaviour
{
    private SlateVisualController baseVisualController;

    private Vector4[] buttonPositions;
    private Color[] buttonColors;
    public SphereButton[] Buttons;

    private void Start()
    {
        this.baseVisualController = GetComponent<SlateVisualController>();
        buttonPositions = new Vector4[Buttons.Length];
        buttonColors = new Color[Buttons.Length];
    }

    private void Update()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            buttonColors[i] = Buttons[i].CurrentGlowColor;
            buttonPositions[i] = GetButtonPosition(i);
        }
        baseVisualController.FillMaterial.SetVectorArray("_ButtonPositions", buttonPositions);
        baseVisualController.FillMaterial.SetColorArray("_ButtonColors", buttonColors);
    }

    private Vector4 GetButtonPosition(int i)
    {
        Vector3 pos = Buttons[i].ButtonContent.transform.position;
        float scale = Buttons[i].transform.lossyScale.x;
        return new Vector4(pos.x, pos.y, pos.z, scale);
    }
}
