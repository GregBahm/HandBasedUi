using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateThumbnailBehavior : MonoBehaviour
{
    public HandMountedUiController HandMount;
    public BoxFocusable Focus;
    public GameObject Visuals;
    public MeshRenderer VideoMesh;
    [SerializeField]
    private SlateController slate;
    public float SummonDuration = 1;
    public float SummonRamp = 1;
    private float summonTime;
    private bool thumbnailGrabbed;
    private Transform summonedPosition;
    private Material boxMaterial;

    private void Start()
    {
        summonedPosition = new GameObject("Thumnail summonedPosition").transform;
        summonedPosition.SetParent(transform, false);
        summonedPosition.SetParent(transform.parent, true);
        boxMaterial = VideoMesh.material;
    }

    public void UpdateThumbnail()
    {
        UpdateIntro();
        HandleThumbnailGrab();
    }

    private void UpdateIntro()
    {
        transform.position = Vector3.Lerp(HandPrototypeProxies.Instance.LeftPalm.position,
            summonedPosition.position,
            HandMount.Summonedness);
        boxMaterial.SetFloat("_Fade", 1f - HandMount.Summonedness);
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

        slate.Summonness = Mathf.Pow(param, SummonRamp);
        thumbnailGrabbed = thumbnailGrabbed && MainPinchDetector.Instance.Pinching;
        Visuals.SetActive(!thumbnailGrabbed);
    }

    private void StartThumbnailGrab()
    {
        summonTime = SummonDuration;
        slate.transform.position = transform.position;
        slate.transform.rotation = transform.rotation;
        slate.Repositioning.StartGrab();
        thumbnailGrabbed = true;
    }
}
