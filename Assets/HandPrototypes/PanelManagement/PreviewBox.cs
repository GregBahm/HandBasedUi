using UnityEngine;

public class PreviewBox : MonoBehaviour
{
    private Transform originalParent;

    public GrabbableItemsManager Manager;

    public BoxCollider Box { get; private set; }

    private void Start()
    {
        originalParent = transform.parent;
        Box = GetComponent<BoxCollider>();
    }

    public float GetDistanceToGrab()
    {
        Vector3 grabPoint = MainPinchDetector.Instance.PinchPoint.position;
        Vector3 closestPoint = Box.ClosestPoint(grabPoint);
        return (grabPoint - closestPoint).magnitude;
    }

    public void StartGrab()
    {
        transform.parent = Manager.SmoothedGrabPoint;

        Transform grabbedTransform = Manager.GrabbedItem.transform;
        transform.localScale = new Vector3(grabbedTransform.lossyScale.x + Manager.PreviewBoxPadding,
            grabbedTransform.lossyScale.y + Manager.PreviewBoxPadding,
            grabbedTransform.lossyScale.z + Manager.PreviewBoxPadding);
        transform.position = grabbedTransform.position;
        transform.rotation = grabbedTransform.rotation;
    }

    public void StartThumbnailGrab()
    {
        transform.parent = Manager.SmoothedGrabPoint;

        Transform grabbedTransform = Manager.GrabbedItem.transform;
        Transform thumbTransform = Manager.GrabbedItem.ThumbnailContent.transform;
        transform.localScale = new Vector3(grabbedTransform.lossyScale.x + Manager.PreviewBoxPadding,
            grabbedTransform.lossyScale.y + Manager.PreviewBoxPadding,
            grabbedTransform.lossyScale.z + Manager.PreviewBoxPadding);
        transform.position = thumbTransform.position;
        transform.rotation = thumbTransform.rotation;
    }

    private void StartGrab(Transform grabbedTransform)
    {
    }

    public void EndGrab()
    {
        transform.parent = originalParent;
    }
}