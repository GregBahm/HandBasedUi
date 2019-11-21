using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoCardBottomButtons : MonoBehaviour
{
    public MainPanelArrangement MainPanel;

    public Transform[] Buttons;

    public float VerticalOffset;

    private void Update()
    {
        transform.localPosition = Vector3.zero;
        float y = -MainPanel.VideoCard.localScale.y / 2 - MainPanel.VerticalButtonMargin;
        float fullWidth = (Buttons.Length - 1) * MainPanel.BottomButtonSpacing;
        for (int i = 0; i < Buttons.Length; i++)
        {
            float x = i * MainPanel.BottomButtonSpacing - fullWidth / 2;
            Buttons[i].localPosition = new Vector3(x, y, 0);
        }
    }
}
