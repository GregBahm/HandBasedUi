using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SlateVisualController))]
public class MainSlateLightingController : MonoBehaviour
{
    public MainPaneController Main;
    private SlateVisualController baseVisualController;

    private Vector4[] buttonPositions;
    private Color[] buttonColors;

    private void Start()
    {
        this.baseVisualController = GetComponent<SlateVisualController>();
        buttonPositions = new Vector4[Main.Buttons.Length];
        buttonColors = new Color[Main.Buttons.Length];
    }

    private void Update()
    {
        for (int i = 0; i < Main.Buttons.Length; i++)
        {
            buttonColors[i] = Main.Buttons[i].CurrentGlowColor;
            buttonPositions[i] = GetButtonPosition(i);
        }
        baseVisualController.FillMaterial.SetVectorArray("_ButtonPositions", buttonPositions);
        baseVisualController.FillMaterial.SetColorArray("_ButtonColors", buttonColors);
    }

    private Vector4 GetButtonPosition(int i)
    {
        Vector3 pos = Main.Buttons[i].ButtonContent.transform.position;
        float scale = Main.Buttons[i].transform.lossyScale.x;
        return new Vector4(pos.x, pos.y, pos.z, scale);
    }
}
