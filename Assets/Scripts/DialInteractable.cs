using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DialInteractable : XRBaseInteractable
{
    public event Action ValueChanged;

    private IXRInteractor interactor;
    private Vector3 interactorUp;
    private Vector3 startingUp;
    private float startingAngle;
    private bool isGrabbing = false;

    public float Value
    {
        get
        {
            return transform.localEulerAngles.z.Remap(0, 180, 0, 1, true);
        }
        set
        {
            transform.localEulerAngles = transform.localEulerAngles.ChangeZ(value * 180f);
        }
    }

    private void Update()
    {
        if(isGrabbing)
        {
            var worldUp = interactor.transform.TransformDirection(interactorUp);
            var angle = Vector3.SignedAngle(startingUp, worldUp, transform.forward);
            var newAngle = startingAngle + angle;
            newAngle = Mathf.Clamp(newAngle, 0, 180);
            transform.localEulerAngles = transform.localEulerAngles.ChangeZ(newAngle);
            ValueChanged?.Invoke();
        }
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        interactor = args.interactorObject;
        interactorUp = interactor.transform.InverseTransformDirection(transform.up);
        startingUp = transform.up;
        startingAngle = transform.localEulerAngles.z;
        isGrabbing = true;
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        interactor = null;
        isGrabbing = false;
    }
}
