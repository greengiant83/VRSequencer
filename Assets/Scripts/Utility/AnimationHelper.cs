using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class AnimationHelper
{
    /////// Float
    public static async Task StartAnimation(MonoBehaviour Source, float Duration, float FromValue, float ToValue, Action<float> UseValue, AnimationCurve curve = null)
    {
        bool isComplete = false;

        Source.StartCoroutine(Animate(Duration, FromValue, ToValue, UseValue, () => isComplete = true, curve));
        await Task.Run(() =>
        {
            while (!isComplete)
            {
            }
        });
    }

    public static void StartAnimation(MonoBehaviour Source, ref Coroutine AnimationCoroutine, float Duration, float FromValue, float ToValue, Action<float> UseValue, Action OnCompleteCallback = null, AnimationCurve curve = null)
    {
        if (AnimationCoroutine != null) Source.StopCoroutine(AnimationCoroutine);

        AnimationCoroutine = Source.StartCoroutine(Animate(Duration, FromValue, ToValue, UseValue, OnCompleteCallback, curve));
    }

    public static IEnumerator Animate(float Duration, float FromValue, float ToValue, Action<float> UseValue, Action OnCompleteCallback = null, AnimationCurve curve = null)
    {
        float elapsed = 0;
        float t;

        while (elapsed <= Duration)
        {
            t = elapsed / Duration;
            if (curve != null) t = curve.Evaluate(t);

            UseValue(Mathf.Lerp(FromValue, ToValue, t));

            yield return null;
            elapsed += Time.deltaTime;
        }

        UseValue(ToValue);
        OnCompleteCallback?.Invoke();
    }

    ////// Quaternion
    public static async Task StartAnimation(MonoBehaviour Source, float Duration, Quaternion FromValue, Quaternion ToValue, Action<Quaternion> UseValue, AnimationCurve curve = null)
    {
        bool isComplete = false;

        Source.StartCoroutine(Animate(Duration, FromValue, ToValue, UseValue, () => isComplete = true, curve));
        await Task.Run(() =>
        {
            while (!isComplete)
            {
            }
        });
    }

    public static void StartAnimation(MonoBehaviour Source, ref Coroutine AnimationCoroutine, float Duration, Quaternion FromValue, Quaternion ToValue, Action<Quaternion> UseValue, Action OnCompleteCallback = null, AnimationCurve curve = null)
    {
        if (AnimationCoroutine != null) Source.StopCoroutine(AnimationCoroutine);

        AnimationCoroutine = Source.StartCoroutine(Animate(Duration, FromValue, ToValue, UseValue, OnCompleteCallback, curve));
    }

    public static IEnumerator Animate(float Duration, Quaternion FromValue, Quaternion ToValue, Action<Quaternion> UseValue, Action OnCompleteCallback = null, AnimationCurve curve = null)
    {
        float elapsed = 0;
        float t;

        while (elapsed <= Duration)
        {
            t = elapsed / Duration;
            if (curve != null) t = curve.Evaluate(t);

            UseValue(Quaternion.Slerp(FromValue, ToValue, t));

            yield return null;
            elapsed += Time.deltaTime;
        }

        UseValue(ToValue);
        OnCompleteCallback?.Invoke();
    }
}