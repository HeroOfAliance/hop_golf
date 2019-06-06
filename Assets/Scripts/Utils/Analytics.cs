using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analytics : Controller
{
    public void StartLevel(int level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, Application.version, level.ToString("00000"));
    }

    public void FinishLevel(int level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, Application.version, level.ToString("00000"));
    }
}
