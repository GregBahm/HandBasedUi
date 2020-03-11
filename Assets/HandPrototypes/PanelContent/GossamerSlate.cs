using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class GossamerSlate : MonoBehaviour
{
    public Transform Hand;
    public LineRenderer LineA;
    public LineRenderer LineB;

    public int Resolution;

    private void Start()
    {
        LineA.positionCount = Resolution;
        LineB.positionCount = Resolution;
    }

    private void Update()
    {
        
    }
}