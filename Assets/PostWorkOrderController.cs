using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostWorkOrderController : MonoBehaviour, IBeatController
{
    [Range(0, 4)]
    [SerializeField]
    private int beat;
    public int Beat { get => beat; set => beat = value; }
    public int MaxBeats => 4;

    public float Beat2IntroDuration;
    private float beat2Intro;

    public float Beat3Duration;
    private float beat3Time;

    public GameObject PostWorkOrder1;
    public GameObject PostWorkOrder2;
    public GameObject PostWorkOrder3;
    public GameObject PostWorkOrder4;

    private SpriteRenderer callSpriteRenderer;
    private SpriteRenderer firstPanelRenderer;


    private void Start()
    {
        callSpriteRenderer = PostWorkOrder1.GetComponent<SpriteRenderer>();
        firstPanelRenderer = PostWorkOrder2.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        PostWorkOrder1.SetActive(Beat == 1 || Beat == 2);
        PostWorkOrder2.SetActive(Beat == 2 && beat2Intro < 0);
        PostWorkOrder3.SetActive(Beat == 3);
        PostWorkOrder4.SetActive(Beat == 4);

        if(Beat == 2)
        {
            beat2Intro -= Time.deltaTime;
            float alpha = beat2Intro / (Beat2IntroDuration / 2);
            callSpriteRenderer.color = new Color(1, 1, 1, alpha);

            float otherAlpha = (beat2Intro + Beat2IntroDuration) / Beat2IntroDuration;
            otherAlpha = 1 - Mathf.Clamp01(otherAlpha);
            firstPanelRenderer.color = new Color(1, 1, 1, otherAlpha);
        }
        else
        {
            callSpriteRenderer.color = Color.white;
            firstPanelRenderer.color = Color.white;
            beat2Intro = Beat2IntroDuration;
        }

        if(Beat == 3)
        {
            beat3Time -= Time.deltaTime;
            if(beat3Time < 0)
            {
                Beat++;
            }
        }
        else
        {
            beat3Time = Beat3Duration;
        }
    }
}
