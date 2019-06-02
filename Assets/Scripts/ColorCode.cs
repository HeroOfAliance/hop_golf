using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ColorCodes", menuName ="CororCode config")]
public class ColorCode : ScriptableObject
{
    [System.Serializable]
    public class ColorPair
    {
        [SerializeField]
        private Color32 _ground;
        [SerializeField]
        private Color32 _obstacles;

        public Color32 ground => _ground;
        public Color32 obstacles => _obstacles;
    }


    [SerializeField]
    private List<ColorPair> _colors;

    private static ColorCode _instance;

    public static ColorCode instance
    {
        get
        {
            if (!_instance)
            {
                _instance = Resources.Load<ColorCode>("ColorCodes");
            }
            return _instance;
        }
    }

    private int _last;

    public ColorPair GetColors(int level)
    {
        return _colors[level % _colors.Count];
    }

}
