using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoCardBottomButtons : MonoBehaviour
{
    private float fullScale;
    public MainPanelArrangement MainPanel;

    public SphereButton[] Buttons;

    public float VerticalOffset;

    private void Start()
    {
        fullScale = Buttons[0].transform.localScale.x;
    }

    public void PlaceButtons()
    {
        transform.localPosition = Vector3.zero;
        float videoSpan = MainPanel.VideoCard.localScale.x;
        float y = -MainPanel.VideoCard.localScale.y / 2 - MainPanel.VerticalButtonMargin;
        float fullWidth = (Buttons.Length) * MainPanel.BottomButtonSpacing;
        float shrinking = GetShrinkingFactor(fullWidth, videoSpan);

        float centering = (Buttons.Length - 1) * MainPanel.BottomButtonSpacing * shrinking * .5f;

        for (int i = 0; i < Buttons.Length; i++)
        {
            float x = i * MainPanel.BottomButtonSpacing * shrinking - centering;
            Buttons[i].transform.localPosition = new Vector3(x, y + VerticalOffset, 0);

            float effectiveScale = fullScale * shrinking * MainPanel.ButtonSummoness;
            Buttons[i].transform.localScale = new Vector3(effectiveScale, effectiveScale, effectiveScale);
        }
    }

    private float GetShrinkingFactor(float fullWidth, float videoSpan)
    {
        if (fullWidth > videoSpan)
        {
            return videoSpan / fullWidth;
        }
        return 1f;
    }
}
