using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EnvelopeHandle : XRBaseInteractable
{
    [SerializeField] private EnvelopeEditor ParentEditor;
    [SerializeField] private Transform UpperHandle;
    [SerializeField] private Transform LowerHandle;
    private Vector3 interactorPoint;
    private IXRInteractor interactor;

    public bool IsGrabbing { get; private set; }

    private void Update()
    {
        if(IsGrabbing)
        {
            var position = interactor.transform.TransformPoint(interactorPoint);
            position = transform.parent.InverseTransformPoint(position);
            var z = position.z;
            
            if (UpperHandle != null)
                z = Mathf.Min(UpperHandle.localPosition.z, z);
            else if (LowerHandle != null)
                z = Mathf.Max(LowerHandle.localPosition.z, z);

            var range = ParentEditor.GetHandleRange();
            z = Mathf.Clamp(z, -range, range);

            position = transform.localPosition;
            position.z = z;
            transform.localPosition = position;
        }
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        interactor = args.interactorObject;
        interactorPoint = interactor.transform.InverseTransformPoint(transform.position);
        IsGrabbing = true;
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        interactor = null;
        IsGrabbing = false;
    }
}
