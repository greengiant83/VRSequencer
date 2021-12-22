using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;



public interface IClickable
{
    void OnClick();
}
public class MouseInteractor : MonoBehaviour
{
    private Camera mainCamera;
    private RaycastHit hitInfo;
    private IXRInteractable interactable;
    private IXRHoverInteractable hoverInteractable;
    private IXRSelectInteractable selectInteractable;
    private bool hasHit;
    private Collider currentCollider;
    private XRInteractionManager interactionManager;

    private void Start()
    {
        mainCamera = Camera.main;
        if (interactionManager == null) interactionManager = FindObjectOfType<XRInteractionManager>();
    }

    private void Update()
    {
        updateRaycast();
    }

    private void updateRaycast()
    {
        var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ToVector2());
        hasHit = Physics.Raycast(ray, out hitInfo);
        if (hasHit)
        {
            if (hitInfo.collider != currentCollider)
            {
                interactionManager.TryGetInteractableForCollider(hitInfo.collider, out interactable);
                hoverInteractable = interactable as IXRHoverInteractable;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                selectInteractable = interactable as IXRSelectInteractable;
                if (selectInteractable != null)
                {
                    var clickable = selectInteractable.transform.GetComponent<IClickable>();
                    clickable?.OnClick();
                }
            }

            currentCollider = hitInfo.collider;
        }
        else
        {
            hoverInteractable = null;
            currentCollider = null;
        }
    }
}



//public class MouseInteractor : XRBaseInteractor, IXRActivateInteractor
//{
//    private Camera mainCamera;
//    private RaycastHit hitInfo;
//    private IXRInteractable interactable;
//    private IXRHoverInteractable hoverInteractable;
//    private IXRActivateInteractable activateInteractable;
//    private bool hasHit;
//    private Collider currentCollider;

//    public bool shouldActivate => Mouse.current.leftButton.wasPressedThisFrame;

//    public bool shouldDeactivate => Mouse.current.leftButton.wasReleasedThisFrame;

//    public void GetActivateTargets(List<IXRActivateInteractable> targets)
//    {
//        Debug.Log("GetActivateTargets");
//        if(activateInteractable != null && !targets.Contains(activateInteractable))
//        {
//            Debug.Log("Adding target: " + activateInteractable.transform.gameObject.name);
//            targets.Add(activateInteractable);
//        }
//    }

//    protected override void Start()
//    {
//        base.Start();
//        mainCamera = Camera.main;
//    }

//    private void Update()
//    {
//        updateRaycast();
//    }

//    public override void GetValidTargets(List<IXRInteractable> targets)
//    {
//        base.GetValidTargets(targets);
//        if (hoverInteractable != null && !targets.Contains(hoverInteractable))
//        {
//            targets.Add(hoverInteractable);
//        }
//    }

//    public override bool CanSelect(IXRSelectInteractable interactable)
//    {
//        if (Mouse.current.leftButton.wasPressedThisFrame) Debug.Log("Mouse can select: " + interactable.transform.gameObject.name);
//        return Mouse.current.leftButton.wasPressedThisFrame;
//    }


//    private void updateRaycast()
//    {
//        var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ToVector2());
//        hasHit = Physics.Raycast(ray, out hitInfo);
//        if (hasHit)
//        {
//            if (hitInfo.collider != currentCollider)
//            {
//                interactionManager.TryGetInteractableForCollider(hitInfo.collider, out interactable);
//                hoverInteractable = interactable as IXRHoverInteractable;
//                activateInteractable = interactable as IXRActivateInteractable;
//            }

//            //if (Mouse.current.leftButton.wasPressedThisFrame)
//            //{
//            //    selectInteractable = interactable as IXRSelectInteractable;
//            //    if (selectInteractable != null)
//            //    {
//            //        var clickable = selectInteractable.transform.GetComponent<IClickable>();
//            //        clickable?.OnClick();
//            //    }
//            //}

//            currentCollider = hitInfo.collider;
//        }
//        else
//        {
//            hoverInteractable = null;
//            activateInteractable = null;
//            currentCollider = null;
//        }
//    }

//}
