using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : Controller
{

    public static bool active { get; set; }

    private enum SwipeDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    [SerializeField]
    private GameObject _winParticle;
    [SerializeField]
    private MeshRenderer _renderer;
    private Vector3 _swipeStart;
    private Vector2Int _pos;
    private bool _inTransition;

    // Start is called before the first frame update
    void Start()
    {
        var start = field.levelData.Units.First(u => u.IsStart);
        _pos = new Vector2Int(start.X, start.Y);

        transform.localPosition += Vector3.up * 3f;
        var sourceColor = _renderer.material.color;
        _renderer.material.color = new Color(sourceColor.r, sourceColor.g, sourceColor.b, 0);

        _inTransition = true;
        var seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMoveY(transform.localPosition.y - 3f, 0.3f).SetEase(Ease.OutBounce));
        seq.Join(_renderer.material.DOFade(1, 0.5f));
        seq.AppendCallback(() =>
        {
            _inTransition = false;
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _swipeStart = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            var delta = Input.mousePosition - _swipeStart;
            if (delta.magnitude > Screen.width / 5f)
            {
                _swipeStart = Input.mousePosition;
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                {
                    if (delta.x < 0)
                    {
                        OnSwipe(SwipeDirection.Left);
                    }
                    else
                    {
                        OnSwipe(SwipeDirection.Right);
                    }
                }
                else
                {
                    if (delta.y > 0)
                    {
                        OnSwipe(SwipeDirection.Up);
                    }
                    else
                    {
                        OnSwipe(SwipeDirection.Down);
                    }
                }
            }
        }
    }

    private void OnSwipe(SwipeDirection dir)
    {
        if (_inTransition)
        {
            return;
        }

        var dt = field.levelData;

        Vector2Int swipeEnd = Vector2Int.zero;
        bool die = false, portal = false;
        switch (dir)
        {
            case SwipeDirection.Up:
                for (int i = _pos.y; i <= dt.Size.y; i++)
                {
                    var item = dt[_pos.x, i];
                    if (item.Type == FieldBlockType.Blocker)
                    {
                        swipeEnd = new Vector2Int(_pos.x, i - 1);
                        break;
                    }

                    if (item.Type == FieldBlockType.Portal || item.Type == FieldBlockType.Void)
                    {
                        swipeEnd = new Vector2Int(_pos.x, i);

                        die = item.Type == FieldBlockType.Void;
                        portal = item.Type == FieldBlockType.Portal;
                        break;
                    }
                }
                break;
            case SwipeDirection.Down:
                for (int i = _pos.y; i >= -1; i--)
                {
                    var item = dt[_pos.x, i];
                    if (item.Type == FieldBlockType.Blocker)
                    {
                        swipeEnd = new Vector2Int(_pos.x, i + 1);
                        break;
                    }

                    if (item.Type == FieldBlockType.Portal || item.Type == FieldBlockType.Void)
                    {
                        swipeEnd = new Vector2Int(_pos.x, i);

                        die = item.Type == FieldBlockType.Void;
                        portal = item.Type == FieldBlockType.Portal;
                        break;
                    }
                }

                break;
            case SwipeDirection.Left:
                for (int i = _pos.x; i >= -1; i--)
                {
                    var item = dt[i, _pos.y];
                    if (item.Type == FieldBlockType.Blocker)
                    {
                        swipeEnd = new Vector2Int(i + 1, _pos.y);
                        break;
                    }

                    if (item.Type == FieldBlockType.Portal || item.Type == FieldBlockType.Void)
                    {
                        swipeEnd = new Vector2Int(i, _pos.y);

                        die = item.Type == FieldBlockType.Void;
                        portal = item.Type == FieldBlockType.Portal;
                        break;
                    }
                }
                break;
            case SwipeDirection.Right:
                for (int i = _pos.x; i <= dt.Size.x; i++)
                {
                    var item = dt[i, _pos.y];
                    if (item.Type == FieldBlockType.Blocker)
                    {
                        swipeEnd = new Vector2Int(i - 1, _pos.y);
                        break;
                    }

                    if (item.Type == FieldBlockType.Portal || item.Type == FieldBlockType.Void)
                    {
                        swipeEnd = new Vector2Int(i, _pos.y);

                        die = item.Type == FieldBlockType.Void;
                        portal = item.Type == FieldBlockType.Portal;
                        break;
                    }
                }
                break;
        }

        if (swipeEnd.x == _pos.x && swipeEnd.y == _pos.y)
        {
            return;
        }

        _inTransition = true;

        var moveTime = 0.3f;

        Sequence moveSeq = DOTween.Sequence();
        moveSeq.Append(transform.DOMove(field.GetPosition(swipeEnd.x, swipeEnd.y), moveTime).SetEase(Ease.OutQuad));

        transform.localRotation = Quaternion.identity;

        switch (dir)
        {
            case SwipeDirection.Down:
                moveSeq.Join(transform.DOLocalRotateQuaternion(transform.localRotation * Quaternion.Euler(Vector3.left * 179), moveTime));
                break;
            case SwipeDirection.Up:
                moveSeq.Join(transform.DOLocalRotateQuaternion(transform.localRotation * Quaternion.Euler(Vector3.left * -179), moveTime));
                break;
            case SwipeDirection.Left:
                moveSeq.Join(transform.DOLocalRotateQuaternion(transform.localRotation * Quaternion.Euler(Vector3.forward * 179), moveTime));
                break;
            case SwipeDirection.Right:
                moveSeq.Join(transform.DOLocalRotateQuaternion(transform.localRotation * Quaternion.Euler(Vector3.forward * -179), moveTime));
                break;
        }

        moveSeq.AppendCallback(() =>
        {
            _inTransition = false;
            _pos = swipeEnd;

            if (die)
            {
                var seq = DOTween.Sequence();
                seq.Append(transform.DOLocalMoveY(transform.localPosition.y - 10f, 0.5f));
                seq.Join(_renderer.material.DOFade(0, 0.5f));
                seq.AppendCallback(() =>
                {
                    OnDie();
                });
            }
            if (portal)
            {
                transform.DOLocalMoveY(transform.localPosition.y - 1f, 0.2f).OnComplete(() =>
                {
                    OnWin();
                });
            }
        });

        if (!die && !portal)
        {
            switch (dir)
            {
                case SwipeDirection.Down:
                    transform.DOPunchPosition(new Vector3(0, 0, 0.2f), 0.2f).SetDelay(moveTime);
                    transform.DOPunchScale(new Vector3(0, 0, -0.2f), 0.2f,0).SetDelay(moveTime);
                    break;
                case SwipeDirection.Up:
                    transform.DOPunchPosition(new Vector3(0, 0, -0.2f), 0.2f).SetDelay(moveTime);
                    transform.DOPunchScale(new Vector3(0, 0, -0.2f), 0.2f, 0).SetDelay(moveTime);
                    break;
                case SwipeDirection.Left:
                    transform.DOPunchPosition(new Vector3(-0.2f, 0, 0), 0.2f).SetDelay(moveTime);
                    transform.DOPunchScale(new Vector3(-0.2f, 0, 0), 0.2f, 0).SetDelay(moveTime);
                    break;
                case SwipeDirection.Right:
                    transform.DOPunchPosition(new Vector3(0.2f, 0, 0), 0.2f).SetDelay(moveTime);
                    transform.DOPunchScale(new Vector3(-0.2f, 0, 0), 0.2f, 0).SetDelay(moveTime);
                    break;
            }
        }
    }

    private void OnDie()
    {
        field.Restart();
    }

    private void OnWin()
    {
        panelsManager.Get<GameplayUI>().Close(() => 
        {
            var gameOver = panelsManager.Get<GameOverUI>();
            gameOver.Open(null, false);
            //gameOver.UpdateLevelNum(gameController.lastPassedLevel);
            gameController.NextLevel();
        }, false);
        
    }
}
