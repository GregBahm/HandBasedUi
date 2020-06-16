using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPaneController : MonoBehaviour
{
    [SerializeField]
    private SlateRepositioning repositioning;
    [SerializeField]
    private SlateResizing resizing;
    [SerializeField]
    private PushyButtonController closeButton;
    [SerializeField]
    private PushyButtonController minimizeButton;

    private void Start()
    {
        closeButton.Pressed += CloseButton_Pressed;
        minimizeButton.Pressed += MinimizeButton_Pressed;
    }

    private void MinimizeButton_Pressed(object sender, EventArgs e)
    {

    }

    private void CloseButton_Pressed(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
    }
    

    private void Update()
    {
        repositioning.DoUpdate();
        resizing.DoUpdate();
    }
}
