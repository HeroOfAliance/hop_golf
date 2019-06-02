using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    [System.Serializable]
    public class FiledUnit
    {
        public int X;
        public int Y;
        public FieldBlockType Type;
        public bool IsStart;
    }

    public Vector2Int Size;
    public float CameraOffset;
    [SerializeField]
    public List<FiledUnit> Units;
    [System.NonSerialized]
    public int Num;

    public FiledUnit this[int x, int y]
    {
        get
        {
            return Units.FirstOrDefault(u => u.X == x && u.Y == y)??new FiledUnit { Type = FieldBlockType.Void, X = x, Y = y };
        }
    }

    public static LevelData Create(Vector2Int size)
    {
        var level = new LevelData
        {
            Size = size,
        };

        level.Units = new List<FiledUnit>();

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                level.Units.Add(new FiledUnit
                {
                    IsStart = false,
                    //Type = x == 0 || x == size.x - 1 || y == 0 || y == size.y - 1 ? FieldBlockType.Void : FieldBlockType.Empty,
                    Type = FieldBlockType.Empty,
                    X = x,
                    Y = y
                });
            }
        }

        return level;
    }

}
