using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorVisualController : MonoBehaviour
{
    [SerializeField]
    private Transform cursorPosition;

    void Update()
    {
        Shader.SetGlobalVector("_CursorPos", cursorPosition.position);
    }
}
