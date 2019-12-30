using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoCardTopButtons : MonoBehaviour
{
    private float fullScale;
    public MainPanelArrangement MainPanel;
    public float MinMargin;

    public Transform[] Buttons;

    private void Start()
    {
        fullScale = Buttons[0].localScale.x;
    }

    public void PlaceButtons()
    {
        transform.localPosition = Vector3.zero;

        float videoSpan = MainPanel.VideoCard.localScale.x;

        float y = MainPanel.VideoCard.localScale.y / 2 + MainPanel.VerticalButtonMargin;
        float itemWidth = GetItemWidth(videoSpan);


        float xStart = -videoSpan / 2 + itemWidth / 2;
        float xEnd = videoSpan / 2 - itemWidth / 2;

        for (int i = 0; i < Buttons.Length; i++)
        {
            float param = (float)i / (Buttons.Length - 1);
            float x = Mathf.Lerp(xStart, xEnd, param);
            Buttons[i].localPosition = new Vector3(x, y, 0);

            float effectiveScale = itemWidth * MainPanel.ButtonSummoness;
            Buttons[i].localScale = new Vector3(effectiveScale, effectiveScale, effectiveScale);
        }
    }

    private float GetItemWidth(float videoSpan)
    {
        float fullItemSpan = (fullScale + MinMargin) * Buttons.Length;
        if(fullItemSpan > videoSpan)
        {
            float availableSpace = videoSpan - MinMargin * Buttons.Length;
            float ret = availableSpace / Buttons.Length;
            ret = Mathf.Max(0, ret);
            return ret;
        }
        return fullScale;
    }
}
