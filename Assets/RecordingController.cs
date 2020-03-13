using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class RecordingController : MonoBehaviour, IBeatController
{
    [Range(0, 5)]
    [SerializeField]
    private int beat;
    public int Beat { get => beat; set => beat = value; }
    public int MaxBeats => 5;

    public GameObject MainUi;
    public GameObject HeadMountedContent;
    public GameObject CaptureFrameText;
    public GameObject CaptureFrameOutline;
    public GameObject CaptureFrameCountdown;
    public GameObject RecordingIndicator;
    public GameObject HVACPhoto;

    private float countdownTime;
    private TMP_Text countdownText;

    private void Start()
    {
        countdownText = CaptureFrameCountdown.GetComponent<TMP_Text>();
    }

    private void Update()
    {
        MainUi.SetActive(Beat > 0);
        HeadMountedContent.SetActive(Beat > 1);
        CaptureFrameText.SetActive(Beat == 2 || Beat == 5);
        CaptureFrameOutline.SetActive(Beat == 2 || Beat==3 || Beat == 5);
        CaptureFrameCountdown.SetActive(Beat == 3);
        RecordingIndicator.SetActive(Beat == 4);
        HVACPhoto.SetActive(Beat == 5);

        if(Beat == 3)
        {
            countdownTime -= Time.deltaTime;
            countdownText.text = Mathf.CeilToInt(countdownTime).ToString();
            if(countdownTime < 0)
            {
                Beat = 4;
            }
        }
        else
        {
            countdownTime = 3f;
        }
    }
}
