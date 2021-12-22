using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonInteractable : XRSimpleInteractable, IClickable
{
    private ActionBasedController controller;
    private IXRInteractor interactor;

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        if (controller != null) return;
        var hoverController = args.interactorObject.transform.gameObject.GetComponentInParent<ActionBasedController>();
        if(hoverController != null)
        {
            controller = hoverController;
            interactor = args.interactorObject;
            hoverController.activateAction.action.performed += Action_performed;
        }
    }

    private void Action_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //var e = new ActivateEventArgs() { interactable = this, interactor = interactor };
        //this.OnActivated(e);



        OnActivated(new ActivateEventArgs() { interactableObject = this, interactorObject = interactor as IXRActivateInteractor });


        ////this.activated.Invoke() .Invoke(e);
        //this.activated.Invoke(new ActivateEventArgs() { interactorObject = interactor as IXRActivateInteractor, interactableObject = this });
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        if (controller == null) return;

        var hoverController = args.interactorObject.transform.gameObject.GetComponentInParent<ActionBasedController>();
        if (hoverController == controller)
        {
            hoverController.activateAction.action.performed -= Action_performed;
            controller = null;
        }
    }

    public void OnClick()
    {
        OnActivated(new ActivateEventArgs() { interactableObject = this });
    }

    //public override bool IsSelectableBy(XRBaseInteractor interactor) //I commented this out to make it work with MouseInteractor
    //{
    //    return false;
    //}
}
