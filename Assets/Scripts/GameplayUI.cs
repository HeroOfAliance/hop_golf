using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : Panel
{
    [SerializeField]
    private Text _level;

    public void UpdateLevelNum(int level)
    {
        _level.text = $"LEVEL {level}";
    }

    public void OnRestart()
    {
        field.Restart();
    }
}
