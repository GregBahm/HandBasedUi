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

    private void Update()
    {
        repositioning.DoUpdate();
        resizing.DoUpdate();
    }
}
