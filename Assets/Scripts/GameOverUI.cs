using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : Panel
{
    public void OnNextLevel()
    {
        Close(() =>
        {
            gameController.NextLevel();
            panelsManager.Get<GameplayUI>().Open(null, false);
        }, false);
        
    }
}
