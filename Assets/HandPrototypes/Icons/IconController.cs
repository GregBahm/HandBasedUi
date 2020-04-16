//
// Copyright (C) Microsoft. All rights reserved.
//

using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(TMP_Text))]
public class IconController : MonoBehaviour
{
    // For a new icon:
    // 1. Add a new enum entry at the end of IconKey
    // 2. Add a new dictionary entry at the end of IconsMap with corresponding IconKey and IconCharacterMapping
    // Don't reorder the enum or the dictionary; the IconControllerEditor will alphabetize the list for users
    public enum IconKey
    {
        Add,
        Arrow_annotate,
        Arrow_next,
        Arrow_previous,
        At,
        Back,
        Backspace,
        Call,
        Calendar,
        Capture,
        Chat_cutout,
        Chat_dot,
        Chat_lines,
        Chat_nolines,
        Chats,
        Check,
        Chevron_down,
        Chevron_left,
        Chevron_right,
        Chevron_up,
        Close,
        Cloud_download,
        Cloud_upload,
        Colorpicker,
        Contacts,
        Copy,
        Copy_sdf,
        Delete,
        Dock,
        Dock_chat,
        Dynamics,
        Endcall,
        Erase,
        Error_badge,
        External_link,
        Files,
        Folder,
        Folder1,
        Help,
        Hololens,
        Image,
        Incident_triangle,
        Info,
        Leave_chat,
        Link,
        Locked,
        Mic,
        Mic_mute,
        Minimize,
        Move,
        Onedrive,
        Participantlist,
        Pen,
        Pen_palette,
        Pencil,
        Penworkspace,
        People_add,
        Person_add,
        Phone,
        Pin,
        Pin_sideways,
        Poor_wifi,
        Ruler,
        Scale,
        Search,
        Search_user,
        Security_alert,
        Send,
        Settings,
        Share,
        Status_onbreak,
        Status_committed,
        Status_error,
        Status_generic,
        Status_inprogress,
        Status_scheduled,
        Status_traveling,
        Switch_user,
        Three_dimensional_file,
        Undo,
        Undock,
        Unshift_left,
        Unshift_right,
        Uptotop,
        Video,
        Video_mute,
        Warning,
        Wifi_1,
        Wifi_2,
        Wifi_3,
        Wifi_4,
        FreezeMode,
        Record_InnerDot,
        Record_OuterRing,
        Stop,
        GoToVideo,
        DateTime,
        Refresh,
        Complete,
        StreamIcon,
    }

    public static readonly Dictionary<IconKey, IconCharacterMapping> IconsMap = new Dictionary<IconKey, IconCharacterMapping>
        {
            { IconKey.Add, new IconCharacterMapping("") },
            { IconKey.Arrow_annotate, new IconCharacterMapping("A", true) },
            { IconKey.Arrow_next, new IconCharacterMapping("") },
            { IconKey.Arrow_previous, new IconCharacterMapping("") },
            { IconKey.At, new IconCharacterMapping("") },
            { IconKey.Back, new IconCharacterMapping("J", true) },
            { IconKey.Backspace, new IconCharacterMapping("") },
            { IconKey.Call, new IconCharacterMapping("") },
            { IconKey.Calendar, new IconCharacterMapping("") },
            { IconKey.Capture, new IconCharacterMapping("") },
            { IconKey.Chat_cutout, new IconCharacterMapping("B", true) },
            { IconKey.Chat_dot, new IconCharacterMapping("") },
            { IconKey.Chat_lines, new IconCharacterMapping("") },
            { IconKey.Chat_nolines, new IconCharacterMapping("") },
            { IconKey.Chats, new IconCharacterMapping("") },
            { IconKey.Check, new IconCharacterMapping("") },
            { IconKey.Chevron_down, new IconCharacterMapping("") },
            { IconKey.Chevron_left, new IconCharacterMapping("") },
            { IconKey.Chevron_right, new IconCharacterMapping("") },
            { IconKey.Chevron_up, new IconCharacterMapping("") },
            { IconKey.Close, new IconCharacterMapping("") },
            { IconKey.Cloud_download, new IconCharacterMapping("") },
            { IconKey.Cloud_upload, new IconCharacterMapping("") },
            { IconKey.Colorpicker, new IconCharacterMapping("I", true) },
            { IconKey.Contacts, new IconCharacterMapping("") },
            { IconKey.Copy, new IconCharacterMapping("") },
            { IconKey.Copy_sdf, new IconCharacterMapping("") },
            { IconKey.Delete, new IconCharacterMapping("") },
            { IconKey.Dock, new IconCharacterMapping("C", true) },
            { IconKey.Dock_chat, new IconCharacterMapping("D", true) },
            { IconKey.Dynamics, new IconCharacterMapping("") },
            { IconKey.Endcall, new IconCharacterMapping("M", true) },
            { IconKey.Erase, new IconCharacterMapping("") },
            { IconKey.Error_badge, new IconCharacterMapping("") },
            { IconKey.External_link, new IconCharacterMapping("E", true) },
            { IconKey.Files, new IconCharacterMapping("") },
            { IconKey.Folder, new IconCharacterMapping("") },
            { IconKey.Folder1, new IconCharacterMapping("") },
            { IconKey.Help, new IconCharacterMapping("") },
            { IconKey.Hololens, new IconCharacterMapping("") },
            { IconKey.Image, new IconCharacterMapping("") },
            { IconKey.Incident_triangle, new IconCharacterMapping("") },
            { IconKey.Info, new IconCharacterMapping("") },
            { IconKey.Leave_chat, new IconCharacterMapping("") },
            { IconKey.Link, new IconCharacterMapping("") },
            { IconKey.Locked, new IconCharacterMapping("") },
            { IconKey.Mic, new IconCharacterMapping("") },
            { IconKey.Mic_mute, new IconCharacterMapping("F", true) },
            { IconKey.Minimize, new IconCharacterMapping("") },
            { IconKey.Move, new IconCharacterMapping("") },
            { IconKey.Onedrive, new IconCharacterMapping("H", true) },
            { IconKey.Participantlist, new IconCharacterMapping("") },
            { IconKey.Pen, new IconCharacterMapping("") },
            { IconKey.Pen_palette, new IconCharacterMapping("") },
            { IconKey.Pencil, new IconCharacterMapping("") },
            { IconKey.Penworkspace, new IconCharacterMapping("") },
            { IconKey.People_add, new IconCharacterMapping("") },
            { IconKey.Person_add, new IconCharacterMapping("") },
            { IconKey.Phone, new IconCharacterMapping("") },
            { IconKey.Pin, new IconCharacterMapping("") },
            { IconKey.Pin_sideways, new IconCharacterMapping("") },
            { IconKey.Poor_wifi, new IconCharacterMapping("") },
            { IconKey.Ruler, new IconCharacterMapping("") },
            { IconKey.Scale, new IconCharacterMapping("") },
            { IconKey.Search, new IconCharacterMapping("") },
            { IconKey.Search_user, new IconCharacterMapping("") },
            { IconKey.Security_alert, new IconCharacterMapping("") },
            { IconKey.Send, new IconCharacterMapping("") },
            { IconKey.Settings, new IconCharacterMapping("") },
            { IconKey.Share, new IconCharacterMapping("") },
            { IconKey.Status_onbreak, new IconCharacterMapping("") },
            { IconKey.Status_committed, new IconCharacterMapping("") },
            { IconKey.Status_error, new IconCharacterMapping("") },
            { IconKey.Status_generic, new IconCharacterMapping("") },
            { IconKey.Status_inprogress, new IconCharacterMapping("") },
            { IconKey.Status_scheduled, new IconCharacterMapping("") },
            { IconKey.Status_traveling, new IconCharacterMapping("") },
            { IconKey.Switch_user, new IconCharacterMapping("G", true) },
            { IconKey.Three_dimensional_file, new IconCharacterMapping("") },
            { IconKey.Undo, new IconCharacterMapping("") },
            { IconKey.Undock, new IconCharacterMapping("") },
            { IconKey.Unshift_left, new IconCharacterMapping("") },
            { IconKey.Unshift_right, new IconCharacterMapping("") },
            { IconKey.Uptotop, new IconCharacterMapping("") },
            { IconKey.Video, new IconCharacterMapping("K", true) },
            { IconKey.Video_mute, new IconCharacterMapping("L", true) },
            { IconKey.Warning, new IconCharacterMapping("") },
            { IconKey.Wifi_1, new IconCharacterMapping("") },
            { IconKey.Wifi_2, new IconCharacterMapping("") },
            { IconKey.Wifi_3, new IconCharacterMapping("") },
            { IconKey.Wifi_4, new IconCharacterMapping("") },
            { IconKey.FreezeMode, new IconCharacterMapping("") },
            { IconKey.Record_InnerDot, new IconCharacterMapping("") },
            { IconKey.Record_OuterRing, new IconCharacterMapping("") },
            { IconKey.Stop, new IconCharacterMapping("") },
            { IconKey.GoToVideo, new IconCharacterMapping("") },
            { IconKey.DateTime, new IconCharacterMapping("") },
            { IconKey.Refresh, new IconCharacterMapping("") },
            { IconKey.Complete, new IconCharacterMapping("") },
            { IconKey.StreamIcon, new IconCharacterMapping("") },
        };

    [SerializeField]
    private IconKey icon;
    public IconKey Icon
    {
        get
        {
            return icon;
        }

        set
        {
            if (icon != value)
            {
                icon = value;
                UpdateIconText();
            }
        }
    }

    private TMP_Text textMeshComponent;

    void Awake()
    {
        textMeshComponent = GetComponent<TMP_Text>();
    }

    public void RestoreBinding()
    {
        if (!string.IsNullOrWhiteSpace(textMeshComponent.text))
        {
            foreach (var item in IconsMap)
            {
                if (textMeshComponent.text == item.Value.IconCharacter)
                {
                    Icon = item.Key;
                    break;
                }
            }
        }
    }
#if (UNITY_EDITOR)
    public const string OfficialIconsAssetName = "OfficialIconsFont";
    public const string CustomIconsAssetName = "CustomIconsFont";

    public static TMP_FontAsset GetFontAsset(string name)
    {
        string assetGuid = AssetDatabase.FindAssets(name).FirstOrDefault();
        string assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
        TMP_FontAsset iconsFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(assetPath);

        if (iconsFont == null)
        {
            Debug.LogError($"IconSource - {name} asset is missing.");
        }

        return iconsFont;
    }
#endif

    private void UpdateIconText()
    {
        IconCharacterMapping mapping = IconsMap[Icon];
#if (UNITY_EDITOR)
        Undo.RecordObject(textMeshComponent, "Icon Change");
        string fontName = mapping.IsCustom ? CustomIconsAssetName : OfficialIconsAssetName;
        textMeshComponent.font = GetFontAsset(fontName);
        textMeshComponent.text = mapping.IconCharacter;
        PrefabUtility.RecordPrefabInstancePropertyModifications(textMeshComponent);
#else
            textMeshComponent.font = mapping.IsCustom ? IconSource.Instance.CustomIconsFont : IconSource.Instance.OfficialIconsFont;
            textMeshComponent.text = mapping.IconCharacter;
#endif
    }

    public class IconCharacterMapping
    {
        public bool IsCustom { get; }
        public string IconCharacter { get; }

        public IconCharacterMapping(string iconCharacter, bool isCustom = false)
        {
            IsCustom = isCustom;
            IconCharacter = iconCharacter;
        }
    }
}

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