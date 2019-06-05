using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Controller
{

    public int lastPassedLevel
    {
        get
        {
            return PlayerPrefs.GetInt("lastPassedLevel", 0);
        }
        set
        {
            PlayerPrefs.SetInt("lastPassedLevel", value);
            PlayerPrefs.Save();
        }
    }

    internal void ResetAll()
    {
        lastPassedLevel = 0;
        Start();
    }

    private int _level;

    private void Start()
    {
        StartLevel(lastPassedLevel + 1, true);
        PlayerController.active = true;
    }

    public void StartLevel(int level, bool soft)
    {
        _level = level;

        var levelAsset = Resources.Load<TextAsset>($"Levels/{level}");
        if (levelAsset != null)
        {
            var levelData = JsonUtility.FromJson<LevelData>(levelAsset.text);
            levelData.Num = level;
            field.Open(levelData, null, soft);
        }
        else
        {
            panelsManager.Get<NoMoreLevelsUI>().Open(null, false);
        }
    }

    public void NextLevel()
    {
        lastPassedLevel = _level;
        StartLevel(_level + 1, false);
    }
}
