//
// Copyright (C) Microsoft. All rights reserved.
//

using TMPro;
using UnityEngine;

public class IconSource : MonoBehaviour
{
    public static IconSource Instance { get; private set; }

    [SerializeField]
    private TMP_FontAsset customIconsFont;
    public TMP_FontAsset CustomIconsFont { get { return this.customIconsFont; } }

    [SerializeField]
    private TMP_FontAsset officialIconsFont;
    public TMP_FontAsset OfficialIconsFont { get { return this.officialIconsFont; } }

    private void Start()
    {
        Instance = this;
    }
}