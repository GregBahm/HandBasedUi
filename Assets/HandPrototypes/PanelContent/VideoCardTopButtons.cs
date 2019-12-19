using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoCardTopButtons : MonoBehaviour
{
    private float fullScale;
    public MainPanelArrangement MainPanel;

    public Transform[] Buttons;

    private void Start()
    {
        fullScale = Buttons[0].localScale.x;
    }

    public void PlaceButtons()
    {
        transform.localPosition = Vector3.zero;

        float y = MainPanel.VideoCard.localScale.y / 2 + MainPanel.VerticalButtonMargin;
        float itemWidth = Buttons[0].localScale.x;

        float videoSpan = MainPanel.VideoCard.localScale.x;
        float xStart = -videoSpan / 2 + itemWidth / 2;
        float xEnd = videoSpan / 2 - itemWidth / 2;

        for (int i = 0; i < Buttons.Length; i++)
        {
            float param = (float)i / (Buttons.Length - 1);
            float x = Mathf.Lerp(xStart, xEnd, param);
            Buttons[i].localPosition = new Vector3(x, y, 0);

            float effectiveScale = fullScale * MainPanel.ButtonSummoness;
            Buttons[i].localScale = new Vector3(effectiveScale, effectiveScale, effectiveScale);
        }
    }
}
