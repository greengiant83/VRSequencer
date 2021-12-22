using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EnvelopeHandle : XRBaseInteractable
{
    [SerializeField] private Transform UpperHandle;
    [SerializeField] private Transform LowerHandle;
    private Vector3 interactorPoint;
    private bool isGrabbing;
    private IXRInteractor interactor;

    private void Update()
    {
        if(isGrabbing)
        {
            var position = interactor.transform.TransformPoint(interactorPoint);
            position = transform.parent.InverseTransformPoint(position);
            var y = position.y;
            
            if (UpperHandle != null)
                y = Mathf.Min(UpperHandle.localPosition.y, y);
            else if (LowerHandle != null)
                y = Mathf.Max(LowerHandle.localPosition.y, y);
            y = Mathf.Clamp(y, -0.5f, 0.5f);

            position = transform.localPosition;
            position.y = y;
            transform.localPosition = position;
        }
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        interactor = args.interactorObject;
        interactorPoint = interactor.transform.InverseTransformPoint(transform.position);
        isGrabbing = true;
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        interactor = null;
        isGrabbing = false;
    }
}
