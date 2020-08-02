using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThumbnailGrabberBehavior : MonoBehaviour
{
    [SerializeField]
    private SlateRepositioning slateRepositioning;
    
    [SerializeField]
    private ScreenspaceFocusable grabberFocus;

    [SerializeField]
    private GrabberVisualController grabberVisual;

    [SerializeField]
    private GameObject content;

    [SerializeField]
    private Image Image;

    private float currentTransition;

    public float Transition { get { return currentTransition / HandMountedUiController.Instance.SummonTransitionTime; } }

    public bool IsFocused { get { return FocusManager.Instance.FocusedItem == grabberFocus; } }

    private bool grabbed;
    private Vector3 originalGrabberScale;

    private void Start()
    {
        originalGrabberScale = grabberVisual.transform.localScale;
    }

    public void DoUpdate(bool summoned)
    {
        UpdateTransition(summoned);
        if(IsFocused && MainPinchDetector.Instance.PinchBeginning)
        {
            StartSummon();
        }
        if(grabbed && !MainPinchDetector.Instance.Pinching)
        {
            grabbed = false;
            grabberVisual.gameObject.SetActive(true);
        }
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        content.SetActive(Transition > float.Epsilon);
        Color fade = new Color(1, 1, 1, Transition);
        Image.color = fade;

        grabberVisual.transform.localScale = originalGrabberScale * Transition;
    }

    private void UpdateTransition(bool summoned)
    {
        if(grabbed || !summoned)
        {
            currentTransition -= Time.deltaTime;
        }
        else
        {
            currentTransition += Time.deltaTime;
        }
        currentTransition = Mathf.Clamp(currentTransition, 0, HandMountedUiController.Instance.SummonTransitionTime);
    }

    public void DoReset()
    {
        currentTransition = 0;
    }

    private void StartSummon()
    {
        grabbed = true;
        grabberVisual.gameObject.SetActive(false);
        slateRepositioning.gameObject.SetActive(true);
        currentTransition = HandMountedUiController.Instance.SummonTransitionTime;
        slateRepositioning.StartSummon(grabberVisual);
    }
}
