using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PortalScript : MonoBehaviour 
{
    public int PortalId;
    void Update () 
    {
        Shader.SetGlobalVector("_PortalNormal", transform.forward);
        Shader.SetGlobalVector("_PortalPoint", transform.position);
	}
}
