using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MasterCoordinator : MonoBehaviour
{
    public AssetMenuController AssetMenu;
    public SpatialInspectionController SpatialInspection;
    public PostWorkOrderController PostWorkOrder;
    public RecordingController Recording;

    public SummonDetector Summoning;
    public MenuItemButton AssetMenuButton;
    public MenuItemButton SpatialInspectionButton;
    public MenuItemButton PostWorkOrderButton;
    public MenuItemButton RecordingButton;

    public MenuItemButton NextButton;
    public MenuItemButton PreviousButton;

    public GameObject SelectorRoot;

    public TMP_Text BeatLabel;
    
    [Range(0, 3)]
    public int MasterBeat;

    private IBeatController[] controllers;
    private MenuItemButton[] buttons;

    public IBeatController CurrentController { get { return controllers[MasterBeat]; } }

    void Start()
    {
        controllers = new IBeatController[] { AssetMenu, Recording, SpatialInspection, PostWorkOrder};
        buttons = new MenuItemButton[] { AssetMenuButton, SpatialInspectionButton, PostWorkOrderButton, RecordingButton };

        AssetMenuButton.Released += AssetMenuButton_Released;
        SpatialInspectionButton.Released += SpatialInspectionButton_Released;
        PostWorkOrderButton.Released += PostWorkOrderButton_Released;
        RecordingButton.Released += RecordingButton_Released;

        NextButton.Released += NextButton_Released;
        PreviousButton.Released += PreviousButton_Released;
    }

    private void AssetMenuButton_Released(object sender, System.EventArgs e)
    {
        MasterBeat = 0;
        CurrentController.Beat = 1;
    }
    private void RecordingButton_Released(object sender, System.EventArgs e)
    {
        MasterBeat = 1;
        CurrentController.Beat = 1;
    }

    private void SpatialInspectionButton_Released(object sender, System.EventArgs e)
    {
        MasterBeat = 2;
        CurrentController.Beat = 1;
    }

    private void PostWorkOrderButton_Released(object sender, System.EventArgs e)
    {
        MasterBeat = 3;
        CurrentController.Beat = 1;
    }


    private void PreviousButton_Released(object sender, System.EventArgs e)
    {
        PreviousSubBeat();
    }

    private void NextButton_Released(object sender, System.EventArgs e)
    {
        NextSubBeat();
    }

    void Update()
    {
        SelectorRoot.SetActive(Summoning.IsSummoned);
        SetBeatVisibility();
        BeatLabel.text = CurrentController.Beat + " / " + CurrentController.MaxBeats;
    }

    private void SetBeatVisibility()
    {

        for (int i = 0; i < controllers.Length; i++)
        {
            if (MasterBeat != i)
            {
                controllers[i].Beat = 0;
                buttons[i].Toggled = false;
            }
        }
    }

    public void NextSubBeat()
    {
        int beat = CurrentController.Beat + 1;
        if (beat > CurrentController.MaxBeats)
        {
            if ( MasterBeat < 3)
            {
                MasterBeat++;
                CurrentController.Beat = 1;
            }
        }
        else
        {
            CurrentController.Beat = beat;
        }
    }

    public void PreviousSubBeat()
    {
        int beat = CurrentController.Beat - 1;
        if (beat < 0)
        {
            if (MasterBeat > 0)
            {
                MasterBeat--;
                CurrentController.Beat = CurrentController.MaxBeats;
            }
        }
        else
        {
            CurrentController.Beat = beat;
        }
    }
}

public interface IBeatController
{
    int Beat { get; set; }
    int MaxBeats { get; }
}
