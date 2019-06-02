using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : Panel
{
    public void OnPlay()
    {
        gameController.StartLevel(1, true);
        Close(() => 
        {
            panelsManager.Get<GameplayUI>().Open(null, false);
        }, false);
    }
}
