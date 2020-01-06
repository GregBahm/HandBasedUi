using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelThumbnailBehavior : MonoBehaviour
{
    public BoxFocusable Focus;
    public GameObject Visuals;
    public MainPanelArrangement Panel; // TODO: Change this to an interface or something
    public float SummonDuration = 1;
    public float SummonRamp = 1;
    private float summonTime;
    private bool thumbnailGrabbed;

    public void UpdateThumbnail()
    {
        HandleThumbnailGrab();
    }

    private bool ShouldStartThumbnailGrab()
    {
        return FocusManager.Instance.FocusedItem == Focus && MainPinchDetector.Instance.PinchBeginning;
    }

    private void HandleThumbnailGrab()
    {
        if (ShouldStartThumbnailGrab())
        {
            StartThumbnailGrab();
        }
        summonTime -= Time.deltaTime;
        summonTime = Mathf.Clamp(summonTime, 0, SummonDuration);
        float param = 1 - (summonTime / SummonDuration);

        Panel.Summonness = Mathf.Pow(param, SummonRamp);
        thumbnailGrabbed = MainPinchDetector.Instance.Pinching;
        Visuals.SetActive(!thumbnailGrabbed);
    }

    private void StartThumbnailGrab()
    {
        summonTime = SummonDuration;
        Panel.transform.position = transform.position;
        Panel.transform.rotation = transform.rotation;
        Panel.Repositioning.StartGrab();
        thumbnailGrabbed = true;
    }
}
