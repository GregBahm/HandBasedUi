//
// Copyright (C) Microsoft. All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace RemoteAssist.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(TMP_Text))]
    public class IconController : MonoBehaviour
    {
        // Edit this and the IconsMap whenever a new icon is added
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
            Follow,
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
        }

        private static readonly Dictionary<IconKey, IconCharacterMapping> IconsMap;

        static IconController()
        {
            IconsMap = new Dictionary<IconKey, IconCharacterMapping>
            {
                { IconKey.Add, new IconCharacterMapping("")},
                { IconKey.Arrow_annotate, new IconCharacterMapping("A", true)},
                { IconKey.Arrow_next, new IconCharacterMapping("")},
                { IconKey.Arrow_previous, new IconCharacterMapping("")},
                { IconKey.At, new IconCharacterMapping("")},
                { IconKey.Back, new IconCharacterMapping("J", true)},
                { IconKey.Backspace, new IconCharacterMapping("")},
                { IconKey.Call, new IconCharacterMapping("")},
                { IconKey.Calendar, new IconCharacterMapping("")},
                { IconKey.Capture, new IconCharacterMapping("")},
                { IconKey.Chat_cutout, new IconCharacterMapping("B", true)},
                { IconKey.Chat_dot, new IconCharacterMapping("")},
                { IconKey.Chat_lines, new IconCharacterMapping("")},
                { IconKey.Chat_nolines, new IconCharacterMapping("")},
                { IconKey.Chats, new IconCharacterMapping("")},
                { IconKey.Check, new IconCharacterMapping("")},
                { IconKey.Chevron_down, new IconCharacterMapping("")},
                { IconKey.Chevron_left, new IconCharacterMapping("")},
                { IconKey.Chevron_right, new IconCharacterMapping("")},
                { IconKey.Chevron_up, new IconCharacterMapping("")},
                { IconKey.Close, new IconCharacterMapping("")},
                { IconKey.Cloud_download, new IconCharacterMapping("")},
                { IconKey.Cloud_upload, new IconCharacterMapping("")},
                { IconKey.Colorpicker, new IconCharacterMapping("I", true)},
                { IconKey.Contacts, new IconCharacterMapping("")},
                { IconKey.Copy, new IconCharacterMapping("")},
                { IconKey.Copy_sdf, new IconCharacterMapping("")},
                { IconKey.Delete, new IconCharacterMapping("")},
                { IconKey.Dock, new IconCharacterMapping("C", true)},
                { IconKey.Dock_chat, new IconCharacterMapping("D", true)},
                { IconKey.Dynamics, new IconCharacterMapping("")},
                { IconKey.Endcall, new IconCharacterMapping("M", true)},
                { IconKey.Erase, new IconCharacterMapping("")},
                { IconKey.Error_badge, new IconCharacterMapping("")},
                { IconKey.External_link, new IconCharacterMapping("E", true)},
                { IconKey.Files, new IconCharacterMapping("")},
                { IconKey.Folder, new IconCharacterMapping("")},
                { IconKey.Folder1, new IconCharacterMapping("")},
                { IconKey.Follow, new IconCharacterMapping("") },
                { IconKey.Help, new IconCharacterMapping("")},
                { IconKey.Hololens, new IconCharacterMapping("")},
                { IconKey.Image, new IconCharacterMapping("")},
                { IconKey.Incident_triangle, new IconCharacterMapping("")},
                { IconKey.Info, new IconCharacterMapping("")},
                { IconKey.Leave_chat, new IconCharacterMapping("")},
                { IconKey.Link, new IconCharacterMapping("")},
                { IconKey.Locked, new IconCharacterMapping("")},
                { IconKey.Mic, new IconCharacterMapping("")},
                { IconKey.Mic_mute, new IconCharacterMapping("F", true)},
                { IconKey.Minimize, new IconCharacterMapping("")},
                { IconKey.Move, new IconCharacterMapping("")},
                { IconKey.Onedrive, new IconCharacterMapping("H", true)},
                { IconKey.Participantlist, new IconCharacterMapping("")},
                { IconKey.Pen, new IconCharacterMapping("")},
                { IconKey.Pen_palette, new IconCharacterMapping("")},
                { IconKey.Pencil, new IconCharacterMapping("")},
                { IconKey.Penworkspace, new IconCharacterMapping("")},
                { IconKey.People_add, new IconCharacterMapping("")},
                { IconKey.Person_add, new IconCharacterMapping("")},
                { IconKey.Phone, new IconCharacterMapping("")},
                { IconKey.Pin, new IconCharacterMapping("")},
                { IconKey.Pin_sideways, new IconCharacterMapping("")},
                { IconKey.Poor_wifi, new IconCharacterMapping("")},
                { IconKey.Ruler, new IconCharacterMapping("")},
                { IconKey.Scale, new IconCharacterMapping("")},
                { IconKey.Search, new IconCharacterMapping("")},
                { IconKey.Search_user, new IconCharacterMapping("")},
                { IconKey.Security_alert, new IconCharacterMapping("")},
                { IconKey.Send, new IconCharacterMapping("")},
                { IconKey.Settings, new IconCharacterMapping("")},
                { IconKey.Share, new IconCharacterMapping("")},
                { IconKey.Status_onbreak, new IconCharacterMapping("")},
                { IconKey.Status_committed, new IconCharacterMapping("")},
                { IconKey.Status_error, new IconCharacterMapping("")},
                { IconKey.Status_generic, new IconCharacterMapping("")},
                { IconKey.Status_inprogress, new IconCharacterMapping("")},
                { IconKey.Status_scheduled, new IconCharacterMapping("")},
                { IconKey.Status_traveling, new IconCharacterMapping("")},
                { IconKey.Switch_user, new IconCharacterMapping("G", true)},
                { IconKey.Three_dimensional_file, new IconCharacterMapping("")},
                { IconKey.Undo, new IconCharacterMapping("")},
                { IconKey.Undock, new IconCharacterMapping("")},
                { IconKey.Unshift_left, new IconCharacterMapping("")},
                { IconKey.Unshift_right, new IconCharacterMapping("")},
                { IconKey.Uptotop, new IconCharacterMapping("")},
                { IconKey.Video, new IconCharacterMapping("K", true)},
                { IconKey.Video_mute, new IconCharacterMapping("L", true)},
                { IconKey.Warning, new IconCharacterMapping("")},
                { IconKey.Wifi_1, new IconCharacterMapping("")},
                { IconKey.Wifi_2, new IconCharacterMapping("")},
                { IconKey.Wifi_3, new IconCharacterMapping("")},
                { IconKey.Wifi_4, new IconCharacterMapping("")},
            };
        }

        private static Dictionary<IconKey, IconCharacterMapping> CreateIconMap()
        {
            Type iconKeyType = typeof(IconKey);
            Dictionary<IconKey, IconCharacterMapping> ret = new Dictionary<IconKey, IconCharacterMapping>();
            foreach (IconKey iconKey in Enum.GetValues(iconKeyType))
            {
                string iconName = Enum.GetName(iconKeyType, iconKey);
                FieldInfo field = iconKeyType.GetField(iconName);
                try
                {
                    IconCharacterMapping mapping = (IconCharacterMapping)Attribute.GetCustomAttribute(field, typeof(IconCharacterMapping));
                    ret.Add(iconKey, mapping);
                }
                catch (TypeLoadException)
                {
                    Debug.LogError("No IconCharacterMapping for " + iconName + ". Add an IconCharacterMapping attribute to the enum field in IconController.cs.");
                }
            }
            return ret;
        }

        [SerializeField]
        private IconKey icon;
        private IconKey lastIcon;

        [SerializeField]
        private TMP_FontAsset officialIconsFont = null;
        [SerializeField]
        private TMP_FontAsset customIconsFont = null;

        private TMP_Text textMeshComponent;

        void Start()
        {
            textMeshComponent = GetComponent<TMP_Text>();
            RestoreBinding();
        }

        private void RestoreBinding()
        {
            if (!string.IsNullOrWhiteSpace(textMeshComponent.text))
            {
                foreach (var item in IconsMap)
                {
                    if (textMeshComponent.text == item.Value.IconCharacter)
                    {
                        icon = item.Key;
                        lastIcon = icon;
                    }
                }
            }
        }

        void Update()
        {
            if (icon != lastIcon)
            {
                lastIcon = icon;
                UpdateIconText();
            }
        }

        private void UpdateIconText()
        {
#if(UNITY_EDITOR)
            Undo.RecordObject(textMeshComponent, "Icon Change");
#endif
            IconCharacterMapping mapping = IconsMap[icon];
            textMeshComponent.font = mapping.IsCustom ? GetCustomIconFont() : GetOfficialIconFont();
            textMeshComponent.text = mapping.IconCharacter;

#if (UNITY_EDITOR)
            PrefabUtility.RecordPrefabInstancePropertyModifications(textMeshComponent);
#endif
        }

        private TMP_FontAsset GetCustomIconFont()
        {
            if (customIconsFont == null)
            {
                Debug.Log("Can't set icon for IconController because CustomIconsFont is not set.");
                return null;
            }
            return customIconsFont;
        }

        private TMP_FontAsset GetOfficialIconFont()
        {
            if (officialIconsFont == null)
            {
                Debug.Log("Can't set icon for IconController because OfficialIconsFont is not set.");
                return null;
            }
            return officialIconsFont;
        }

        private class IconCharacterMapping : System.Attribute
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
}