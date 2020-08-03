using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UguiFocusable : FocusableItemBehavior
{


    void Start()
    {
        FocusManager.Instance.RegisterFocusable(this);
    }

    void Update()
    {

    }
 
    public override float GetDistanceToPointer(Vector3 pointerPos)
    {
        //TODO: This
        return 0;
    }

}
