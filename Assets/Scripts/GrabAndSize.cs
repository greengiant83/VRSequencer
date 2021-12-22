using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabAndSize : MonoBehaviour
{
    [SerializeField] private Sequencer Sizer;

    private Vector3 firstGrabPoint; //Local space to interactor
    private Vector3 secondGrabPoint;
    private Quaternion firstGrabRotation;
    private Quaternion secondGrabRotation;
    private Vector3 initialSize;
    private Vector3 scaleInversion = Vector3.one;

    public bool IsGrabbing { get; private set; }

    private void Update()
    {
        if (!IsGrabbing) return;

        if(Sizer.interactorsSelecting.Count == 1)
        {
            transform.position = Sizer.interactorsSelecting[0].transform.TransformPoint(firstGrabPoint);
            transform.rotation = Sizer.interactorsSelecting[0].transform.TransformRotation(firstGrabRotation);
        }
        else if(Sizer.interactorsSelecting.Count >= 2)
        {
            var firstWorldPoint = Sizer.interactorsSelecting[0].transform.TransformPoint(firstGrabPoint);
            var secondWorldPoint = Sizer.interactorsSelecting[1].transform.TransformPoint(secondGrabPoint);
            var firstWorldRotation = Sizer.interactorsSelecting[0].transform.TransformRotation(firstGrabRotation);
            var secondWorldRotation = Sizer.interactorsSelecting[1].transform.TransformRotation(secondGrabRotation);

            transform.rotation = Quaternion.Slerp(firstWorldRotation, secondWorldRotation, 0.5f);
            transform.position = Vector3.Lerp(firstWorldPoint, secondWorldPoint, 0.5f);

            var localFirst = Quaternion.Inverse(transform.rotation) * firstWorldPoint;
            var localSecond = Quaternion.Inverse(transform.rotation) * secondWorldPoint;
            var localDelta = localFirst - localSecond;

            localDelta.Scale(scaleInversion);
            Sizer.SetSize(localDelta + initialSize);
        }
    }

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (Sizer.IsSpawning) return;

        Debug.Log("Select enter: " + Sizer.interactorsSelecting.Count);

        if (Sizer.interactorsSelecting.Count == 1)
            grabStart();
        else if (Sizer.interactorsSelecting.Count == 2)
            stretchStart();
    }

    public void OnSelectExited(SelectExitEventArgs args)
    {
        if (Sizer.IsSpawning) return;

        if (Sizer.interactorsSelecting.Count == 1)
            grabStart();
        else if (Sizer.interactorsSelecting.Count == 0)
            grabEnd();
    }

    private void grabStart()
    {
        IsGrabbing = true;
        firstGrabPoint = Sizer.interactorsSelecting[0].transform.InverseTransformPoint(transform.position);
        firstGrabRotation = Sizer.interactorsSelecting[0].transform.InverseTransformRotation(transform.rotation);
    }

    private void grabEnd()
    {
        IsGrabbing = false;
    }

    private void stretchStart()
    {
        Debug.Log("Stretching start");
        secondGrabPoint = Sizer.interactorsSelecting[1].transform.InverseTransformPoint(transform.position);
        secondGrabRotation = Sizer.interactorsSelecting[1].transform.InverseTransformRotation(transform.rotation);
        initialSize = Sizer.GetSize();

        var firstInteractionPoint = getLocalInteractionPoint(Sizer.interactorsSelecting[0]);
        var secondInteractionPoint = getLocalInteractionPoint(Sizer.interactorsSelecting[1]);

        scaleInversion.x = firstInteractionPoint.x < secondInteractionPoint.x ? -1 : 1;
        scaleInversion.y = firstInteractionPoint.y < secondInteractionPoint.y ? -1 : 1;
    }

    private Vector3 getLocalInteractionPoint(IXRInteractor interactor)
    {
        Vector3 point;

        var rayInteractor = interactor as XRRayInteractor;
        if (rayInteractor != null && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hitInfo))
            point = hitInfo.point;
        else
            point = interactor.transform.position;

        return transform.InverseTransformPoint(point);
    }

    private void stretchEnd()
    {
        Debug.Log("stretch end");
    }
}
