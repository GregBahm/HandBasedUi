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
    private UguiButtonController chatButton;

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
    private GrabberVisualController scrollGrabber;

    [SerializeField]
    private float overScrollDampening = 0.2f;

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
        showChat = chatButton.Toggled;
        UpdateMainOpenness();
        UpdateGrabbedness();
        if(scrollGrabber.IsGrabbed)
        {
            UpdateGrabbedScrollPosition();
            scrollProg = GetScrollProgress();
        }
        else
        {
            UpdatePassiveScrollPosition();
        }
        ApplyScrolling();
    }

    private void UpdateGrabbedness()
    {
        if(FocusManager.Instance.FocusedItem == scrollGrabber.Focus
            && MainPinchDetector.Instance.PinchBeginning)
        {
            scrollGrabber.Focus.ForceFocus = true;
        }
        if(!MainPinchDetector.Instance.Pinching)
        {
            scrollGrabber.Focus.ForceFocus = false;
        }
    }

    private void UpdatePassiveScrollPosition()
    {
        float scrollTarget = Mathf.Clamp01(scrollProg);
        scrollProg = Mathf.Lerp(scrollProg, scrollTarget, Time.deltaTime * 4);
        scrollPosition.position = Vector3.Lerp(scrollTop.position, scrollBottom.position, scrollProg);
        scrollGrabberPosition.position = Vector3.Lerp(scrollGrabberPosition.position, scrollPosition.position, Time.deltaTime * 4);
    }

    private float GetScrollProgress()
    {
        Vector3 topToBottom = scrollTop.position - scrollBottom.position;
        Plane plane = new Plane(topToBottom, scrollBottom.position);
        float dist = plane.GetDistanceToPoint(scrollPosition.position);
        float ret = 1 - (dist / topToBottom.magnitude);
        return DampenOverscrolling(ret);
    }

    private float DampenOverscrolling(float ret)
    {
        if (ret > 1)
        {
            return Mathf.Pow(ret, overScrollDampening);
        }
        if (ret < 0)
        {
            float valToSoften = 1 - ret;
            valToSoften = Mathf.Pow(valToSoften, overScrollDampening);
            return -(valToSoften - 1);
        }
        return ret;
    }

    private void UpdateGrabbedScrollPosition()
    {
        scrollGrabberPosition.position = MainPinchDetector.Instance.PinchPoint.position;
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
