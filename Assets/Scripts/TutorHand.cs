using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorHand : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _renderer;
    private Sequence _sequence;

    [SerializeField]
    private Color32 _levelOneHandColr;
    [SerializeField]
    private Color32 _levelTwoHandColr;

    private int? _tutorLevel;

    private static TutorHand _instance;

    private void Update()
    {
        if (_sequence == null && _tutorLevel.HasValue)
        {
            _sequence = GetSequence(_tutorLevel.Value, () => 
            {
                _sequence = null;
            });
        }
    }


    private Sequence GetSequence(int level, Action onComplete)
    {
        Sequence sequence = null;
        if (level == 1)
        {
            transform.localPosition = new Vector3(1.2f, 0, 0);
            transform.localScale = Vector3.one * 1.5f;
            _renderer.material.color = _levelOneHandColr;
            sequence = DOTween.Sequence();
            sequence.Append(_renderer.material.DOFade(1, 0.3f));
            sequence.Join(transform.DOScale(1, 0.3f));
            sequence.Append(transform.DOLocalMoveZ(3, 0.5f));
            sequence.Append(_renderer.material.DOFade(0, 0.3f));
            sequence.Join(transform.DOScale(1.5f, 0.3f));
            sequence.AppendCallback(() => onComplete?.Invoke());
        }
        else if (level == 2)
        {
            transform.localPosition = new Vector3(1.2f, 0, -2f);
            transform.localScale = Vector3.one * 1.5f;
            _renderer.material.color = _levelTwoHandColr;
            sequence = DOTween.Sequence();
            sequence.Append(_renderer.material.DOFade(1, 0.3f));
            sequence.Join(transform.DOScale(1, 0.3f));
            sequence.Append(transform.DOLocalMoveZ(1f, 0.3f));
            sequence.Append(transform.DOLocalMoveX(3, 0.3f));
            sequence.Append(_renderer.material.DOFade(0, 0.3f));
            sequence.Join(transform.DOScale(1.5f, 0.3f));
            sequence.AppendCallback(() => onComplete?.Invoke());
        }

        return sequence;
    }

    public void Init(int value)
    {
        gameObject.SetActive(true);
        _tutorLevel = value;
        _instance = this;
    }

    public static void Hide()
    {
        if (_instance)
        {
            GameObject.Destroy(_instance.gameObject);
            _instance = null;
        }
    }
}
