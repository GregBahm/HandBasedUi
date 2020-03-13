using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterCoordinator : MonoBehaviour
{
    public AssetMenuController AssetMenu;
    public SpatialInspectionController SpatialInspection;
    public PostWorkOrderController PostWorkOrder;
    public RecordingController Recording;

    [Range(0, 3)]
    public int MasterBeat;

    private IBeatController[] controllers;

    public IBeatController CurrentController { get { return controllers[MasterBeat]; } }

    void Start()
    {
        controllers = new IBeatController[] { AssetMenu, SpatialInspection, PostWorkOrder, Recording};
    }

    void Update()
    {
        for (int i = 0; i < controllers.Length; i++)
        {
            if(MasterBeat != i)
            {
                controllers[i].Beat = 0;
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
        beat = Mathf.Min(beat, 0);
        CurrentController.Beat = beat;
    }
}

public interface IBeatController
{
    int Beat { get; set; }
    int MaxBeats { get; }
}
