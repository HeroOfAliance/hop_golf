using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : Panel
{

    protected override void OnOpen()
    {
        PlayerController.active = false;
    }

    public void OnNextLevel()
    {
        Close(() =>
        {
            panelsManager.Get<GameplayUI>().Open(null, false);
        }, false);
        PlayerController.active = true;   
    }
}
