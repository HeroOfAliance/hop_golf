using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMoreLevelsUI : Panel
{

    public void OnRestartAll()
    {
        Close(null, false);
        gameController.ResetAll();
    }
}
