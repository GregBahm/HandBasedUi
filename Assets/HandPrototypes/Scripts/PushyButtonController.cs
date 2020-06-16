using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PushyButtonController : MonoBehaviour
{
    public bool IsFocused { get { return FocusManager.Instance.FocusedItem == focus; } }

    public Color CurrentColor { get; private set; }

    public event EventHandler Pressed;

    [SerializeField]
    private ScreenspaceFocusable focus;

    [SerializeField]
    private Transform buttonContent;

    [SerializeField]
    private Renderer sphereRenderer;

    [SerializeField]
    private TMP_Text label;

    [SerializeField]
    private AudioSource pressSound;

    [SerializeField]
    private AudioSource releaseSound;

    [SerializeField]
    private float distToFlex = 1;

    [SerializeField]
    private LineRenderer lineRenderer;
    private Material lineMat;

    [SerializeField]
    private ButtonStyle style;
    private ButtonStyling styling;

    public bool Toggled;

    private Color currentGlowColor;
    private float currentLabelAlpha;
    private ButtonState oldState;
    private ButtonState state;

    private Material sphereMeshMat;

    private float baseGlobalSize;
    private Vector3 baseLocalSize;

    private float highlightness;

    private float pressedness;

#if UNITY_EDITOR
    [SerializeField]
    private bool testClick;
#endif

    private void Start()
    {
        styling = ButtonStylingManager.Instance.GetStyling(style);
        sphereMeshMat = sphereRenderer.material;
        state = ButtonState.Ready;
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
#if UNITY_EDITOR
        if(testClick)
        {
            testClick = false;
            OnPressed();
        }
#endif
        state = UpdateState();
        if(state == ButtonState.Pressed && oldState != ButtonState.Pressed)
        {
            OnPressed();
        }
        oldState = state;
        UpdateOuterRing();
        UpdateMaterials();
        UpdateSize();
    }

    private float outro;

    private void UpdateOuterRing()
    {
        float highlightnessTarget = IsFocused ? 1f : 0;
        highlightness = Mathf.Lerp(highlightness, highlightnessTarget, Time.deltaTime * 10);

        float flex;
        if(state == ButtonState.Pressed)
        {
            outro += Time.deltaTime;
            flex = outro / .3f;
            flex = Mathf.Pow(flex, .5f);
        }
        else
        {
            outro = 0;
            flex = Mathf.Max(0, -pressedness) / distToFlex;
        }
        float brightness = Mathf.Clamp01(1 - flex) * highlightness;
        lineMat.SetFloat("_Brightness", brightness);
        float lineScale = flex + 1;
        lineRenderer.transform.localScale = new Vector3(lineScale, lineScale, lineScale);
    }

    private void OnPressed()
    {
        pressSound.Play();
        EventHandler handler = Pressed;
        handler?.Invoke(this, EventArgs.Empty);
    }

    
    private ButtonState UpdateState()
    {
        if(IsFocused)
        {
            Vector3 localPos = GetLocalFingerPosition();
            pressedness = localPos.z;
            if(localPos.z < 0 && (state == ButtonState.Ready || state == ButtonState.Hovered))
            {
                return ButtonState.Hovered;
            }
            if((state == ButtonState.Hovered || state == ButtonState.Pressed) && localPos.z > 0)
            {
                return ButtonState.Pressed;
            }
        }
        return ButtonState.Ready;
    }

    private Vector3 GetLocalFingerPosition()
    {
        Vector3 fingerPos = Hands.Instance.RightHandProxy.IndexTip.position;
        return transform.InverseTransformPoint(fingerPos);
    }

    private void UpdateSize()
    {
        Vector3 scaleTarget = GetScaleTarget();
        transform.localScale = Vector3.Lerp(transform.localScale, scaleTarget, Time.deltaTime * 4);
    }

    private Vector3 GetScaleTarget()
    {
        if (IsFocused)
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
        Color targetGlowColor = colorTarget * (state == ButtonState.Hovered ? 1 : 0);
        currentGlowColor = Color.Lerp(currentGlowColor, targetGlowColor, Time.deltaTime * 15);

        sphereMeshMat.SetColor("_Color", CurrentColor);
        sphereMeshMat.SetFloat("_Disabledness", state == ButtonState.Disabled ? 1 : 0);

        float labelAlpha = IsFocused ? 1 : 0;
        currentLabelAlpha = Mathf.Lerp(currentLabelAlpha, labelAlpha, Time.deltaTime * 15);
        label.color = new Color(1, 1, 1, currentLabelAlpha);
    }

    private Color GetStateColor()
    {
        switch (state)
        {
            case ButtonState.Ready:
                return Toggled ? styling.ReadyToggledColor : styling.ReadyColor;
            case ButtonState.Hovered:
                return styling.HoverColor;
            case ButtonState.Pressed:
                return styling.PressingColor;
            case ButtonState.Disabled:
            default:
                return styling.DisabledColor;
        }
    }

    private enum ButtonState
    {
        Disabled,
        Ready,
        Hovered,
        Pressed,
        Released,
    }
    public enum ButtonInteractionStyles
    {
        ToggleButton,
        ClickButton
    }
}
