using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Tile : ButtonInteractable
{
    private const float animateDuration = 0.2f;

    private readonly Quaternion onRotation = Quaternion.Euler(90, 0, 0);
    private readonly Quaternion offRotation = Quaternion.Euler(-90, 0, 0);
    private Coroutine animateRotationCoroutine;

    public event Action<Tile> Changed;

    private bool _isOn;
    public bool IsOn 
    { 
        get { return _isOn; }
        set
        {
            _isOn = value;
            AnimateRotation(value ? onRotation : offRotation);
        }
    }

    public int Row { get; set; }
    
    public int Col { get; set; }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);

        IsOn = !IsOn;
        AnimateRotation(IsOn ? onRotation : offRotation);
        Changed?.Invoke(this);
    }

    private void AnimateRotation(Quaternion targetRotation)
    {
        AnimationHelper.StartAnimation(this, ref animateRotationCoroutine, animateDuration, transform.localRotation, targetRotation, i => transform.localRotation = i);
    }
}
