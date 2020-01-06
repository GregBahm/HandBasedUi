using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStyling : MonoBehaviour
{
    public MainPanelArrangement Main;
    public float HoverRadius { get { return Main.ButtonHoverDist; } }
    public Color ReadyColor = Color.black;
    public Color ReadyToggledColor = Color.gray;
    public Color HoverColor = Color.blue;
    public Color PressingColor = Color.cyan;
    public Color DisabledColor = Color.gray;

    public AudioSource HoverSound;
    public AudioSource ClickSound;
}
