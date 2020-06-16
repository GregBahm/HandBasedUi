using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlateController : MonoBehaviour
{
    [SerializeField]
    private float contentRatio;

    public float PanelSummoness
    {
        get
        {
            float remap = Mathf.Clamp01(Summonness * 2);
            return Mathf.Pow(Summonness, .7f);
        }
    }

    [SerializeField]
    [Range(0, 1)]
    private float summonness;
    public float Summonness { get => this.summonness; set => this.summonness = value; }

    [SerializeField]
    private SlateRepositioning repositioning;
    public SlateRepositioning Repositioning { get { return this.repositioning; } }

    [SerializeField]
    private Transform slate;
    public Transform Slate { get { return this.slate; } }

    [SerializeField]
    private SlateThumbnailBehavior thumbnail;
    public SlateThumbnailBehavior Thumbnail { get { return this.thumbnail; } }

    [SerializeField]
    private Transform slateContent;
    public Transform SlateContent { get { return this.slateContent; } }
    
    [SerializeField]
    private float horizontalSlateMargin;
    [SerializeField]
    private float verticalSlateMargin;

    private Vector3 unsummonedSlateScale;
    
    protected void PositionContent()
    {
        float effectiveHorizontalMargin = this.horizontalSlateMargin * PanelSummoness;
        float effectiveVerticalMargin = this.verticalSlateMargin * PanelSummoness;
        Vector3 videoCardScale = new Vector3(Slate.localScale.x - effectiveHorizontalMargin, Slate.localScale.y - effectiveVerticalMargin, 1);
        Vector3 clampedVideoCardScale = GetClampedContentScale(videoCardScale);
        SlateContent.localScale = clampedVideoCardScale;
    }

    private Vector3 GetClampedContentScale(Vector3 availableSpace)
    {
        float ratio = availableSpace.x / availableSpace.y;
        if (ratio < this.contentRatio)
        {
            float newY = availableSpace.x * (1 / this.contentRatio);
            return new Vector3(availableSpace.x, newY, 1);
        }
        else
        {
            float newX = availableSpace.y * this.contentRatio;
            return new Vector3(newX, availableSpace.y, 1);
        }
    }
}
