using UnityEngine;

public class SubPincher
{
    private Transform thumbProxy;
    private Transform fingerProxy;

    public SubPincher(Transform thumbProxy, Transform fingerProxy)
    {
        this.thumbProxy = thumbProxy;
        this.fingerProxy = fingerProxy;
    }

    public bool GetIsPinching()
    {
        float tipDistance = (thumbProxy.position - fingerProxy.position).magnitude;
        return tipDistance < MainPinchDetector.Instance.PinchDist;

    }
}