using System;
using UnityEngine;

public class MediaSlateController : SlateController
{
    private void Update()
    {
        Thumbnail.UpdateThumbnail();
        HandleSlateSizing();

        Repositioning.UpdateSlatePositioning();
        PositionContent();
    }
}