using UnityEngine;

public interface IFocusableItem
{
    ItemFocusType FocusType { get; }
    bool IsFocusable { get; }
    FocusPriority Priority { get; }
    float ActivationDistance { get; }
    float GetDistanceToPointer(Vector3 pointerPos);
}
