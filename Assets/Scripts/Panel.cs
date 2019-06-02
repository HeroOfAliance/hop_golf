using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Panel : Controller
{
    [SerializeField]
    private CanvasGroup _canvas;

    [SerializeField]
    private bool _openBuyDefault;

    protected virtual void OnOpen() { }
    protected virtual void OnClose() { }

    protected override void Awake()
    {
        base.Awake();
        if (!_openBuyDefault)
        {
            Close(null, true);
        }
    }

    public void Open(Action callback, bool instant)
    {
        gameObject.SetActive(true);

        if (instant)
        {
            _canvas.alpha = 1;
            OnOpen();
        }
        else
        {
            _canvas.DOFade(1, 0.3f).OnComplete( () => 
            {
                callback?.Invoke();
                OnOpen();
            });
        }
    }

    public void Close(Action callback, bool instant)
    {
        gameObject.SetActive(true);

        if (instant)
        {
            _canvas.alpha = 0;
            gameObject.SetActive(false);
            OnClose();
        }
        else
        {
            _canvas.DOFade(0, 0.3f).OnComplete(() =>
            {
                callback?.Invoke();
                gameObject.SetActive(false);
                OnClose();
            });
        }
    }
}
