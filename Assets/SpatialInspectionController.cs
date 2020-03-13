using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpatialInspectionController : MonoBehaviour, IBeatController
{
    [Range(0, 3)]
    [SerializeField]
    private int beat;
    public int Beat { get => beat; set => beat = value; }
    public int MaxBeats => 3;

    public GameObject Slate;
    public GameObject BeatA;
    public GameObject BeatB;
    public GameObject BeatC;


    void Update()
    {
        Slate.SetActive(Beat > 0);
        BeatA.SetActive(Beat == 1);
        BeatB.SetActive(Beat == 2);
        BeatC.SetActive(Beat == 3);
    }
}
