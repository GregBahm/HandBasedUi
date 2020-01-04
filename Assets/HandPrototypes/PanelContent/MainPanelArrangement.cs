using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanelArrangement : MonoBehaviour
{
    public const float VideoWidthHeightRatio = 1200f / 720;

    [Range(0, 1)]
    public float Summonness;

    public float ButtonSummoness
    {
        get
        {
            float remap = Mathf.Clamp01((Summonness - .5f) * 2);
            return Mathf.Pow(remap, 2);
        }
    }

    public float PanelSummoness
    {
        get
        {
            float remap = Mathf.Clamp01(Summonness * 2);
            return Mathf.Pow(Summonness, .7f);
        }
    }

    public Transform VideoCard;
    public Transform Slate;
    public Transform NamePlate;
    public BoxFocusable Thumbnail;
    public SlateResizing Resizing;
    public SlateRepositioning Repositioning;
    public VideoCardTopButtons TopButtons;
    public VideoCardBottomButtons ButtomButtons;

    public float ButtonHoverDist;
    public float VerticalButtonMargin;
    public float HorizontalSlateMargin;
    public float VerticalSlateMargin;
    [Range(0, .1f)]
    public float BottomButtonSpacing;
    public float NameplateMargin;

    public float SummonDuration = 1;
    public float SummonRamp = 1;
    private float summonTime;
    private bool thumbnailGrabbed;
    private Vector3 unsummonedSlateScale;
    
    private void Update()
    {
        PositionThumbnail();
        HandleThumbnailGrab();
        HandleSlateSizing();
        Repositioning.UpdateSlatePositioning();
        PositionVideoCard();
        PositionNamePlate();
        TopButtons.PlaceButtons();
        ButtomButtons.PlaceButtons();
    }

    private void PositionThumbnail()
    {
        Thumbnail.transform.position = HandPrototypeProxies.Instance.LeftPalm.position + new Vector3(0, .1f, 0);
    }

    private void HandleThumbnailGrab()
    {
        if(ShouldStartThumbnailGrab())
        {
            StartThumbnailGrab();
        }
        summonTime -= Time.deltaTime;
        summonTime = Mathf.Clamp(summonTime, 0, SummonDuration);
        Summonness = 1 - (summonTime / SummonDuration);
        Summonness = Mathf.Pow(Summonness, SummonRamp);
        thumbnailGrabbed = MainPinchDetector.Instance.Pinching;
        Thumbnail.gameObject.SetActive(!thumbnailGrabbed);
    }

    private void StartThumbnailGrab()
    {
        summonTime = SummonDuration;
        transform.position = Thumbnail.transform.position;
        transform.rotation = Thumbnail.transform.rotation;
        Repositioning.StartGrab();
        thumbnailGrabbed = true;
    }

    private bool ShouldStartThumbnailGrab()
    {
        return FocusManager.Instance.FocusedItem == Thumbnail && MainPinchDetector.Instance.PinchBeginning;
    }

    private void HandleSlateSizing()
    {
        if (Summonness < .99f)
        {
            Slate.localScale = Vector3.Lerp(Thumbnail.transform.localScale, unsummonedSlateScale, Summonness);
        }
        else
        {
            Resizing.UpdateSlateResizing();
            unsummonedSlateScale = Slate.localScale;
        }
    }

    private void PositionVideoCard()
    {
        float effectiveHorizontalMargin = HorizontalSlateMargin * PanelSummoness;
        float effectiveVerticalMargin = VerticalSlateMargin * PanelSummoness;
        Vector3 videoCardScale = new Vector3(Slate.localScale.x - effectiveHorizontalMargin, Slate.localScale.y - effectiveVerticalMargin, 1);
        Vector3 clampedVideoCardScale = GetClampedVideocardScale(videoCardScale);
        VideoCard.localScale = clampedVideoCardScale;
    }

    private Vector3 GetClampedVideocardScale(Vector3 availableSpace)
    {
        float ratio = availableSpace.x / availableSpace.y;
        if(ratio < VideoWidthHeightRatio)
        {
            float newY = availableSpace.x * (1 / VideoWidthHeightRatio);
            return new Vector3(availableSpace.x, newY, 1);
        }
        else
        {
            float newX = availableSpace.y * (VideoWidthHeightRatio);
            return new Vector3(newX, availableSpace.y, 1);
        }
    }

    private void PositionNamePlate()
    {
        float x = -VideoCard.localScale.x / 2 - NameplateMargin;
        float y = VideoCard.localScale.y / 2 + NameplateMargin;
        NamePlate.localPosition = new Vector3(x, y, NamePlate.localPosition.z);


        NamePlate.localScale = new Vector3(Summonness, Summonness, Summonness);
    }
}
