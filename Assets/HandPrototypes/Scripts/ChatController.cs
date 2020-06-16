using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatController : MonoBehaviour
{
    [SerializeField]
    private bool showChat;

    [SerializeField]
    private RectTransform chatContainer;

    [SerializeField]
    private PushyButtonController chatButton;

    [SerializeField]
    private float bottomY;
    [SerializeField]
    private float topY;

    [SerializeField]
    [Range(0, 1)]
    private float scrollProg;

    [SerializeField]
    private RectTransform scrollableContent;
    
    private float openWidth;
    private float currentWidth;

    private void Start()
    {
        openWidth = chatContainer.sizeDelta.x;
        currentWidth = GetWidthTarget();
    }

    private float GetWidthTarget()
    {
        return showChat? openWidth : 0;
    }

    private void Update()
    {
        UpdateMainOpenness();
        UpdateScrolling();
        showChat = chatButton.Toggled;
    }

    private void UpdateScrolling()
    {
        float scrollTarget = Mathf.Lerp(bottomY, topY, scrollProg);
        scrollableContent.localPosition = new Vector3(scrollableContent.localPosition.x, scrollTarget, scrollableContent.localPosition.z);
    }

    private void UpdateMainOpenness()
    {
        float widthTarget = GetWidthTarget();
        currentWidth = Mathf.Lerp(currentWidth, widthTarget, Time.deltaTime * 10);
        chatContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentWidth);
    }
}
