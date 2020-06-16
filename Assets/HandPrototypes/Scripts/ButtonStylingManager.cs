using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStylingManager : MonoBehaviour
{
    [SerializeField]
    private ButtonStyling topButtonStyle;

    [SerializeField]
    private ButtonStyling bottomButtonStyle;

    [SerializeField]
    private ButtonStyling hangUpButtonStyle;

    public static ButtonStylingManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public ButtonStyling GetStyling(ButtonStyle style)
    {
        switch (style)
        {
            case ButtonStyle.TopButton:
                return topButtonStyle;
            case ButtonStyle.BottumButton:
                return bottomButtonStyle;
            case ButtonStyle.HangUpButton:
            default:
                return hangUpButtonStyle;
        }
    }
}

public enum ButtonStyle
{
    TopButton,
    BottumButton,
    HangUpButton
}
