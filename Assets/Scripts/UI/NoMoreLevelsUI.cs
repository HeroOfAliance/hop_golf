using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMoreLevelsUI : Panel
{
    [SerializeField]
    private Color32 _noMoreLevelsColor;

    protected override void OnPreOpen()
    {
        uiColors.UpdateColors(_noMoreLevelsColor);
    }

    public void OnRestartAll()
    {
        Close(null, false);
        gameController.ResetAll();
    }
}
