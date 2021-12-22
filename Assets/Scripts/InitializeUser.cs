using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeUser : MonoBehaviour
{
    [SerializeField] private Transform MainCamera;
    [SerializeField] private Transform CameraOffset;

    private void Start()
    {
        initializePose();
    }

    private void initializePose()
    {
        Debug.DrawRay(MainCamera.position, MainCamera.forward, Color.red, 60);
        //var visualLocalRot = Quaternion.Inverse(transform.rotation) * viewModel.Visual.transform.rotation;

        //var rotOffset = TargetPosition.localRotation * Quaternion.Inverse(visualLocalRot);
        //var targetRot = rotOffset * Offset.localRotation;
        //var originalRot = Offset.localRotation;
        //Offset.localRotation = targetRot;
    }
}
