using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeG : StandardHandPrototype
{
    public Transform GazeCursor;

    private void Update()
    {
        UpdatePrimaryVisibility();
        UpdatePosition();
        UpdateGazeCursor();
    }

    private void UpdateGazeCursor()
    {
        GazeCursor.position = Camera.main.transform.position + Camera.main.transform.forward;
    }
}
