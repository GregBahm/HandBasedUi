using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereButton : MonoBehaviour
{
    public Transform Disk;
    private Material sphereMat;
    public MeshRenderer Sphere;

    public Transform Finger;

    public float MaxDist;

    private void Start()
    {
        sphereMat = Sphere.material;
    }

    private void Update()
    {
        float fingerParam = GetFingerParam();

        float diskZ = Mathf.Lerp(-.5f, .5f, fingerParam);


        float diskScale = GetDiskScale(fingerParam);
        
        Disk.localPosition = new Vector3(0, 0, diskZ);
        Disk.localScale = new Vector3(diskScale, diskScale, diskScale);


        transform.LookAt(Finger);
        transform.Rotate(0, 180, 0, Space.Self);

        sphereMat.SetFloat("_FingerParam", fingerParam);
    }

    private float GetFingerParam()
    {
        Vector3 toFinger = transform.position - Finger.position;
        float minDist = transform.localScale.x / 2;
        float distLength = MaxDist - minDist;

        float someOtherNumber = toFinger.magnitude - minDist;
        someOtherNumber = Mathf.Max(0, someOtherNumber);
        float ret = someOtherNumber / distLength;
        return Mathf.Clamp01(ret);
    }

    private float GetDiskScale(float fingerParam)
    {
        float x = Mathf.Lerp(-1f, 1f, fingerParam);
        return Mathf.Sqrt(1 - Mathf.Pow(x, 2)); 
    }
}
