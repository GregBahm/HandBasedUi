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
        controllers = new IBeatController[] { AssetMenu, SpatialInspection, PostWorkOrder, Recording};
        buttons = new MenuItemButton[] { AssetMenuButton, SpatialInspectionButton, PostWorkOrderButton, RecordingButton };

        AssetMenuButton.Released += AssetMenuButton_Released;
        SpatialInspectionButton.Released += SpatialInspectionButton_Released;
        PostWorkOrderButton.Released += PostWorkOrderButton_Released;
        RecordingButton.Released += RecordingButton_Released;

        NextButton.Released += NextButton_Released;
        PreviousButton.Released += PreviousButton_Released;
    }

    private void RecordingButton_Released(object sender, System.EventArgs e)
    {
        MasterBeat = 3;
    }

    private void PostWorkOrderButton_Released(object sender, System.EventArgs e)
    {
        MasterBeat = 2;
    }

    private void SpatialInspectionButton_Released(object sender, System.EventArgs e)
    {
        MasterBeat = 1;
    }

    private void AssetMenuButton_Released(object sender, System.EventArgs e)
    {
        MasterBeat = 0;
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
        PreviousButton.IsDisabled = CurrentController.Beat == 0;
        NextButton.IsDisabled = CurrentController.Beat == (CurrentController.MaxBeats);

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

    public void GoToAssetMenu()
    {
        MasterBeat = 0;
        NextSubBeat();
    }

    public void GotoSpatialInspection()
    {
        MasterBeat = 1;
        NextSubBeat();
    }

    public void GoToPostWorkOrder()
    {
        MasterBeat = 2;
        NextSubBeat();
    }

    public void GotoRecordingController()
    {
        MasterBeat = 3;
        NextSubBeat();
    }

    public void NextSubBeat()
    {
        int beat = CurrentController.Beat + 1;
        beat = Mathf.Min(beat, CurrentController.MaxBeats);
        CurrentController.Beat = beat;
    }

    public void PreviousSubBeat()
    {
        int beat = CurrentController.Beat - 1;
        beat = Mathf.Max(beat, 0);
        CurrentController.Beat = beat;
    }
}

public interface IBeatController
{
    int Beat { get; set; }
    int MaxBeats { get; }
}
