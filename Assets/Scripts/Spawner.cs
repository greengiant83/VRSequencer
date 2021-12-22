using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Spawner : MonoBehaviour
{
    private const float HandsTogetherDistanceThreshold = 0.15f;

    [SerializeField] private GameObject ItemPrefab;
    [SerializeField] private Transform LeftController;
    [SerializeField] private Transform RightController;
    [SerializeField] private InputActionProperty LeftGripAction;
    [SerializeField] private InputActionProperty RightGripAction;
    
    public static bool IsSpawning { get; private set; }

    private Sequencer currentItem;
    private IXRSelectInteractor leftInteractor;
    private IXRSelectInteractor rightInteractor;

    private void Start()
    {
        leftInteractor = LeftController.GetComponentInParent<IXRSelectInteractor>();
        rightInteractor = RightController.GetComponentInParent<IXRSelectInteractor>();

        LeftGripAction.action.started += LeftGrip_started;
        LeftGripAction.action.canceled += LeftGrip_canceled;

        RightGripAction.action.started += RightGrip_started;
        RightGripAction.action.canceled += RightGrip_canceled;
    }

    private void Update()
    {
        if(IsSpawning)
        {
            currentItem.transform.rotation = Quaternion.Slerp(LeftController.rotation, RightController.rotation, 0.5f);
            currentItem.transform.position = Vector3.Lerp(LeftController.position, RightController.position, 0.5f);

            var localLeft = Quaternion.Inverse(currentItem.transform.rotation) * LeftController.position;
            var localRight = Quaternion.Inverse(currentItem.transform.rotation) * RightController.position;
            var localDelta = localRight - localLeft;

            currentItem.SetSize(localDelta);
        }
    }

    private void checkSpawnStart()
    {
        if (leftInteractor.hasSelection || rightInteractor.hasSelection) return;

        if(LeftGripAction.action.IsPressed() && RightGripAction.action.IsPressed() && Vector3.Distance(LeftController.position, RightController.position) <= HandsTogetherDistanceThreshold)
        {
            currentItem = Instantiate(ItemPrefab).GetComponent<Sequencer>();
            currentItem.Select();
            currentItem.IsSpawning = true;
            IsSpawning = true;
        }
    }

    private void gripEnd()
    {
        if(currentItem != null) currentItem.IsSpawning = false;
        IsSpawning = false;
        currentItem = null;
    }

    private void LeftGrip_started(InputAction.CallbackContext obj)
    {
        checkSpawnStart();
    }

    private void RightGrip_started(InputAction.CallbackContext obj)
    {
        checkSpawnStart();
    }
    
    private void LeftGrip_canceled(InputAction.CallbackContext obj)
    {
        gripEnd();
    }

    private void RightGrip_canceled(InputAction.CallbackContext obj)
    {
        gripEnd();
    }
}
