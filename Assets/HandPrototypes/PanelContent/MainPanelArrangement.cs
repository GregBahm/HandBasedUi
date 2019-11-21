using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanelArrangement : MonoBehaviour
{
    [Range(0, 1)]
    public float Summonness;

    public Transform VideoCard;
    public Transform Slate;
    public Transform NamePlate;
    public float VerticalButtonMargin;
    public float HorizontalSlateMargin;
    public float VerticalSlateMargin;
    public float BottomButtonSpacing;
    public float NameplateMargin;

    private void Update()
    {
        PositionSlate();
        PositionNamePlate();
    }

    private void PositionNamePlate()
    {
        float x = -VideoCard.localScale.x / 2 - NameplateMargin;
        float y = VideoCard.localScale.y / 2 + NameplateMargin;
        NamePlate.localPosition = new Vector3(x, y, 0);
    }

    private void PositionSlate()
    {
        Vector3 slateScale = new Vector3(VideoCard.localScale.x + HorizontalSlateMargin, VideoCard.localScale.y + VerticalSlateMargin, 1);
        Slate.localScale = slateScale;
    }
}
