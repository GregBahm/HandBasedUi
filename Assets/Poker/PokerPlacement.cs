using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerPlacement : MonoBehaviour
{
    public SummonDetector SummonDetector;
    public Transform Pivot;

    private void Update()
    {
        if(SummonDetector.IsSummoned)
        {
            Vector3 positionTarget = HandPrototypeProxies.Instance.LeftPalm.position;
            transform.position = Vector3.Lerp(transform.position, positionTarget, Time.deltaTime * 5);
            Pivot.LookAt(Camera.main.transform);
            Pivot.Rotate(0, 180, 0);
        }
    }
}
