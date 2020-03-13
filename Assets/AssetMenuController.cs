using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AssetMenuController : MonoBehaviour, IBeatController
{
    [Range(0, 5)]
    [SerializeField]
    private int beat;
    public int Beat { get => beat; set => beat = value; }
    public int MaxBeats => 5;

    public GameObject AssetMenuMain;
    public GameObject AssetMenu1;
    public GameObject AssetMenu3;
    public GameObject AssetMenu4;
    public GameObject AssetMenu5;
    public GameObject AssetPDF;


    void Update()
    {
        AssetMenuMain.SetActive(Beat > 0);
        AssetMenu1.SetActive(Beat == 1 || Beat == 2);
        AssetMenu3.SetActive(Beat == 3);
        AssetMenu4.SetActive(Beat == 4);
        AssetMenu5.SetActive(Beat == 5);
        AssetPDF.SetActive(Beat > 1);

    }
}