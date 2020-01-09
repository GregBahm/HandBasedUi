using System;
using UnityEngine;

public class MediaSlateController : SlateController
{
    [SerializeField]
    private Transform[] topButtons;
    [SerializeField]
    private float horizontalButtonMargin;
    [SerializeField]
    private float verticalButtonMargin;

    private void Update()
    {
        PlaceButtons();
        Thumbnail.UpdateThumbnail();
        HandleSlateSizing();

        Repositioning.UpdateSlatePositioning();
        PositionContent();
    }

    private void PlaceButtons()
    {
        float xStart = SlateContent.localScale.x / 2;
        float y = SlateContent.localScale.y / 2 + verticalButtonMargin;
        float buttonWidth = topButtons[0].localScale.x;

        float currentX = xStart - buttonWidth / 2;
        for (int i = 0; i < topButtons.Length; i++)
        {
            topButtons[i].localPosition = new Vector3(currentX, y, 0);
            currentX -= buttonWidth;
            currentX -= horizontalButtonMargin;
        }
    }
}