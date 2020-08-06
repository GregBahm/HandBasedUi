using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PushyButtonController))]
public class RoundButtonVisualsController : ButtonVisualController
{
    [SerializeField]
    private Renderer sphereRenderer;

    [SerializeField]
    private LineRenderer lineRenderer;
    private Material lineMat;

    private Material sphereMeshMat;

    private float baseGlobalSize;
    private Vector3 baseLocalSize;

    [SerializeField]
    private float hoverRadius = 0.03f;

    [SerializeField]
    private float distToFlex = 1;


    public Color CurrentColor { get; private set; }
    private Color currentGlowColor;
    private float currentLabelAlpha;
    private float highlightness;

    [SerializeField]
    private TMP_Text label;

    private void Start()
    {
        button = GetComponent<PushyButtonController>();
        sphereMeshMat = sphereRenderer.material;
        baseGlobalSize = transform.lossyScale.x;
        baseLocalSize = transform.localScale;
        lineMat = lineRenderer.material;
    }

    private IEnumerable<Vector3> CreateCircleVerts()
    {
        float increment = 360f / 32;
        for (int i = 0; i < 32; i++)
        {
            float angle = i * increment;
            angle = Mathf.Deg2Rad * angle;
            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);
            yield return new Vector3(x, y, 0) * .5f;
        }
    }

    private void Update()
    {
        UpdateOuterRing();
        UpdateMaterials();
        UpdateSize();
    }
    private void UpdateOuterRing()
    {
        float highlightnessTarget = button.IsFocused ? 1f : 0;
        highlightness = Mathf.Lerp(highlightness, highlightnessTarget, Time.deltaTime * 10);

        float flex = GetOuterRingFlex();
        float brightness = Mathf.Clamp01(1 - flex) * highlightness;
        lineMat.SetFloat("_Brightness", brightness);
        float lineScale = flex + 1;
        lineRenderer.transform.localScale = new Vector3(lineScale, lineScale, lineScale);
    }

    private float GetOuterRingFlex()
    {
        return button.State == ButtonState.Pressed ?
            Mathf.Pow(button.Outro / .3f, .5f) :
            Mathf.Max(0, -button.Pressedness) / distToFlex;
    }

    private void UpdateSize()
    {
        Vector3 scaleTarget = GetScaleTarget();
        transform.localScale = Vector3.Lerp(transform.localScale, scaleTarget, Time.deltaTime * 4);
    }

    private Vector3 GetScaleTarget()
    {
        if (button.IsFocused)
        {
            float factor = Mathf.Min(transform.lossyScale.x, baseGlobalSize);
            float newScale = baseGlobalSize / factor;
            newScale = Mathf.Pow(newScale, 2);
            return baseLocalSize * newScale;
        }
        else
        {
            return baseLocalSize;
        }
    }

    private void UpdateMaterials()
    {
        Color colorTarget = GetStateColor();
        CurrentColor = Color.Lerp(CurrentColor, colorTarget, Time.deltaTime * 15);
        Color targetGlowColor = colorTarget * (button.State == ButtonState.Hovered ? 1 : 0);
        currentGlowColor = Color.Lerp(currentGlowColor, targetGlowColor, Time.deltaTime * 15);

        sphereMeshMat.SetColor("_Color", CurrentColor);
        sphereMeshMat.SetFloat("_Disabledness", button.State == ButtonState.Disabled ? 1 : 0);

        float labelAlpha = button.IsFocused ? 1 : 0;
        currentLabelAlpha = Mathf.Lerp(currentLabelAlpha, labelAlpha, Time.deltaTime * 15);
        label.color = new Color(1, 1, 1, currentLabelAlpha);
    }
}
