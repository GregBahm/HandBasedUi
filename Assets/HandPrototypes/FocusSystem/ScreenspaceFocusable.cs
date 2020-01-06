using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenspaceFocusable : FocusableItemBehavior
{
    private void Start()
    {
        FocusManager.Instance.RegisterFocusable(this);
    }

    public override float GetDistanceToPointer(Vector3 pointerPos)
    {
        Vector3 selfInScreenspace = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 pointerInScreenspace = Camera.main.WorldToScreenPoint(pointerPos);
        return (selfInScreenspace - pointerInScreenspace).magnitude;
    }
}
