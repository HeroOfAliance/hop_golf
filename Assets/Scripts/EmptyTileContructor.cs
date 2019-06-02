 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTileContructor : MonoBehaviour
{
    [System.Serializable]
    private class SideData
    {
        [SerializeField]
        private GameObject _solid;
        [SerializeField]
        private GameObject _chamfer;

        public void Activate(bool side)
        {
            _solid.SetActive(side);
            _chamfer.SetActive(!side);
        }
    }

    [System.Serializable]
    private class CornerData
    {
        [SerializeField]
        private GameObject _horizontal;
        [SerializeField]
        private GameObject _vertical;
        [SerializeField]
        private GameObject _solid;
        [SerializeField]
        private GameObject _chamfer;
        [SerializeField]
        private GameObject _cap;

        public void Activate(bool horizontal, bool vertical, bool diag)
        {
            _cap.SetActive(horizontal && vertical && !diag);
            _solid.SetActive(horizontal && vertical && diag);
            _chamfer.SetActive(!horizontal && !vertical);
            _horizontal.SetActive(!vertical && horizontal);
            _vertical.SetActive(vertical && !horizontal);
        }
    }

    [SerializeField]
    private CornerData _leftBottom;
    [SerializeField]
    private CornerData _rightBottom;
    [SerializeField]
    private CornerData _leftTop;
    [SerializeField]
    private CornerData _rightTop;

    [SerializeField]
    private SideData _left;
    [SerializeField]
    private SideData _right;
    [SerializeField]
    private SideData _top;
    [SerializeField]
    private SideData _bottom;

    public LevelData.FiledUnit unit { get; private set; }

    public void Init(LevelData.FiledUnit unit)
    {
        return;
        this.unit = unit;
        var f = Controller.field;

        var top = f.levelData[unit.X, unit.Y +1].Type != FieldBlockType.Void;
        var bottom = f.levelData[unit.X, unit.Y -1].Type != FieldBlockType.Void;
        var left = f.levelData[unit.X -1, unit.Y].Type != FieldBlockType.Void;
        var right = f.levelData[unit.X + 1, unit.Y].Type != FieldBlockType.Void;

        _left.Activate(left);
        _right.Activate(right);
        _top.Activate(top);
        _bottom.Activate(bottom);

        _leftBottom.Activate(left, bottom, f.levelData[unit.X - 1, unit.Y - 1].Type != FieldBlockType.Void);
        _rightBottom.Activate(right, bottom, f.levelData[unit.X + 1, unit.Y - 1].Type != FieldBlockType.Void);
        _leftTop.Activate(left, top, f.levelData[unit.X - 1, unit.Y + 1].Type != FieldBlockType.Void);
        _rightTop.Activate(right, top, f.levelData[unit.X + 1, unit.Y + 1].Type != FieldBlockType.Void);

    }
}
