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

    protected virtual void OnPreOpen() { }
    protected virtual void OnPreClose() { }

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
        OnPreOpen();

        if (instant)
        {
            _canvas.alpha = 1;
            transform.localScale = Vector3.one;
            OnOpen();
        }
        else
        {
            _canvas.alpha = 0;
            transform.localScale = Vector3.one * 0.5f;
            _canvas.DOFade(1, 0.3f);
            transform.DOScale(1, 0.3f).SetEase(Ease.InOutSine).OnComplete( () => 
            {
                callback?.Invoke();
                OnOpen();
            });
        }
    }

    public void Close(Action callback, bool instant)
    {
        gameObject.SetActive(true);
        OnPreClose();
        if (instant)
        {
            _canvas.alpha = 0;
            transform.localScale = Vector3.zero;
            gameObject.SetActive(false);
            OnClose();
        }
        else
        {
            _canvas.alpha = 1;
            transform.localScale = Vector3.one;
            _canvas.DOFade(0, 0.3f);
            transform.DOScale(1.5f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                callback?.Invoke();
                gameObject.SetActive(false);
                OnClose();
            });
        }
    }
}
