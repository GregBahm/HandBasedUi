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
    
    private float scrollProg;

    [SerializeField]
    private RectTransform scrollableContent;

    [SerializeField]
    private Transform scrollTop;
    [SerializeField]
    private Transform scrollBottom;
    [SerializeField]
    private Transform scrollPosition;
    [SerializeField]
    private Transform scrollGrabberPosition;

    [SerializeField]
    private GameObject scrollGrabberPrefab;

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
        UpdateScrollPosition();
        scrollProg = GetScrollProgress();
        ApplyScrolling();
        showChat = chatButton.Toggled;
    }

    private float GetScrollProgress()
    {
        Vector3 topToBottom = scrollTop.position - scrollBottom.position;
        Plane plane = new Plane(topToBottom, scrollBottom.position);
        float dist = plane.GetDistanceToPoint(scrollPosition.position);
        //dist = Mathf.Clamp(dist, 0, topToBottom.magnitude);
        return 1 - (dist / topToBottom.magnitude);
    }

    private void UpdateScrollPosition()
    {
        Vector3 topToBottom = scrollBottom.position - scrollTop.position;
        Vector3 topToGrabber = scrollGrabberPosition.position - scrollTop.position;
        scrollPosition.position = Vector3.Project(topToGrabber, topToBottom.normalized) + scrollTop.position;
    }

    private void ApplyScrolling()
    {
        float scrollTarget = Mathf.LerpUnclamped(bottomY, topY, scrollProg);
        scrollableContent.localPosition = new Vector3(scrollableContent.localPosition.x, scrollTarget, scrollableContent.localPosition.z);
    }

    private void UpdateMainOpenness()
    {
        float widthTarget = GetWidthTarget();
        currentWidth = Mathf.Lerp(currentWidth, widthTarget, Time.deltaTime * 10);
        chatContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentWidth);
    }
}
