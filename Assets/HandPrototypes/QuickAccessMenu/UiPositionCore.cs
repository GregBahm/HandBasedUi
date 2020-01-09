using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPositionCore : MonoBehaviour
{
    private HandPrototypeProxies proxies;

    public Transform CoreTransform { get; private set; }

    private void Awake()
    {
        CoreTransform = new GameObject("UiPositionCore").transform;
    }

    private void Start()
    {
        proxies = HandPrototypeProxies.Instance;
    }

    private void Update()
    {
        UpdateCorePosition();
    }

    private void UpdateCorePosition()
    {
        CoreTransform.position = proxies.LeftPalm.position;
        Vector3 psuedoPalmForward = GetForwardVector();
        Vector3 psuedoPalmUp = GetUpVector(psuedoPalmForward);
        CoreTransform.rotation = Quaternion.LookRotation(psuedoPalmUp, -psuedoPalmForward);
    }
    private Vector3 GetUpVector(Vector3 forward)
    {
        Vector3 fingerTipAverage = (proxies.LeftIndex.position +
            proxies.LeftMiddle.position +
            proxies.LeftPinky.position +
            proxies.LeftRing.position) / 4;
        Vector3 toPalm = proxies.LeftPalm.position - fingerTipAverage;
        return Vector3.Cross(toPalm, forward);
    }


    private Vector3 GetForwardVector()
    {
        Vector3 pointA = (proxies.LeftIndex.position + proxies.LeftMiddle.position) / 2;
        Vector3 pointB = (proxies.LeftRing.position + proxies.LeftPinky.position) / 2;

        Vector3 toPalmA = pointA - proxies.LeftPalm.position;
        Vector3 toPalmB = pointB - proxies.LeftPalm.position;
        return Vector3.Cross(toPalmA, toPalmB);
    }

}
