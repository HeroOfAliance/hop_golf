using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Controller
{
    private int _level;
    public void StartLevel(int level)
    {
        _level = level;

        var levelAsset = Resources.Load<TextAsset>($"Levels/{level}");
        if (levelAsset != null)
        {
            var levelData = JsonUtility.FromJson<LevelData>(levelAsset.text);
            levelData.Num = level;
            field.Open(levelData, null);
        }
    }

    public void NextLevel()
    {
        StartLevel(_level + 1);
    }
}
