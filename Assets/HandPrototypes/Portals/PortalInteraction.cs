using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalInteraction : MonoBehaviour
{
    private bool wasPinching;
    public PinchDetector Pinching;

    void Update()
    {
        if(Pinching.Pinching && !wasPinching)
        {
            transform.parent = Pinching.PinchPoint;
        }
        if(!Pinching.Pinching)
        {
            transform.parent = null;
        }
        wasPinching = Pinching.Pinching;
    }
}
