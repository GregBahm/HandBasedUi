using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStyling : MonoBehaviour
{
    public MainPaneController Main;
    public float HoverRadius { get { return 0.03f; } }
    public Color ReadyColor = Color.black;
    public Color ReadyToggledColor = Color.gray;
    public Color HoverColor = Color.blue;
    public Color PressingColor = Color.cyan;
    public Color DisabledColor = Color.gray;

    public AudioSource HoverSound;
    public AudioSource ClickSound;
}
