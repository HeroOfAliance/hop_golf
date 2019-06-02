using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : Controller
{
    [SerializeField]
    private FieldBlock _fieldBlock;

    [SerializeField]
    private Transform _fieldRoot;

    [SerializeField]
    private float _nodeSize;
    public LevelData levelData { get; private set; }

    private bool _editMode;

    private List<FieldBlock> _spawnedBlock = new List<FieldBlock>();


    private Tween _closeTween;
    private Tween _openTween;

    [SerializeField]
    private Camera _gameCamera;
    [SerializeField]
    private Camera _editorCamera;

    private void Start()
    {
        _fieldBlock.gameObject.SetActive(false);
        SetEditMode(false);
    }

    void Update()
    {

    }

    public void Restart()
    {
        Open(levelData, null, true);
    }

    private Vector3 offset => new Vector3(-levelData.Size.x / 2f * _nodeSize, 0, -levelData.Size.y / 2f * _nodeSize) + new Vector3(_nodeSize / 2f, 0, _nodeSize / 2f);

    public void Open(LevelData level, Action callback, bool soft = false)
    {
        if (_openTween != null)
        {
            _openTween.Kill();
            _openTween = null;
        }
        panelsManager.Get<GameplayUI>().UpdateLevelNum(level.Num);

        Close( ()=> 
        {
            if (!soft)
            {
                _fieldRoot.localPosition = Vector3.up * -30f;
            }

            levelData = level;

            FieldBlock start = null;

            var codes = ColorCode.instance.GetColors(level.Num);

            foreach (var item in level.Units)
            {
                var block = Instantiate(_fieldBlock);
                block.transform.SetParent(_fieldRoot);
                block.SetStartNode(false);
                block.transform.localPosition = new Vector3(_nodeSize * item.X, 0, _nodeSize * item.Y) + offset;
                block.gameObject.SetActive(true);
                block.SetUnit(item);
                block.SetType(item.Type);
                block.SetColors(codes);
                if (item.IsStart)
                {
                    start = block;
                }

                block.SetStartNode(false);
                _spawnedBlock.Add(block);
            }

            if (!soft)
            {
                _openTween = _fieldRoot.DOLocalMoveY(0, 0.3f).SetEase(Ease.OutQuint).OnComplete(() =>
                {
                    callback?.Invoke();
                    start?.SetStartNode(true);
                });
            }
            else
            {
                callback?.Invoke();
                start?.SetStartNode(true);
            }

        }, soft);
    }

    public void Close(Action callback, bool soft)
    {
        if (_closeTween != null)
        {
            _closeTween.Kill();
            _closeTween = null;
        }

        if (_spawnedBlock.Count > 0)
        {
            Action onComplete = () => 
            {
                foreach (var item in _spawnedBlock)
                {
                    if (item)
                    {
                        Destroy(item.gameObject);
                    }
                }
                _spawnedBlock.Clear();

                _closeTween = null;
                callback.Invoke();
            };

            _fieldRoot.localPosition = Vector3.zero;

            if (!soft)
            {
                _closeTween = _fieldRoot.DOLocalMoveY(10, 0.3f).SetEase(Ease.InBack).OnComplete(()=> onComplete() );
            }
            else
            {
                onComplete?.Invoke();
            }
        }
        else
        {
            callback?.Invoke();
        }
    }

    public void SetEditMode(bool active)
    {
        _editMode = active;
        _editorCamera.gameObject.SetActive(active);
        _gameCamera.gameObject.SetActive(!active);

    }

    public Vector3 GetPosition(int x, int y) => new Vector3(_nodeSize * x, 0, _nodeSize * y) + offset;
}
