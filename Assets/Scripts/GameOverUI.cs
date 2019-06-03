using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : Panel
{
    public void SwitchSound()
    {

    }

    public void RemoveAds()
    {

    }

    public void RestorePurchases()
    {

    }

    public void Leaderboards()
    {

    }

    public void Skins()
    {

    }
    
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
