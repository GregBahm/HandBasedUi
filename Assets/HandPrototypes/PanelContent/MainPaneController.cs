using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPaneController : SlateController
{
    public float ButtonSummoness
    {
        get
        {
            float remap = Mathf.Clamp01((Summonness - .5f) * 2);
            return Mathf.Pow(remap, 2);
        }
    }
    public Transform NamePlate;
    public VideoCardTopButtons TopButtons;
    public VideoCardBottomButtons ButtomButtons;

    private PushyButtonController[] buttons;
    public PushyButtonController[] Buttons
    {
        get
        {
            if(buttons == null)
            {
                buttons = GetButtons();
            }
            return this.buttons;
        }
    }
    
    [SerializeField]
    private float verticalButtonMargin;
    public float VerticalButtonMargin { get { return this.verticalButtonMargin; } }
    [SerializeField]
    [Range(0, .1f)]
    private float bottomButtonSpacing;
    public float BottomButtonSpacing { get { return this.bottomButtonSpacing; } }
    [SerializeField]
    private float nameplateMargin;
    [SerializeField]
    private GrabberPlacer grabberPlacer;

    private void Start()
    {
        RegisterCooldowns();
    }

    private PushyButtonController[] GetButtons()
    {
        List<PushyButtonController> ret = new List<PushyButtonController>();
        ret.AddRange(TopButtons.Buttons);
        ret.AddRange(ButtomButtons.Buttons);
        return ret.ToArray();
    }

    private void RegisterCooldowns()
    {
        foreach (PushyButtonController button in Buttons)
        {
            button.Released += Button_Released;
        }
    }

    private void Button_Released(object sender, EventArgs e)
    {
        //Repositioning.ResetInteractionCooldown();
    }

    private void Update()
    {
        Thumbnail.UpdateThumbnail();
        HandleSlateSizing();
        Repositioning.UpdateSlatePositioning();
        grabberPlacer.DoUpdate();
        PositionContent();
        PositionNamePlate();
        TopButtons.PlaceButtons();
        ButtomButtons.PlaceButtons();
    }

    private void PositionNamePlate()
    {
        float x = -SlateContent.localScale.x / 2 - this.nameplateMargin;
        float y = SlateContent.localScale.y / 2 + this.nameplateMargin;
        NamePlate.localPosition = new Vector3(x, y, NamePlate.localPosition.z);


        NamePlate.localScale = new Vector3(Summonness, Summonness, Summonness);
    }
}
