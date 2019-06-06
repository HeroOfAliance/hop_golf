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
        GameAnalyticsSDK.GameAnalytics.Initialize();

        StartLevel(lastPassedLevel + 1, true);
        PlayerController.active = true;
        Social.localUser.Authenticate((result) => { });
    }

    public void StartLevel(int level, bool soft)
    {
        analytics.StartLevel(level);
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
        analytics.FinishLevel(_level);
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(_level, "Progress", (onsuccess) => { });
        }
        lastPassedLevel = _level;
        StartLevel(_level + 1, false);
    }
}
