using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Routiner : Controller
{
    public void CallOnNextFrame(Action callback)
    {
        StartCoroutine(CallOnNextFrameRoutine(callback));
    }

    public void CallLater(Action callback, float dellay)
    {
        StartCoroutine(CallLaterRoutine(callback, dellay));
    }

    private IEnumerator CallOnNextFrameRoutine(Action callback)
    {
        yield return new WaitForEndOfFrame();
        callback?.Invoke();
    }

    private IEnumerator CallLaterRoutine(Action callback, float dellay)
    {
        yield return new WaitForSeconds(dellay);
        callback?.Invoke();
    }
}
