using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMountedUiController : StandardHandPrototype
{
    private void Update()
    {
        UpdatePrimaryVisibility();
        UpdatePosition();
    }
}
