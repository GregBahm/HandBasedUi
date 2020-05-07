using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonDetector : MonoBehaviour
{
    private bool wasSummoned;
    public bool IsSummoned { get; private set; }

    /// <summary>
    /// True on the frame a new summoning occurs
    /// </summary>
    public bool JustSummoned { get; private set; }

    private HandMountedUiController PositionCore { get { return HandMountedUiController.Instance; } }

    [Range(0, 1)]
    public float PalmSummonThreshold;
    [Range(0, 1)]
    public float PalmDismissThreshold;

    private void Update()
    {
        IsSummoned = GetPrimaryVisibility();
        JustSummoned = IsSummoned && !wasSummoned;
        wasSummoned = IsSummoned;
    }

    private bool GetPrimaryVisibility()
    {
        Vector3 toCamera = (PositionCore.CoreTransform.position - Camera.main.transform.position).normalized;
        float palmDot = Vector3.Dot(toCamera, PositionCore.CoreTransform.up);

        if (IsSummoned)
        {
            return palmDot > PalmDismissThreshold;
        }
        return palmDot > PalmSummonThreshold;
    }
}
